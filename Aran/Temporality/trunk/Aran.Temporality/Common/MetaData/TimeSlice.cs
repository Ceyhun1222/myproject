using System;
using Aran.Temporality.Common.Config;

namespace Aran.Temporality.Common.MetaData
{
    [Serializable]
    public sealed class TimeSlice
    {
        public TimeSlice()
        {
        }

        public TimeSlice(TimeSlice other)
        {
            if (other == null) return;
            BeginPosition = other.BeginPosition;
            EndPosition = other.EndPosition;
        }

        public TimeSlice(DateTime begin, DateTime? end = null)
        {
            BeginPosition = begin;
            EndPosition = end;
        }

        public DateTime BeginPosition { get; set; }
        public DateTime? EndPosition { get; set; }

        public override string ToString()
        {
            return String.Format(ConfigUtil.TimeSlicedDateFormat, BeginPosition) + " - "
                   + (EndPosition == null ? "~" : String.Format(ConfigUtil.TimeSlicedDateFormat, EndPosition));
        }
    }
}