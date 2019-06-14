using Aran.Temporality.Common.Entity;
using Aran.Temporality.Internal.Service;

namespace Aran.Temporality.Internal.WorkFlow.Routines
{
    internal class PublishRoutine
    {

        public static void PublishPublicSlot(PublicSlot publicSlot, AimTemporalityService aimTemporalityService)
        {
            aimTemporalityService.PublishPublicSlot(publicSlot);
        }
    }
}
