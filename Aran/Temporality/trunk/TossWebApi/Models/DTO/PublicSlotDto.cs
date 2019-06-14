using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TossWebApi.Models.DTO
{
    public class PublicSlotDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime PlannedCommitDate { get; set; }

        public PublicSlotType SlotType { get; set; }

        public SlotStatus Status { get; set; }

        public List<PrivateSlotDto> PrivateSlots { get; set; }
    }
}