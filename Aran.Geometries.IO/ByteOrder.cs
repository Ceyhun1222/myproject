using System;
using System.Runtime.InteropServices;

namespace Aran.Geometries.IO
{
    [StructLayout ( LayoutKind.Explicit )]
    internal struct DoubleInt
    {
        [FieldOffset ( 0 )]
        public Int64 i;
        [FieldOffset ( 0 )]
        public UInt64 ui;
        [FieldOffset ( 0 )]
        public double f;
    }

    [StructLayout ( LayoutKind.Explicit )]
    internal struct FloatInt
    {
        [FieldOffset ( 0 )]
        public Int32 i;
        [FieldOffset ( 0 )]
        public UInt32 ui;
        [FieldOffset ( 0 )]
        public float f;
    }

	[StructLayout(LayoutKind.Explicit)]
	internal struct OrderedBytes
	{
		[FieldOffset(0)]
		public byte ob0;
		[FieldOffset(1)]
		public byte ob1;
	}

	[StructLayout(LayoutKind.Explicit)]
    internal struct TestByteOrder
    {
		[FieldOffset(0)]
        public Int16 i;
        [FieldOffset ( 0 )]
        public OrderedBytes orderedBytes;
    }

    public enum ByteOrder
    {
        BigEndian,
        LittleEndian
    }
}
