using Aran.Temporality.Common.Entity;
using Aran.Temporality.Internal.Service;

namespace Aran.Temporality.Internal.WorkFlow
{
    internal interface ICheckPrivateSlotRoutine
    {
        bool CheckPrivateSlot(PrivateSlot slot, AimTemporalityService service);
    }
}
