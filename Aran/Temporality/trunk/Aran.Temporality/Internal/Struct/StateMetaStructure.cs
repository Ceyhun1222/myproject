#region

using System;
using System.Runtime.InteropServices;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.MetaData;
using Aran.Temporality.Internal.MetaData.Offset;

#endregion

namespace Aran.Temporality.Internal.Struct
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct StateMetaStructure
    {
        public BlockOffsetStructure BlockOffset;

        public DateTime? ClearStatesAfter;

        public int WorkPackage;
        public Guid? Guid;

        public int FeatureType;


        public DateTime StartDate;
        public DateTime? EndDate;

        public int SequenceNumber;
        public int CorrectionNumber;

        public StateMetaStructure(OffsetStateMetaData<BlockOffsetStructure> meta)
        {
            ClearStatesAfter = meta.ClearStatesAfter;
            FeatureType = meta.FeatureTypeId;
            Guid = meta.Guid;
            StartDate = meta.TimeSlice.BeginPosition;
            EndDate = meta.TimeSlice.EndPosition;
            SequenceNumber = meta.Version.SequenceNumber;
            CorrectionNumber = meta.Version.CorrectionNumber;
            WorkPackage = meta.WorkPackage;
            BlockOffset = meta.Offset;
        }

        public OffsetStateMetaData<BlockOffsetStructure> OffsetStateMetaData()
        {
            StateMetaStructure str = this;
            return new OffsetStateMetaData<BlockOffsetStructure>
                       {
                           ClearStatesAfter = str.ClearStatesAfter,
                           FeatureTypeId = str.FeatureType,
                           Guid = str.Guid,
                           Offset = str.BlockOffset,
                           TimeSlice = new TimeSlice(str.StartDate, str.EndDate),
                           Version = new TimeSliceVersion(str.SequenceNumber, str.CorrectionNumber),
                           WorkPackage = str.WorkPackage
                       };
        }
    }
}