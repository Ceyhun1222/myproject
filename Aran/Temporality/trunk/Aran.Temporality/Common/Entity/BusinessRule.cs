using System;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    [Serializable]
    public class BusinessRule : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int RuleId { get; set; }
    }
}
