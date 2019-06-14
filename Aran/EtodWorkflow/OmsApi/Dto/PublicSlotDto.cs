using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OmsApi.Entity;

namespace OmsApi.Dto
{
    public class PublicSlotDto : IBaseDto
    {
        public string Name { get; set; }

        public DateTime EffectiveDate { get; set; }

        public SlotStatus Status { get; set; }

        public SlotType Type { get; set; }

        public IList<PrivateSlotDto> PrivateSlots { get; set; }

        public long Id { get; set ; }
    }    
}
