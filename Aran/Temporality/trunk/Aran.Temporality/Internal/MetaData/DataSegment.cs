#region

using System;
using System.IO;
using Aran.Temporality.Internal.Struct;

#endregion

namespace Aran.Temporality.Internal.MetaData
{
    internal class DataSegment
    {
        public int Index { get; set; }

        public Int32 Offset { get; set; }
        public Int32 Length { get; set; }
        public byte[] Data { get; set; }


        public override string ToString()
        {
            int log = BlockOffsetStructure.Log2(Length);
            if (Length == 1 << log)
            {
                return Length + "[2^" + log + "] from " + Offset;
            }
            return Length + "[2^0] from " + Offset;
        }

        public void LoadData(byte[] bytes, Stream stream)
        {
            Data = new byte[Length];
            stream.Position = Offset;
            stream.Read(Data, 0, Length);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (DataSegment)) return false;
            return Equals((DataSegment) obj);
        }

        public bool Equals(DataSegment other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Offset == Offset;
        }

        public override int GetHashCode()
        {
            return Offset.GetHashCode();
        }

        public static bool operator ==(DataSegment left, DataSegment right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DataSegment left, DataSegment right)
        {
            return !Equals(left, right);
        }
    }
}