using System;
using Aran.Temporality.Common.Id;

namespace Aran.Temporality.Common.OperationResult
{
    [Serializable]
    public class FeatureOperationResult : CommonOperationResult
    {
        public TimeSliceVersion TimeSliceVersion { get; set; }

        public override string ToString()
        {
            return base.ToString() +
                   (TimeSliceVersion == null ? String.Empty : ", " + TimeSliceVersion);
        }

        internal void Add(FeatureOperationResult other)
        {
            //merge IsOk
            IsOk &= other.IsOk;

            //merge error message
            if (!String.IsNullOrWhiteSpace(other.ErrorMessage))
            {
                ReportError(other.ErrorMessage);
            }

            //keep old not null version
            if (TimeSliceVersion == null)
            {
                TimeSliceVersion = other.TimeSliceVersion;
            }
        }
    }
}