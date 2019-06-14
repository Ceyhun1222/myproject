using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;

namespace Aran.Temporality.Common.Util
{
    public interface ISerializableToArray
    {
        byte[] ToArray();
    }

    public abstract class SerializableStream : Stream, ISerializableToArray
    {
        public abstract byte[] ToArray();

        public bool IsComunications { get; set; }
        public object SerializedObject { get; set; }
    }

    public class SerializableMemoryStream : SerializableStream
    {
        private readonly MemoryStream _memoryStream;

      
        public SerializableMemoryStream()
        {
            _memoryStream = MemoryUtil.GetMemoryStream();
        }

        protected override void Dispose(bool disposing)
        {
            _memoryStream.Dispose();
        }

        public override void Flush()
        {
            _memoryStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _memoryStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _memoryStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _memoryStream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _memoryStream.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return _memoryStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _memoryStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _memoryStream.CanWrite; }
        }

        public override long Length
        {
            get { return _memoryStream.Length; }
        }

        public override long Position
        {
            get { return _memoryStream.Position; }
            set { _memoryStream.Position = value; }
        }

        public override byte[] ToArray()
        {
            return _memoryStream.ToArray();
        }
    }

    public class SingleThreadSerializableStream : SerializableStream
    {
        //private MemoryStream _memoryStream;
        private Stream _zipStream;

        public static Stream OpenForRead(byte[] bytes, int offset, int length)
        {
          return  UncompressStream(MemoryUtil.GetMemoryStream(bytes, offset, length));
        }

        public static Stream OpenForRead(byte[] bytes)
        {
            return OpenForRead(bytes,0,bytes.Length);
        }


        public static SerializableStream OpenForWrite(object serializedObject, bool isComunications, bool compressMaximum=false)
        {
            var stream = new SingleThreadSerializableStream
            {
                CompressMaximum=compressMaximum,
                SerializedObject = serializedObject,
                IsComunications = isComunications,
                _zipStream = MemoryUtil.GetMemoryStream()
            };
            return stream;
        }

        public bool CompressMaximum { get; set; }


        private SingleThreadSerializableStream()
        {
        }

        //protected override void Dispose(bool disposing)
        //{
        //    _zipStreamReader.Dispose();
        //    _memoryStream.Dispose();
        //}

        public override void Flush()
        {
            _zipStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _zipStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _zipStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _zipStream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _zipStream.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return _zipStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _zipStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _zipStream.CanWrite; }
        }

        public override long Length
        {
            get { return _zipStream.Length; }
        }

        public override long Position
        {
            get { return _zipStream.Position; }
            set { _zipStream.Position = value; }
        }


        // Compresses the supplied memory stream, naming it as zipEntryName, into a zip,
        // which is returned as a memory stream or a byte array.
        //
        private byte[] CompressStream(MemoryStream memStreamIn)
        {
            
            if ((CompressMaximum && memStreamIn.Length < 256) ||//if we store it but it is too small to get advantage of compression OR
                (!CompressMaximum && (!IsComunications || memStreamIn.Length < 4096)))//we do not store it AND it is not communication OR not very big
            {
                var buffer = memStreamIn.GetBuffer();//do zip if object itself starts from zip magic because it will be processed as zip on reading anyway
                if (memStreamIn.Length < 2 || buffer[0] != MagicUtil.GZip[0] || buffer[1] != MagicUtil.GZip[1])
                {   
                    return memStreamIn.ToArray();
                }
            }

            MemoryStream outputMemStream = new MemoryStream();
            var zipStream = new GZipOutputStream(outputMemStream);

            zipStream.SetLevel(CompressMaximum ? 9 : 3);

            //if (memStreamIn.Length < 1000)
            //{
            //    zipStream.SetLevel(1);
            //}
            //else if (IsComunications)
            //{
            //    zipStream.SetLevel(1); //0-9, 9 being the highest level of compression
            //}
            //else
            //{
            //    zipStream.SetLevel(9); //0-9, 9 being the highest level of compression
            //}

            StreamUtils.Copy(memStreamIn, zipStream, new byte[4096]);

            zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.
            zipStream.Close();          // Must finish the ZipOutputStream before using outputMemStream.

            outputMemStream.Position = 0;
            //return outputMemStream;

            // Alternative outputs:
            // ToArray is the cleaner and easiest to use correctly with the penalty of duplicating allocated memory.
            byte[] byteArrayOut = outputMemStream.ToArray();

            // GetBuffer returns a raw buffer raw and so you need to account for the true length yourself.
            //byte[] byteArrayOut = outputMemStream.GetBuffer();
            //long len = outputMemStream.Length;

            return byteArrayOut;
        }

        private static Stream UncompressStream(Stream zipStream)
        {
            zipStream.Position = 0;
            if (zipStream.ReadByte() != MagicUtil.GZip[0] || zipStream.ReadByte() != MagicUtil.GZip[1])
            {
                zipStream.Position = 0;
                return zipStream;
            }
            zipStream.Position = 0;
            var zipInputStream = new GZipInputStream(zipStream);
            return zipInputStream;
        }

        public override byte[] ToArray()
        {
            _zipStream.Position = 0;
            var data= CompressStream((MemoryStream)_zipStream);
            return data;
        }
    }
}