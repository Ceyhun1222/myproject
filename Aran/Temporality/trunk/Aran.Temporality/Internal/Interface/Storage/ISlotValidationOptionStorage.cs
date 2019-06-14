using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Temporality.Common.Entity;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface ISlotValidationOptionStorage : ICrudStorage<SlotValidationOption>
    {
        SlotValidationOption GetOptionBySlotId(int slotId);
        bool UpdateSlotValidationOption(int slotId, SlotValidationOption newOption);
    }
}
