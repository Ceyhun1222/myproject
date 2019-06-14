using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Internal.Implementation.Repository.NHibernate.Entity
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
