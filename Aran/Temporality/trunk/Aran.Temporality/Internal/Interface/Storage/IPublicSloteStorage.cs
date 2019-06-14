using Aran.Temporality.Common.Entity;
using System.Collections.Generic;
using Aran.Temporality.Common.Entity.Enum;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IPublicSlotStorage : ICrudStorage<PublicSlot>
    {
        bool UpdatePublicSlot(PublicSlot publicSlot);
        void ResetSlotStatus();
        PublicSlot GetFirstAndSetStatus(SlotStatus initialStatus, SlotStatus nextStatus);
    }
}
