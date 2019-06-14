using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    public class ModuleChangeLog : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual Module Module { get; set; } //use Module enum

        [StringLength(50)]
        public virtual string Version { get; set; }

        [StringLength(5000)]
        public virtual string Change { get; set; }

    }
}
