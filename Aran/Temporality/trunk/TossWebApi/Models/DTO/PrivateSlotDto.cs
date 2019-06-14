using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using System;

namespace TossWebApi.Models.DTO
{
    public class PrivateSlotDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public SlotStatus Status { get; set; }
        
        public DateTime CreationDate { get; set; }
    }
}