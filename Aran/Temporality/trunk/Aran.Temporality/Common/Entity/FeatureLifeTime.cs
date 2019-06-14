using System;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
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
