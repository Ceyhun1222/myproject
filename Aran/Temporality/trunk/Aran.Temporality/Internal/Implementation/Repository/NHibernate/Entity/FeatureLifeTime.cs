using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Internal.Implementation.Repository.NHibernate.Entity
{
    [Serializable]
    public class FeatureLifeTime : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual Guid Guid { get; set; }
        public virtual int FeatureTypeId { get; set; }
        public virtual int WorkPackage { get; set; }


        public virtual DateTime BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

    }
}
