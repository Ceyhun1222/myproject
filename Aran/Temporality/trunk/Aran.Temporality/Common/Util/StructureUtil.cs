using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Aran.Temporality.Common.Util
{
    public class StructureUtil
    {
        public static byte[] StructureToBytes(Object t)
        {
            int size = Marshal.SizeOf(t.GetType());
            var result = new byte[size];
            IntPtr pointer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(t, pointer, true);
            Marshal.Copy(pointer, result, 0, size);
            Marshal.FreeHGlobal(pointer);
            return result;
        }

        public static TStructureType StructureFromBytes<TStructureType>(byte[] bytes)
        {
            IntPtr pointer = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, pointer, bytes.Length);
            var result = (TStructureType) Marshal.PtrToStructure(pointer, typeof (TStructureType));
            Marshal.FreeHGlobal(pointer);
            return result;
        }

        public static void WriteStructure(Stream stream, ValueType item)
        {
            byte[] bytes = StructureToBytes(item);
            stream.Write(bytes, 0, bytes.Length);
        }

        public static T LoadStructureFromStream<T>(Stream stream)
        {
            int size = Marshal.SizeOf(typeof (T));
            var bytes = new byte[size];
            stream.Read(bytes, 0, size);
            return StructureFromBytes<T>(bytes);
        }
    }
}