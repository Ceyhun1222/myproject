using System;

namespace Aran.Temporality.Common.Id
{
    [Serializable]
    public sealed class TimeSliceVersion
    {
        public static TimeSliceVersion First = new TimeSliceVersion(1, 0);

        public TimeSliceVersion()
        {
        }

        public TimeSliceVersion(TimeSliceVersion other)
        {
            if (other == null) return;

            SequenceNumber = other.SequenceNumber;
            CorrectionNumber = other.CorrectionNumber;
        }

        public TimeSliceVersion(int sequenceNumber, int correctionNumber)
        {
            SequenceNumber = sequenceNumber;
            CorrectionNumber = correctionNumber;
        }

        public int SequenceNumber { get; set; }
        public int CorrectionNumber { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is TimeSliceVersion)
            {
                var other = obj as TimeSliceVersion;
                return other.CorrectionNumber == CorrectionNumber && other.SequenceNumber == SequenceNumber;
            }
            return false;
        }

        public override string ToString()
        {
            return SequenceNumber + "." + CorrectionNumber;
        }

        public bool Equals(TimeSliceVersion other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.SequenceNumber == SequenceNumber && other.CorrectionNumber == CorrectionNumber;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (SequenceNumber*397) ^ CorrectionNumber;
            }
        }

        public static bool operator ==(TimeSliceVersion left, TimeSliceVersion right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TimeSliceVersion left, TimeSliceVersion right)
        {
            return !Equals(left, right);
        }
    }
}