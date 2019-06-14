using System;
using System.ComponentModel;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Exceptions;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    public enum PublicSlotType
    {
        [Description("Permanent Delta")]
        PermanentDelta,
        [Description("Temporary Delta")]
        TemporaryDelta,
        [Description("Mixed")]
        Mixed
    }

    [Serializable]
    public class PublicSlot : INHibernateEntity
    {
        public virtual int Id { get; set; }

        [StringLength(400)] 
        public virtual String Name { get; set; }

        public virtual DateTime EffectiveDate { get; set; }
        public virtual DateTime EndEffectiveDate { get; set; }

        public virtual DateTime PlannedCommitDate { get; set; }

        public virtual SlotStatus Status { get; set; }
        public virtual int SlotType { get; set; }

        public virtual DateTime StatusChangedDate { get; set; }

        [NotPersisted]
        public virtual bool Editable => !(Status == SlotStatus.Expired
                                  || Status == SlotStatus.Published
                                  || Status == SlotStatus.Publishing
                                  || Status == SlotStatus.Publishing
                                  || Status == SlotStatus.Checking
                                  || Status == SlotStatus.ToBeChecked);

        [NotPersisted]
        public virtual bool Frozen => Status == SlotStatus.Published
                              || Status == SlotStatus.Publishing
                              || Status == SlotStatus.ToBePublished
                              || Status == SlotStatus.Expired;
    }
}
