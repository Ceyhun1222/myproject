using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Internal.Implementation.Repository.NHibernate.Entity
{
    internal enum ViolationType
    {

    }

    [Serializable]
    public class Violation : INHibernateEntity
    {
        public virtual int Id { get; set; }
        public virtual string Storage { get; set; } //can not be null

        public virtual int WorkPackage { get; set; } 

        public virtual Guid ViolatorGuid { get; set; }
        public virtual int ViolatorFeatureTypeId { get; set; }

        public virtual Guid TargetGuid { get; set; }
        public virtual int TargetFeatureTypeId { get; set; }

        public virtual int ViolationType { get; set; }

    }
}
