using OmsApi.Dto;
using OmsApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class SlotDto : IBaseDto
    {
        public string Name { get; set; }

        public DateTime EffectiveDate { get; set; }

        public SlotStatus Status { get; set; }

        public SlotType Type { get; set; }

        public PrivateSlotDto Private { get; set; }

        public long Id { get; set; }
    }
}