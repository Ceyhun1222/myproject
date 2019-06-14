#region

using System;
using Aran.Temporality.Common.Abstract.MetaData;
using Aran.Temporality.Internal.Interface.Util;

#endregion

namespace Aran.Temporality.Internal.MetaData.Offset
{
    [Serializable]
    internal class OffsetStateMetaData<TOffsetType> : StateMetaData, IHasOffset<TOffsetType>
    {
        public OffsetStateMetaData()
        {
        }

      

        public OffsetStateMetaData(AbstractMetaData other) : base(other)
        {
        }

        public int Index { get; set; }
        public DateTime? ClearStatesAfter { get; set; }

        #region IHasOffset<TOffsetType> Members

        public TOffsetType Offset { get; set; }

        #endregion

        public override string ToString()
        {
            if (ClearStatesAfter != null) return "Clear after " + ClearStatesAfter;
            return base.ToString();
        }
    }
}