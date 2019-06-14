using System;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    [Serializable]
    public class EventEntity : INHibernateEntity
    {
        public virtual int Id { get; set; }

        //feature ID
        public virtual Guid Guid { get; set; }
        public virtual int FeatureTypeId { get; set; }
        public virtual int WorkPackage { get; set; }

        //TimeSlice
        public virtual DateTime BeginPosition { get; set; }
        public virtual DateTime? EndPosition { get; set; }

        //Version
        public virtual int SequenceNumber { get; set; }
        public virtual int CorrectionNumber { get; set; }

        //util
        public virtual DateTime SubmitDate { get; set; }
        public virtual int Interpretation { get; set; }
        public virtual bool IsCanceled { get; set; }

        //data
        public virtual byte[] Data { get; set; }
    }
}
