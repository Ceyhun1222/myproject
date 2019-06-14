using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Entity
{
    [Table("Slot")]
    public class Slot :  Base
    {
        public string Name { get; set; }

        public DateTime EffectiveDate { get; set; }

        public SlotStatus Status { get; set; }

        public SlotType Type { get; set; }

        public long TossId { get; set; }

        public PrivateSlot Private { get; set; }
    }

    [Owned]
    public class PrivateSlot
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public long Id { get; set; }
        public long TossId { get; set; }

        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public SlotStatus Status { get; set; }

    }

    public enum SlotStatus
    {
        Empty,
        Opened,
        ToBeChecked,
        Checking,
        CheckFailed,
        PublishingFailed,
        ValidatedOk,
        CheckOk,
        Expired,
        ToBePublished,
        Publishing,
        Published,
        CheckCancelled
    }

    public enum SlotType
    {
        PermanentDelta,
        TemporaryDelta,
        Mixed
    }
}