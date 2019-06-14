using System;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Enum;

namespace Aran.Temporality.Internal.Service.Validation
{
    internal class EventValidation: ValidationBase
    {
        public EventValidation(AimTemporalityService aimTemporalityService) : base(aimTemporalityService)
        {
        }



        private void IsNull(AbstractEvent<AimFeature> abstractEvent)
        {
            if(abstractEvent == null)
                Error("Event can't be null.");
        }
    }
}