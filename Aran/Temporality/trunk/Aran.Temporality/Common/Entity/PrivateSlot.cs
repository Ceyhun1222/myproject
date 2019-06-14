using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute;
using Aran.Temporality.Internal.Interface;
using Newtonsoft.Json;

namespace Aran.Temporality.Common.Entity
{
    [Serializable]
    public class PrivateSlot : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual PublicSlot PublicSlot { get; set; }

        [StringLength(400)]
        public virtual String Name { get; set; }

        public virtual SlotStatus Status { get; set; }

        public virtual DateTime StatusChangeDate { get; set; }

        public virtual DateTime CreationDate { get; set; }

        [StringLength(4000)]
        public virtual String Reason { get; set; }

        public virtual int OriginatorSlotId { get; set; }
       [JsonIgnore]
        public virtual IList<int> RelatedSlotId { get; set; }
    }
}
