using System.Collections.Generic;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IPrivateSlotStorage : ICrudStorage<PrivateSlot>
    {
        IList<PrivateSlot> GetPrivateSlots(int publicSlotId, int userId);

        bool UpdatePrivateSlot(PrivateSlot privateSlot);

        PrivateSlot GetFirstAndSetStatus(SlotStatus initialStatus, SlotStatus nextStatus);

        void PreStart();

        void ResetSlotStatus();

        bool DeleteById(int id);
    }
}
