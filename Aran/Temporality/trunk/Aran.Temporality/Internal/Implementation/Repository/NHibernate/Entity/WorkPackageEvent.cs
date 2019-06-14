using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Internal.Implementation.Repository.NHibernate.Entity
{
    [Serializable]
    internal class WorkPackageEvent : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual string Storage { get; set; }
        public virtual int WorkPackage { get; set; }
        public virtual DateTime FirstEventDate { get; set; }

        public virtual int SequenceNumber { get; set; }
        public virtual Guid FeatureId { get; set; }
    }
}
