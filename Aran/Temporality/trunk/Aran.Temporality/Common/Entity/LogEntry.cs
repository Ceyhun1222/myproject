using System;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    [Serializable]
    public class LogEntry : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual bool AccessGranted { get; set; }

        public virtual string Ip { get; set; }

        public virtual string Storage { get; set; }
        public virtual string Application { get; set; }
        public virtual string UserName { get; set; }
        public virtual int UserId { get; set; }
        public virtual string Action { get; set; }

        [StringLength(400)]
        public virtual string Parameters { get; set; }
    }
}
