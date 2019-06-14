using System;
using System.IO;
using System.Threading;

namespace Aran.Temporality.Common.Util
{
    public class ByteArrayStreamFactory : IDisposable
    {
        private int _readerCount;
        private bool _disposed;
        private byte[] _buffer;

        public void SetData(byte[] data)
        {
            _buffer = data;
        }

        public ByteArrayReaderStream GetReader()
        {
            Interlocked.Increment(ref _readerCount);
            return new ByteArrayReaderStream(_buffer) { OnDispose = OnReaderDisposed };
        }

        private void OnReaderDisposed()
        {
            Interlocked.Decrement(ref _readerCount);
        }

       
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
            var i = 0;
            while (i++<100)
            {
                Thread.Sleep(1000);
                if (_readerCount <= 0)
                {
                    _buffer = null;
                    break;
                }
            }
        }
    }

    public class ByteArrayReaderStream : Stream, IDisposable
    {
        private int _position;
        private readonly byte[] _buffer;


        public ByteArrayReaderStream(byte[]bufer)
        {
            _buffer = bufer;
            _position = 0;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new NotImplementedException();    
        }

        public override long Length
        {
            get { return _buffer.Length; }
        }

        public override long Position
        {
            get
            {
                return _position;
            }
            set { _position = (int)value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Buffer.BlockCopy(_buffer, _position, buffer, offset, count);
            _position += count;
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public Action OnDispose;

        public new void Dispose()
        {
            OnDispose?.Invoke();
        }
    }
}
