using System;
using Aran.Temporality.Common.Abstract.MetaData;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.MetaData;

namespace Aran.Temporality.Common.Abstract.Event
{
    [Serializable]
    public abstract class AbstractEventMetaData : AbstractMetaData
    {
        #region Ctor

        protected AbstractEventMetaData()
        {
        }

        protected AbstractEventMetaData(AbstractEventMetaData other)
        {
            InitFromEventMeta(other);
        }

        #endregion

        public virtual string User { get; set; }
        public virtual Interpretation Interpretation { get; set; }
        public virtual bool IsCanceled { get; set; }
        public virtual bool ApplyTimeSliceToFeatureLifeTime { get; set; }
        public virtual bool IsCreatedInWorkPackage { get; set; }


        private DateTime? _lifeTimeBegin;
        public DateTime? LifeTimeBegin
        {
            get => _lifeTimeBegin;
            set
            {
                _lifeTimeBegin = value;
                if (_lifeTimeBegin!=null)
                {
                    LifeTimeBeginSet = true;
                }
            }
        }

        private DateTime? _lifeTimeEnd;
        public DateTime? LifeTimeEnd
        {
            get => _lifeTimeEnd;
            set
            {
                _lifeTimeEnd = value;
                if (_lifeTimeEnd != null)
                {
                    LifeTimeEndSet = true;
                }
            }
        }

        public bool LifeTimeBeginSet { get; set; }
        public bool LifeTimeEndSet { get; set; }


        #region Has Impact on specified date

        public bool HasImpact(DateTime actualDate)
        {
            return TimeSlice.BeginPosition <= actualDate && (TimeSlice.EndPosition == null || TimeSlice.EndPosition >= actualDate);
        }

        public bool HasImpact(DateTime fromDate, DateTime toDate)
        {
            return HasImpact(fromDate) || HasImpact(toDate) ||
                   (fromDate <= TimeSlice.BeginPosition && TimeSlice.EndPosition != null && TimeSlice.EndPosition <= toDate);
        }

        #endregion

        public AbstractEventMetaData InitFromEventMeta(AbstractEventMetaData other)
        {
            IsCreatedInWorkPackage = other.IsCreatedInWorkPackage;
            User = other.User;
            ApplyTimeSliceToFeatureLifeTime = other.ApplyTimeSliceToFeatureLifeTime;
            FeatureTypeId = other.FeatureTypeId;
            Guid = other.Guid;
            Interpretation = other.Interpretation;
            IsCanceled = other.IsCanceled;
            SubmitDate = other.SubmitDate;
            WorkPackage = other.WorkPackage;

            LifeTimeBegin = other.LifeTimeBegin;
            LifeTimeEnd = other.LifeTimeEnd;
            LifeTimeBeginSet = other.LifeTimeBeginSet;
            LifeTimeEndSet = other.LifeTimeEndSet;


            if (other.TimeSlice!=null)
            {
                TimeSlice = new TimeSlice(other.TimeSlice);
            }
            if (other.Version!=null)
            {
                Version = new TimeSliceVersion(other.Version);
            }

          
            return this;
        }

        public override string ToString()
        {
            return Interpretation + " " + Version + (IsCanceled ? " cancelled" : string.Empty);
        }
    }
}