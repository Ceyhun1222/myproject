using System;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    [Serializable]
    public class UserModuleVersion : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual User User { get; set; }

        public virtual string Module { get; set; } //use Module enum

        [StringLength(50)]
        public virtual string ActualVersion { get; set; }
    }
}
