using System;
using Aran.Temporality.Common.Abstract.MetaData;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.MetaData;

namespace Aran.Temporality.Common.Abstract.State
{
    [Serializable]
    public abstract class AbstractStateMetaData : AbstractMetaData
    {
        #region Ctor

        protected AbstractStateMetaData()
        {
        }

        protected AbstractStateMetaData(AbstractMetaData other)
        {
            InitFromMeta(other);
        }

        #endregion

        protected void InitFromMeta(AbstractMetaData other)
        {
            FeatureTypeId = other.FeatureTypeId;
            Guid = other.Guid;
            TimeSlice = new TimeSlice(other.TimeSlice);
            Version = new TimeSliceVersion(other.Version);
            WorkPackage = other.WorkPackage;
        }

        public override string ToString()
        {
            return "SnapShot " + Version;
        }
    }
}