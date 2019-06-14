#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Aran.Temporality.Internal.MetaData;

#endregion

namespace Aran.Temporality.Internal.Struct
{
    internal struct BlockOffsetStructure
    {
        public const int MinBlockBits = 8; //minimal block is 256 (2^8) bytes

        public Int32 BlockLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public Int32[] Offsets;

        public IList<DataSegment> GetSegments(bool initData = true)
        {
            var result = new List<DataSegment>();

            int currentSegmentMask = BlockLength;
            //process minBlock
            const int minSegmentMask = (1 << MinBlockBits) - 1;
            if ((BlockLength & minSegmentMask) != 0)
            {
                result.Add(new DataSegment
                               {
                                   Offset = Offsets[0],
                                   Length = BlockLength & minSegmentMask,
                                   Data = initData ? new byte[BlockLength & minSegmentMask] : null
                               });
            }

            //init
            currentSegmentMask = currentSegmentMask >> MinBlockBits;
            int currentShift = MinBlockBits;
            while (currentSegmentMask > 0)
            {
                if ((currentSegmentMask & 1) != 0)
                {
                    result.Add(new DataSegment
                                   {
                                       Offset = Offsets[currentShift],
                                       Length = 1 << currentShift,
                                       Data = initData ? new byte[1 << currentShift] : null
                                   });
                }

                currentShift++;
                currentSegmentMask = currentSegmentMask >> 1;
            }

            return result;
        }

        public static BlockOffsetStructure New()
        {
            var result = new BlockOffsetStructure
                             {
                                 Offsets = new int[32]
                             };
            return result;
        }

        public static BlockOffsetStructure LoadFromBytes(byte[] bytes)
        {
            var result = new BlockOffsetStructure();
            int size = Marshal.SizeOf(result);
            IntPtr pointer = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, pointer, size);
            result = (BlockOffsetStructure) Marshal.PtrToStructure(pointer, result.GetType());
            Marshal.FreeHGlobal(pointer);
            return result;
        }

        public byte[] GetBytes()
        {
            int size = Marshal.SizeOf(this);
            var result = new byte[size];
            IntPtr pointer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(this, pointer, true);
            Marshal.Copy(pointer, result, 0, size);
            Marshal.FreeHGlobal(pointer);
            return result;
        }

        public static IList<DataSegment> FromData(byte[] dataBytes)
        {
            var offset = new BlockOffsetStructure
                             {
                                 BlockLength = dataBytes.Length,
                                 Offsets = new Int32[32]
                             };
            IList<DataSegment> result = offset.GetSegments();

            int pos = dataBytes.Length;
            foreach (var segment in result)
            {
                pos -= segment.Length;
                Buffer.BlockCopy(dataBytes, pos, segment.Data, 0, segment.Length);
            }

            return result;
        }

        public static int Log2(Int32 val)
        {
            int total = 0;
            while (val > 1)
            {
                total++;
                val = val >> 1;
            }
            return total;
        }

        public static BlockOffsetStructure FromSegments(IList<DataSegment> segments)
        {
            int length = segments.Sum(segment => segment.Length);

            var result = new BlockOffsetStructure
                             {
                                 BlockLength = length,
                                 Offsets = new Int32[32]
                             };

            foreach (var segment in segments)
            {
                int index = Log2(segment.Length);
                if (index < MinBlockBits)
                {
                    index = 0;
                }

                result.Offsets[index] = segment.Offset;
            }

            return result;
        }
    }
}