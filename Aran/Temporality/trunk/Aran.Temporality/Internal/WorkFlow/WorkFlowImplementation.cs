using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Internal.Implementation.Storage;
using Aran.Temporality.Internal.Service;
using Aran.Temporality.Internal.WorkFlow.Routines;

namespace Aran.Temporality.Internal.WorkFlow
{
    internal class WorkFlowImplementation
    {
        public static bool DoWorkflow(AimTemporalityService service)
        {
            {
                var privateSlot = StorageService.GetFirstPrivateSlotAndSetStatus(SlotStatus.ToBeChecked, SlotStatus.Checking);
                if (privateSlot != null)
                {
                    CheckRoutine.CheckPrivateSlot(privateSlot, service);
                    return true;
                }
            }
            //
            {
                //var publicSlot = StorageService.GetFirstPublicSlotAndSetStatus(SlotStatus.ToBeChecked, SlotStatus.Checking);
                //if (publicSlot != null)
                //{
                //    CheckRoutine.CheckPublicSlot(publicSlot, service);
                //    return true;
                //}
            }
            //
            {
                var publicSlot = StorageService.GetFirstPublicSlotAndSetStatus(SlotStatus.ToBePublished, SlotStatus.Publishing);
                if (publicSlot != null)
                {
                    PublishRoutine.PublishPublicSlot(publicSlot, service);
                    return true;
                }
            }

            {
                var operations = StorageService.GetAllActiveAimslOperations();
                if (operations != null && operations.Count > 0)
                {
                    AimslUploadRoutine.Proccess(operations, service);
                    return true;
                }



            }


            return false;
        }
    }
}
