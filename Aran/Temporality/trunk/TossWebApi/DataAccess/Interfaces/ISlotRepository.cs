using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using System.Collections.Generic;
using TossWebApi.Models.DTO;

namespace TossWebApi.DataAccess.Interfaces
{
    public interface ISlotRepository
    {
        IEnumerable<PublicSlotDto> GetSlots(SlotStatus[] excludedSlotStatuses = null, PublicSlotType[] includedSlotTypes = null, int userId = 0);

        IEnumerable<PublicSlotDto> GetPublicSlots(SlotStatus[] excludedSlotStatuses, PublicSlotType[] includedSlotTypes = null);

        IEnumerable<PrivateSlotDto> GetPrivateSlots(int publicSlotId, SlotStatus[] excludedSlotStatuses, int userId = 0);

        int CreatePublicSlot(PublicSlotDto publicSlotDto);

        int CreatePrivateSlot(int publicSlotId, PrivateSlotDto privateSlotDto);
    }
}
