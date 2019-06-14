#region

using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.Interface.Util;

#endregion

namespace Aran.Temporality.Internal.MetaData.Offset
{
    [Serializable]
    internal class OffsetEventMetaData<TOffsetType> : EventMetaData, IHasOffset<TOffsetType>
    {
        public IDictionary<int, List<IFeatureId>> RelatedFeatures = new SortedList<int, List<IFeatureId>>();
      
        public OffsetEventMetaData()
        {
        }

        public OffsetEventMetaData(AbstractEventMetaData other) : base(other)
        {
        }

        #region IHasOffset<TOffsetType> Members

        public TOffsetType Offset { get; set; }

        public bool IsDecommission
        {
            get { return LifeTimeBeginSet && LifeTimeEndSet && LifeTimeBegin == LifeTimeEnd && !IsCanceled; }
        }
        public bool IsCancelCommission
        {
            get { return IsCanceled && Version.SequenceNumber == 1; }
        }
        #endregion
    }
}