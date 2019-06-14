using System;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    public enum ValidationOption
    {
        CheckSyntax =           1 << 0,
        CheckLinks  =           1 << 1,
        CheckBusinessRules =    1 << 2,
        CheckMore =             1 << 3
    }

    [Serializable]
    public class SlotValidationOption : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual PrivateSlot PrivateSlot { get; set; }

        public virtual int Flag { get; set; }

        public virtual byte[] MoreOptions { get; set; }
    }
}
