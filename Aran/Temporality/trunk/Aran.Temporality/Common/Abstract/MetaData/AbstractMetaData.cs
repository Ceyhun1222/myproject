using System;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.MetaData;

namespace Aran.Temporality.Common.Abstract.MetaData
{
    [Serializable]
    public abstract class AbstractMetaData : IFeatureId
    {
        protected AbstractMetaData()
        {
        }

        protected AbstractMetaData(IFeatureId featureId)
        {
            InitFromFeatureId(featureId);
        }

        public TimeSlice TimeSlice { get; set; }
        public TimeSliceVersion Version { get; set; }

        public AbstractMetaData InitFromFeatureId(IFeatureId featureId)
        {
            Guid = featureId.Guid;
            FeatureTypeId = featureId.FeatureTypeId;
            WorkPackage = featureId.WorkPackage;
            return this;
        }


        private DateTime _submitDate = DateTime.Now;
        public virtual DateTime SubmitDate
        {
            get => _submitDate;
            set => _submitDate = value;
        }

        #region Implementation of IFeatureId

        public abstract Guid? Guid { get; set; }
        public abstract int FeatureTypeId { get; set; }
        public int WorkPackage { get; set; }

        #endregion
    }
}