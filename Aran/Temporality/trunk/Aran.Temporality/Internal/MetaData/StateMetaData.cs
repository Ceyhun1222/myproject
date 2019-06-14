#region

using System;
using Aran.Temporality.Common.Abstract.MetaData;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Interface;

#endregion

namespace Aran.Temporality.Internal.MetaData
{
    [Serializable]
    internal class StateMetaData : AbstractStateMetaData
    {
        public StateMetaData()
        {
        }

        public StateMetaData(IFeatureId other)
        {
            InitFromFeatureId(other);
        }


        public StateMetaData(AbstractMetaData other)
        {
            InitFromMeta(other);
        }

        #region Overrides of AbstractEventMetaData

        public override Guid? Guid { get; set; }
        public override int FeatureTypeId { get; set; }

        #endregion

        public override string ToString()
        {
            return base.ToString() + (TimeSlice == null ? "" : " at " + TimeSlice.BeginPosition);
        }
    }
}