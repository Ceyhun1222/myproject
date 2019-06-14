﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Aran.Temporality.Internal.Util
{

    /// <summary>
    /// MemoryStream implementation that deals with pooling and managing memory streams which use potentially large
    /// buffers.
    /// </summary>
    /// <remarks>
    /// This class works in tandem with the RecylableMemoryStreamManager to supply MemoryStream
    /// objects to callers, while avoiding these specific problems:
    /// 1. LOH allocations - since all large buffers are pooled, they will never incur a Gen2 GC
    /// 2. Memory waste - A standard memory stream doubles its size when it runs out of room. This
    /// leads to continual memory growth as each stream approaches the maximum allowed size.
    /// 3. Memory copying - Each time a MemoryStream grows, all the bytes are copied into new buffers.
    /// This implementation only copies the bytes when GetBuffer is called.
    /// 4. Memory fragmentation - By using homogenous buffer sizes, it ensures that blocks of memory
    /// can be easily reused.
    /// 
    /// The stream is implemented on top of a series of uniformly-sized blocks. As the stream's length grows,
    /// additional blocks are retrieved from the memory manager. It is these blocks that are pooled, not the stream
    /// object itself.
    /// 
    /// The biggest wrinkle in this implementation is when GetBuffer() is called. This requires a single 
    /// contiguous buffer. If only a single block is in use, then that block is returned. If multiple blocks 
    /// are in use, we retrieve a larger buffer from the memory manager. These large buffers are also pooled, 
    /// split by size--they are multiples of a chunk size (1 MB by default).
    /// 
    /// Once a large buffer is assigned to the stream the blocks are NEVER again used for this stream. All operations take place on the 
    /// large buffer. The large buffer can be replaced by a larger buffer from the pool as needed. All blocks and large buffers 
    /// are maintained in the stream until the stream is disposed (unless AggressiveBufferReturn is enabled in the stream manager).
    /// 
    /// </remarks>
    public sealed class RecyclableMemoryStream : MemoryStream
    {
        private const long MaxStreamLength = Int32.MaxValue;

        /// <summary>
        /// All of these blocks must be the same size
        /// </summary>
        private readonly List<byte[]> blocks = new List<byte[]>(1);

        /// <summary>
        /// This is only set by GetBuffer() if the necessary buffer is larger than a single block size, or on
        /// construction if the caller immediately requests a single large buffer.
        /// </summary>
        /// <remarks>If this field is non-null, it contains the concatenation of the bytes found in the individual
        /// blocks. Once it is created, this (or a larger) largeBuffer will be used for the life of the stream.
        /// </remarks>
        private byte[] largeBuffer;

        /// <summary>
        /// This list is used to store buffers once they're replaced by something larger.
        /// This is for the cases where you have users of this class that may hold onto the buffers longer
        /// than they should and you want to prevent race conditions which could corrupt the data.
        /// </summary>
        private List<byte[]> dirtyBuffers;
        
        private readonly Guid id;
        /// <summary>
        /// Unique identifier for this stream across it's entire lifetime
        /// </summary>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        internal Guid Id { get { CheckDisposed(); return id; } }

        private readonly string tag;
        /// <summary>
        /// A temporary identifier for the current usage of this stream.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        internal string Tag { get { CheckDisposed(); return tag; } }

        private readonly RecyclableMemoryStreamManager memoryManager;

        /// <summary>
        /// Gets the memory manager being used by this stream.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        internal RecyclableMemoryStreamManager MemoryManager
        {
            get
            {
                CheckDisposed();
                return memoryManager;
            }
        }

        private bool disposed;

        /// <summary>
        /// Callstack of the constructor. It is only set if MemoryManager.GenerateCallStacks is true,
        /// which should only be in debugging situations.
        /// </summary>
        internal string AllocationStack { get; }

        /// <summary>
        /// Callstack of the Dispose call. It is only set if MemoryManager.GenerateCallStacks is true,
        /// which should only be in debugging situations.
        /// </summary>
        internal string DisposeStack { get; private set; }

        /// <summary>
        /// This buffer exists so that WriteByte can forward all of its calls to Write
        /// without creating a new byte[] buffer on every call.
        /// </summary>
        private readonly byte[] byteBuffer = new byte[1];

        #region Constructors
        /// <summary>
        /// Allocate a new RecyclableMemoryStream object.
        /// </summary>
        /// <param name="memoryManager">The memory manager</param>
        public RecyclableMemoryStream(RecyclableMemoryStreamManager memoryManager)
            : this(memoryManager, null)
        {

        }

        /// <summary>
        /// Allocate a new RecyclableMemoryStream object
        /// </summary>
        /// <param name="memoryManager">The memory manager</param>
        /// <param name="tag">A string identifying this stream for logging and debugging purposes</param>
        public RecyclableMemoryStream(RecyclableMemoryStreamManager memoryManager, string tag)
            : this(memoryManager, tag, 0)
        {

        }

        /// <summary>
        /// Allocate a new RecyclableMemoryStream object
        /// </summary>
        /// <param name="memoryManager">The memory manager</param>
        /// <param name="tag">A string identifying this stream for logging and debugging purposes</param>
        /// <param name="requestedSize">The initial requested size to prevent future allocations</param>
        public RecyclableMemoryStream(RecyclableMemoryStreamManager memoryManager, string tag, int requestedSize)
            : this(memoryManager, tag, requestedSize, null)
        {
        }

        /// <summary>
        /// Allocate a new RecyclableMemoryStream object
        /// </summary>
        /// <param name="memoryManager">The memory manager</param>
        /// <param name="tag">A string identifying this stream for logging and debugging purposes</param>
        /// <param name="requestedSize">The initial requested size to prevent future allocations</param>
        /// <param name="initialLargeBuffer">An initial buffer to use. This buffer will be owned by the stream and returned to the memory manager upon Dispose.</param>
        internal RecyclableMemoryStream(RecyclableMemoryStreamManager memoryManager, string tag, int requestedSize,
                                      byte[] initialLargeBuffer)
        {
            this.memoryManager = memoryManager;
            id = Guid.NewGuid();
            this.tag = tag;

            if (requestedSize < memoryManager.BlockSize)
            {
                requestedSize = memoryManager.BlockSize;
            }

            if (initialLargeBuffer == null)
            {
                EnsureCapacity(requestedSize);
            }
            else
            {
                largeBuffer = initialLargeBuffer;
            }

            disposed = false;

            if (this.memoryManager.GenerateCallStacks)
            {
                AllocationStack = Environment.StackTrace;
            }

            this.memoryManager.ReportStreamCreated();
        }
        #endregion

        #region Dispose and Finalize
        
        ~RecyclableMemoryStream()
        {
            Dispose(false);
        }

        /// <summary>
        /// Returns the memory used by this stream back to the pool.
        /// </summary>
        /// <param name="disposing">Whether we're disposing (true), or being called by the finalizer (false)</param>
        /// <remarks>This method is not thread safe and it may not be called more than once.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "We have different disposal semantics, so SuppressFinalize is in a different spot.")]
        protected override void Dispose(bool disposing)
        {
            if (disposed)
            {
                string doubleDisposeStack = null;
                if (memoryManager.GenerateCallStacks)
                {
                    doubleDisposeStack = Environment.StackTrace;
                }

                throw new InvalidOperationException("Cannot dispose of RecyclableMemoryStream twice");
            }
            

            if (memoryManager.GenerateCallStacks)
            {
                DisposeStack = Environment.StackTrace;
            }

            if (disposing)
            {
                // Once this flag is set, we can't access any properties -- use fields directly
                disposed = true;

                memoryManager.ReportStreamDisposed();
                                
                GC.SuppressFinalize(this);
            }
            else
            {
                // We're being finalized.


                if (AppDomain.CurrentDomain.IsFinalizingForUnload())
                {
                    // If we're being finalized because of a shutdown, don't go any further.
                    // We have no idea what's already been cleaned up. Triggering events may cause
                    // a crash.
                    base.Dispose(disposing);
                    return;
                }
                
                memoryManager.ReportStreamFinalized(); 
            }

            memoryManager.ReportStreamLength(length);

            if (largeBuffer != null)
            {
                memoryManager.ReturnLargeBuffer(largeBuffer, tag);
            }

            if (dirtyBuffers != null)
            {
                foreach (var buffer in dirtyBuffers)
                {
                    memoryManager.ReturnLargeBuffer(buffer, tag);
                }
            }

            memoryManager.ReturnBlocks(blocks, tag);
            
            base.Dispose(disposing);
        }

        /// <summary>
        /// Equivalent to Dispose
        /// </summary>
        public override void Close()
        {
            Dispose(true);
        }

        #endregion

        #region MemoryStream overrides
        /// <summary>
        /// Gets or sets the capacity
        /// </summary>
        /// <remarks>Capacity is always in multiples of the memory manager's block size, unless
        /// the large buffer is in use.  Capacity never decreases during a stream's lifetime. 
        /// Explicitly setting the capacity to a lower value than the current value will have no effect. 
        /// This is because the buffers are all pooled by chunks and there's little reason to 
        /// allow stream truncation.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        public override int Capacity
        {
            get
            {
                CheckDisposed();
                if (largeBuffer != null)
                {
                    return largeBuffer.Length;
                }

                if (blocks.Count > 0)
                {
                    return blocks.Count * memoryManager.BlockSize;
                }
                return 0;
            }
            set { EnsureCapacity(value); }
        }

        private int length;

        /// <summary>
        /// Gets the number of bytes written to this stream.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        public override long Length
        {
            get
            {
                CheckDisposed();
                return length;
            }
        }

        private int position;

        /// <summary>
        /// Gets the current position in the stream
        /// </summary>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        public override long Position
        {
            get 
            { 
                CheckDisposed();
                return position; 
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), @"value must be non-negative");
                }

                if (value > MaxStreamLength)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), @"value cannot be more than " + MaxStreamLength);
                }

                position= (int)value;
            }
        }

        /// <summary>
        /// Whether the stream can currently read
        /// </summary>
        public override bool CanRead
        {
            get { return !disposed; }
        }

        /// <summary>
        /// Whether the stream can currently seek
        /// </summary>
        public override bool CanSeek
        {
            get { return !disposed; }
        }

        /// <summary>
        /// Always false
        /// </summary>
        public override bool CanTimeout
        {
            get { return false; }
        }

        /// <summary>
        /// Whether the stream can currently write
        /// </summary>
        public override bool CanWrite
        {
            get { return !disposed; }
        }

        /// <summary>
        /// Returns a single buffer containing the contents of the stream.
        /// The buffer may be longer than the stream length.
        /// </summary>
        /// <returns>A byte[] buffer</returns>
        /// <remarks>IMPORTANT: Doing a Write() after calling GetBuffer() invalidates the buffer. The old buffer is held onto
        /// until Dispose is called, but the next time GetBuffer() is called, a new buffer from the pool will be required.</remarks>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        public override byte[] GetBuffer()
        {
            CheckDisposed();

            if (largeBuffer != null)
            {
                return largeBuffer;
            }

            if (blocks.Count == 1)
            {
                return blocks[0];
            }

            // Buffer needs to reflect the capacity, not the length, because
            // it's possible that people will manipulate the buffer directly
            // and set the length afterward. Capacity sets the expectation
            // for the size of the buffer.
            var newBuffer = MemoryManager.GetLargeBuffer(Capacity, tag);
            
            // InternalRead will check for existence of largeBuffer, so make sure we
            // don't set it until after we've copied the data.
            InternalRead(newBuffer, 0, length, 0);
            largeBuffer = newBuffer;

            if (blocks.Count > 0 && memoryManager.AggressiveBufferReturn)
            {
                memoryManager.ReturnBlocks(blocks, tag);
                blocks.Clear();
            }

            return largeBuffer;
        }

        /// <summary>
        /// Returns a new array with a copy of the buffer's contents. You should almost certainly be using GetBuffer combined with the Length to 
        /// access the bytes in this stream. Calling ToArray will destroy the benefits of pooled buffers, but it is included
        /// for the sake of completeness.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        [Obsolete("This method has degraded performance vs. GetBuffer and should be avoided.")]
        public override byte[] ToArray()
        {
            CheckDisposed();
            var newBuffer = new byte[Length];

            InternalRead(newBuffer, 0, length, 0);
            string stack = memoryManager.GenerateCallStacks ? Environment.StackTrace : null;
            memoryManager.ReportStreamToArray();

            return newBuffer;
        }

        /// <summary>
        /// Reads from the current position into the provided buffer
        /// </summary>
        /// <param name="buffer">Destination buffer</param>
        /// <param name="offset">Offset into buffer at which to start placing the read bytes.</param>
        /// <param name="count">Number of bytes to read.</param>
        /// <returns>The number of bytes read</returns>
        /// <exception cref="ArgumentNullException">buffer is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset or count is less than 0</exception>
        /// <exception cref="ArgumentException">offset subtracted from the buffer length is less than count</exception>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            CheckDisposed();
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "offset cannot be negative");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "count cannot be negative");
            }

            if (offset + count > buffer.Length)
            {
                throw new ArgumentException("buffer length must be at least offset + count");
            }

            int amountRead = InternalRead(buffer, offset, count, position);
            position += amountRead;
            return amountRead;
        }

        /// <summary>
        /// Writes the buffer to the stream
        /// </summary>
        /// <param name="buffer">Source buffer</param>
        /// <param name="offset">Start position</param>
        /// <param name="count">Number of bytes to write</param>
        /// <exception cref="ArgumentNullException">buffer is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset or count is negative</exception>
        /// <exception cref="ArgumentException">buffer.Length - offset is not less than count</exception>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            CheckDisposed();
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }
            
            if (offset < 0)
            {
                throw  new ArgumentOutOfRangeException(nameof(offset), offset, "Offset must be in the range of 0 - buffer.Length-1");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "count must be non-negative");
            }

            if (count + offset > buffer.Length)
            {
                throw new ArgumentException("count must be greater than buffer.Length - offset");
            }

            // Check for overflow
            if (Position + count > MaxStreamLength)
            {
                throw new IOException("Maximum capacity exceeded");
            }
            
            int end = (int)Position + count;

            int blockSize = memoryManager.BlockSize;

            int requiredBuffers = (end + blockSize - 1) / blockSize;
            
            if (requiredBuffers * blockSize > MaxStreamLength)
            {
                throw new IOException("Maximum capacity exceeded");
            }

            EnsureCapacity(end);

            if (largeBuffer == null)
            {
                int bytesRemaining = count;
                int bytesWritten = 0;
                int currentBlockIndex = OffsetToBlockIndex(position);

                int blockOffset = OffsetToBlockOffset(position);

                while (bytesRemaining > 0)
                {
                    byte[] currentBlock = blocks[currentBlockIndex];
                    int remainingInBlock = blockSize - blockOffset;
                    int amountToWriteInBlock = Math.Min(remainingInBlock, bytesRemaining);

                    Buffer.BlockCopy(buffer, offset + bytesWritten, currentBlock, blockOffset, amountToWriteInBlock);

                    bytesRemaining -= amountToWriteInBlock;
                    bytesWritten += amountToWriteInBlock;

                    currentBlockIndex++;
                    blockOffset = 0;
                }
            }
            else
            {
                Buffer.BlockCopy(buffer, offset, largeBuffer, position, count);
            }
            Position = end;
            length = Math.Max(position, length);
        }

        /// <summary>
        /// Returns a useful string for debugging. This should not normally be called in actual production code.
        /// </summary>
        public override string ToString()
        {
            return $"Id = {Id}, Tag = {Tag}, Length = {Length:N0} bytes";
        }

        /// <summary>
        /// Writes a single byte to the current position in the stream.
        /// </summary>
        /// <param name="value">byte value to write</param>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        public override void WriteByte(byte value)
        {
            CheckDisposed();
            byteBuffer[0] = value;
            Write(byteBuffer, 0, 1);
        }

        /// <summary>
        /// Reads a single byte from the current position in the stream.
        /// </summary>
        /// <returns>The byte at the current position, or -1 if the position is at the end of the stream.</returns>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        public override int ReadByte()
        {
            CheckDisposed();
            if (position == length)
            {
                return -1;
            }
            byte value = 0;
            if (largeBuffer == null)
            {
                int block = OffsetToBlockIndex(position);
                int blockOffset = OffsetToBlockOffset(position);
                value = blocks[block][blockOffset];
            }
            else
            {
                value = largeBuffer[position];
            }
            position++;
            return value;
        }

        /// <summary>
        /// Sets the length of the stream
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">value is negative or larger than MaxStreamLength</exception>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        public override void SetLength(long value)
        {
            CheckDisposed();
            if (value < 0 || value > MaxStreamLength)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative and at most " + MaxStreamLength);
            }
            
            EnsureCapacity((int)value);

            length = (int)value;
            if (position > value)
            {
                position = (int) value;
            }
        }

        /// <summary>
        /// Sets the position to the offset from the seek location
        /// </summary>
        /// <param name="offset">How many bytes to move</param>
        /// <param name="loc">From where</param>
        /// <returns>The new position</returns>
        /// <exception cref="ObjectDisposedException">Object has been disposed</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset is larger than MaxStreamLength</exception>
        /// <exception cref="ArgumentException">Invalid seek origin</exception>
        /// <exception cref="IOException">Attempt to set negative position</exception>
        public override long Seek(long offset, SeekOrigin loc)
        {
            CheckDisposed();
            if (offset > MaxStreamLength)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "offset cannot be larger than " + MaxStreamLength);
            }

            int newPosition;
            switch (loc)
            {
                case SeekOrigin.Begin:
                    newPosition = (int)offset;
                break;
                case SeekOrigin.Current:
                    newPosition = (int) offset + position;
                break;
                case SeekOrigin.End:
                    newPosition = (int) offset + length;
                break;
                default:
                    throw new ArgumentException("Invalid seek origin", nameof(loc));
            }
            if (newPosition < 0)
            {
                throw new IOException("Seek before beginning");
            }
            position = newPosition;
            return position;
        }

        /// <summary>
        /// Synchronously writes this stream's bytes to the parameter stream.
        /// </summary>
        /// <param name="stream">Destination stream</param>
        /// <remarks>Important: This does a synchronous write, which may not be desired in some situations</remarks>
        public override void WriteTo(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            
            if (largeBuffer == null)
            {
                int currentBlock = 0;
                int bytesRemaining = length;

                while (bytesRemaining > 0)
                {
                    int amountToCopy = Math.Min(blocks[currentBlock].Length, bytesRemaining);
                    stream.Write(blocks[currentBlock], 0, amountToCopy);
                    
                    bytesRemaining -= amountToCopy;

                    ++currentBlock;
                }
            }
            else
            {
                stream.Write(largeBuffer, 0, length);
            }
        }
        #endregion
        
        #region Helper Methods
        
        private void CheckDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException($"The stream with Id {id} and Tag {tag} is disposed.");
            }
        }

        private int InternalRead(byte[] buffer, int offset, int count, int fromPosition)
        {
            if (length - fromPosition <= 0)
            {
                return 0;
            }
            if (largeBuffer == null)
            {
                int currentBlock = OffsetToBlockIndex(fromPosition);
                int bytesWritten = 0;
                int bytesRemaining = Math.Min(count, length - fromPosition);
                int blockOffset = OffsetToBlockOffset(fromPosition);

                while (bytesRemaining > 0)
                {
                    int amountToCopy = Math.Min(blocks[currentBlock].Length - blockOffset, bytesRemaining);
                    Buffer.BlockCopy(blocks[currentBlock], blockOffset, buffer, bytesWritten + offset, amountToCopy);

                    bytesWritten += amountToCopy;
                    bytesRemaining -= amountToCopy;

                    ++currentBlock;
                    blockOffset = 0;
                }
                return bytesWritten;
            }
            else
            {
                int amountToCopy = Math.Min(count, length - fromPosition);
                Buffer.BlockCopy(largeBuffer, fromPosition, buffer, offset, amountToCopy);
                return amountToCopy;
            }
        }

        private int OffsetToBlockIndex(int offset)
        {
            return offset / memoryManager.BlockSize;
        }

        private int OffsetToBlockOffset(int offset)
        {
            return offset % memoryManager.BlockSize;
        }

        private void EnsureCapacity(int newCapacity)
        {
            if (newCapacity > memoryManager.MaximumStreamCapacity && memoryManager.MaximumStreamCapacity > 0)
            {
                throw new InvalidOperationException("Requested capacity is too large: " + newCapacity + ". Limit is " + memoryManager.MaximumStreamCapacity);
            }

            if (largeBuffer != null)
            {
                if (newCapacity > largeBuffer.Length)
                {
                    var newBuffer = memoryManager.GetLargeBuffer(newCapacity, tag);
                    InternalRead(newBuffer, 0, length, 0);
                    ReleaseLargeBuffer();
                    largeBuffer = newBuffer;
                }
            }
            else
            {
                while (Capacity < newCapacity)
                {
                    blocks.Add(MemoryManager.GetBlock());
                }
            }
        }

        /// <summary>
        /// Release the large buffer (either stores it for eventual release or returns it immediately).
        /// </summary>
        private void ReleaseLargeBuffer()
        {
            if (memoryManager.AggressiveBufferReturn)
            {
                memoryManager.ReturnLargeBuffer(largeBuffer, tag);
            }
            else
            {
                if (dirtyBuffers == null)
                {
                    // We most likely will only ever need space for one
                    dirtyBuffers = new List<byte[]>(1);
                }
                dirtyBuffers.Add(largeBuffer);
            }

            largeBuffer = null;
        }
        #endregion
    }
}
