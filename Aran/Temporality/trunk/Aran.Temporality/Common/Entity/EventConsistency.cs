using System;
using System.ComponentModel;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    [Serializable]
    public class EventConsistency : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual RepositoryType RepositoryType { get; set; }

        [StringLength(64)]
        public virtual string StorageName { get; set; }

        public virtual int WorkPackage { get; set; }

        public virtual FeatureType FeatureType { get; set; }

        public virtual Guid? Identifier { get; set; }

        public virtual Interpretation Interpretation { get; set; }

        public virtual int SequenceNumber { get; set; }

        public virtual int CorrectionNumber { get; set; }

        public virtual DateTime? ValidTimeBegin { get; set; }

        public virtual DateTime? ValidTimeEnd { get; set; }

        public virtual DateTime SubmitDate { get; set; }

        [StringLength(128)]
        public virtual string Hash { get; set; }

        public virtual DateTime CalculationDate { get; set; } = DateTime.Now;
    }
}
