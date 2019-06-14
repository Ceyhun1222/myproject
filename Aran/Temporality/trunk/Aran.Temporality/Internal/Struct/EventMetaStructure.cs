#region

using System;
using System.Runtime.InteropServices;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.MetaData;
using Aran.Temporality.Internal.MetaData.Offset;

#endregion

namespace Aran.Temporality.Internal.Struct
{
    internal struct EventMetaStructure
    {
        [MarshalAs(UnmanagedType.LPStr, SizeConst = 100)]
        public string User;

        public bool ApplyTimeSliceToFeatureLifeTime;
        public int CorrectionNumber;
        public DateTime? EndDate;

        public int FeatureTypeId;
        public Guid? Guid;

        public Interpretation Interpretation;
        public bool IsCanceled;
        public long Offset;
        public int SequenceNumber;
        public DateTime StartDate;
        public DateTime SubmitDate;
        public int WorkPackage;
        public bool IsCreatedInWorkPackage;


        public EventMetaStructure(OffsetEventMetaData<long> meta)
        {
            IsCreatedInWorkPackage = meta.IsCreatedInWorkPackage;
            User = meta.User;
            Offset = meta.Offset;
            StartDate = meta.TimeSlice.BeginPosition;
            EndDate = meta.TimeSlice.EndPosition;
            SequenceNumber = meta.Version.SequenceNumber;
            CorrectionNumber = meta.Version.CorrectionNumber;
            Guid = meta.Guid;
            FeatureTypeId = meta.FeatureTypeId;
            WorkPackage = meta.WorkPackage;
            SubmitDate = meta.SubmitDate;
            Interpretation = meta.Interpretation;
            IsCanceled = meta.IsCanceled;
            ApplyTimeSliceToFeatureLifeTime = meta.ApplyTimeSliceToFeatureLifeTime;
        }

        public OffsetEventMetaData<long> GetEventMetaData()
        {
            EventMetaStructure str = this;
            return new OffsetEventMetaData<long>
                       {
                           IsCreatedInWorkPackage = str.IsCreatedInWorkPackage,
                           User = str.User,
                           Offset = str.Offset,
                           TimeSlice = new TimeSlice(str.StartDate, str.EndDate),
                           Version = new TimeSliceVersion(str.SequenceNumber, str.CorrectionNumber),
                           Guid = str.Guid,
                           FeatureTypeId = str.FeatureTypeId,
                           WorkPackage = str.WorkPackage,
                           SubmitDate = str.SubmitDate,
                           Interpretation = str.Interpretation,
                           IsCanceled = str.IsCanceled,
                           ApplyTimeSliceToFeatureLifeTime = str.ApplyTimeSliceToFeatureLifeTime,
                       };
        }
    }
}