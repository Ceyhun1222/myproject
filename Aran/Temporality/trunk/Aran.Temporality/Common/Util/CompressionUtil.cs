using System.IO;
using System.IO.Compression;

namespace Aran.Temporality.Common.Util
{
    public class CompressionUtil
    {
        public static byte[] ReadFully(Stream input)
        {
            using (var ms = MemoryUtil.GetMemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        public static void CompressToStream(byte[] bytesToCompress, int length, ref MemoryStream retStream)
        {
            GZipStream gzStream = new GZipStream(retStream, CompressionMode.Compress, true);
            gzStream.Write(bytesToCompress, 0, length);
            gzStream.Close();
        }

        public static void CompressToStream(byte[] bytesToCompress, ref MemoryStream retStream)
        {
            CompressToStream(bytesToCompress, bytesToCompress.Length, ref retStream);
        }

        public static void UnCompress(byte[] buffToUnCompress, ref byte[] unCompressedBuffer)
        {
            var cmpStream = MemoryUtil.GetMemoryStream(buffToUnCompress);

            GZipStream unCompZip = new GZipStream(cmpStream, CompressionMode.Decompress, true);

            var unCmpStream = MemoryUtil.GetMemoryStream();
            unCompressedBuffer = new byte[buffToUnCompress.Length];

            int read = 0;
            while (0 != (read = unCompZip.Read(unCompressedBuffer, 0, buffToUnCompress.Length)))
            {
                unCmpStream.Write(unCompressedBuffer, 0, read);
            }

            unCompressedBuffer = unCmpStream.ToArray();

            unCmpStream.Close();
            unCompZip.Close();
            cmpStream.Close();
        }
    }
}
