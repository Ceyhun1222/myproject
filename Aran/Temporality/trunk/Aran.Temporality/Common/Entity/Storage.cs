using System;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    [Serializable]
    public class Storage : INHibernateEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }
}
