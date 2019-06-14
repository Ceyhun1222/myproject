using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Interface;

namespace Aran.Temporality.Common.Helper
{
    public class StateHelper
    {
        private readonly ITemporalityService<AimFeature> _service;

        public StateHelper(ITemporalityService<AimFeature> service)
        {
            _service = service;
        }


    }
}
