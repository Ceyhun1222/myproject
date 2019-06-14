using System;
using Aran.Temporality.Common.Interface;

namespace Aran.Temporality.Common.Id
{
    [Serializable]
    public class TimeSliceId : FeatureId
    {
        public TimeSliceId()
        {
        }

        public TimeSliceId(IFeatureId other) : base(other)
        {
        }

        public TimeSliceVersion Version { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is TimeSliceId)
            {
                var other = obj as TimeSliceId;
                if (ReferenceEquals(this, other)) return true;
                return Version.Equals(other.Version) &&
                       FeatureTypeId == other.FeatureTypeId &&
                       Guid == other.Guid;
            }

            return false;
        }


        public override int GetHashCode()
        {
            return Version?.GetHashCode() ?? 0;
        }
    }
}