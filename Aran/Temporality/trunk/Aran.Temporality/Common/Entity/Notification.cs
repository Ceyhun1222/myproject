using System;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    [Serializable]
    public class Notification : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual User User { get; set; }

        [StringLength(4000)]
        public virtual string Content { get; set; }
    }
}
