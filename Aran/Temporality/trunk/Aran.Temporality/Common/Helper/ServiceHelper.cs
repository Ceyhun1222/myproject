using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Interface;

namespace Aran.Temporality.Common.Helper
{
    public class ServiceHelper
    {
        private ITemporalityService<AimFeature> _service;
        public ServiceHelper(ITemporalityService<AimFeature> service)
        {
            _service = service;
            //
            Event =new EventHelper(service);
            State = new StateHelper(service);
            Storage = new StorageHelper(service);
        }

        public EventHelper Event { get; }

        public StateHelper State { get; }

        public StorageHelper Storage { get; }
    }
}
