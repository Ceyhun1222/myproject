using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Exceptions;
using Aran.Temporality.Common.Interface;

namespace Aran.Temporality.Internal.Service.Validation
{
  
    internal class SlotValidation : ValidationBase
    {
        public SlotValidation(AimTemporalityService aimTemporalityService) : base(aimTemporalityService)
        {
        }

        public void IsSlotOpen(IFeatureId id)
        {
            var slot = AimTemporalityService.GetPrivateSlotById(id.WorkPackage);
            if (slot.PublicSlot.Status == SlotStatus.Expired)
                Error("Can't write to expired slot.");
            if (slot.PublicSlot.Status == SlotStatus.Published
                || slot.PublicSlot.Status == SlotStatus.Publishing
                || slot.PublicSlot.Status == SlotStatus.ToBePublished)
                Error("Can't write to published or publishing slot.");
            if (slot.PublicSlot.Status == SlotStatus.Checking
                || slot.PublicSlot.Status == SlotStatus.ToBeChecked)
                Error("Can't write to checking slot.");
        }

        public void IsNull(IFeatureId id)
        {
            if (id == null)
                Error("FeatureId can not be null");
        }



        public void IsPrivateSlot(IFeatureId id)
        {
            if (id.WorkPackage == 0)
                Error("Can't write to public data. Only to private slot");
        }

        public void PrivateSlotWriteValidation(IFeatureId id)
        {
            IsNull(id);
            IsPrivateSlot(id);
            IsSlotOpen(id);
        }
    }
}