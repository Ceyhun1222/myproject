using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    public enum AimslOperationStatusType
    {
        [Description("Open")]
        Opened,
        [Description("Closed")]
        Closed,
        [Description("Destroyed")]
        Destroyed
    }

    [Serializable]
    public class AimslOperation : INHibernateEntity
    {
        public virtual int Id { get; set; }
        public virtual string JobId { get; set; }
        public virtual string FileName { get; set; }
        public virtual string Username { get; set; }
        public virtual string Description { get; set; }
        public virtual string PullPoint { get; set; }
        public virtual string Subscription { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual DateTime LastChangeTime { get; set; }
        public virtual string Status { get; set; }
        public virtual string Messages { get; set; }
        public virtual AimslOperationStatusType InternalStatus { get; set; } = AimslOperationStatusType.Opened;
    }
}
