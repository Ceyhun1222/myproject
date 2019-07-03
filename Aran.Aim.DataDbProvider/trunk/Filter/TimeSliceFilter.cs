using System;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Data.Filters
{
    public enum QueryType
    {
        ByEffectiveDate,
        ByTimePeriod,
        BySequenceNumber
    }

    [Serializable]
    public class TimeSliceFilter
    {
        public TimeSliceFilter(DateTime effectiveDate)
        {
            EffectiveDate = effectiveDate;
        }

        public TimeSliceFilter(TimePeriod period)
        {
            ValidTime = period;
        }

        public TimeSliceFilter(int sequence, int correction)
        {
            SequenceNumber = sequence;
            CorrectionNumber = correction;
        }

        public TimeSliceFilter(int sequence)
        {
            SequenceNumber = sequence;
        }

        public QueryType QueryType
        {
            get;
            set;
        }

        public DateTime EffectiveDate
        {
            get
            {
                return _effectiveDate;
            }

            set
            {
                _effectiveDate = value;
                QueryType = Filters.QueryType.ByEffectiveDate;
            }
        }

        public TimePeriod ValidTime
        {
            get
            {
                return _validTime;
            }
            set
            {
                _validTime = value;
                QueryType = QueryType.ByTimePeriod;
            }
        }

        public int SequenceNumber
        {
            get
            {
                return _sequenceNumber;
            }

            set
            {
                _sequenceNumber = value;
                QueryType = Filters.QueryType.BySequenceNumber;
            }
        }

        public int? CorrectionNumber { get; set; }

        private TimePeriod _validTime;
        private DateTime _effectiveDate;
        private int _sequenceNumber;
    }
}
