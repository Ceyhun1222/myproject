using System;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    [Serializable]
    public class FeatureRelation : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual Guid SourceGuid { get; set; }
        public virtual int SourceFeatureTypeId { get; set; }

        public virtual int WorkPackage { get; set; }

        public virtual Guid TargetGuid { get; set; }
        public virtual int TargetFeatureTypeId { get; set; }

        public virtual DateTime StartDate { get; set; }
        public virtual int SequenceNumber { get; set; }
    }
}
