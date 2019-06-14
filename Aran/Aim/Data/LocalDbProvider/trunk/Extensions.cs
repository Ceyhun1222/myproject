using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;
using System.IO;

namespace Aran.Aim.Data.Local
{
    internal static class ExtensionBinaryPackageWriter
    {
        public static int CurrentPosition (this BinaryPackageWriter bpw)
        {
            return (int) bpw.Writer.BaseStream.Position;
        }

        public static void Seek (this BinaryPackageWriter bpw, long offset, SeekOrigin origin)
        {
            bpw.Writer.BaseStream.Seek (offset, origin);
        }

        public static byte [] ToArray (this BinaryPackageWriter bpw)
        {
            return (bpw.Writer.BaseStream as MemoryStream).ToArray ();
        }


        public static int CurrentPosition (this BinaryPackageReader bpr)
        {
            return (int) bpr.Reader.BaseStream.Position;
        }

        public static void Seek (this BinaryPackageReader bpr, long offset, SeekOrigin origin)
        {
            bpr.Reader.BaseStream.Seek (offset, origin);
        }
    }
}
