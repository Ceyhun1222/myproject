using System;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Interface;

namespace Aran.Temporality.Common.Helper
{
    public class StorageHelper
    {
        private readonly ITemporalityService<AimFeature> _service;

        public StorageHelper(ITemporalityService<AimFeature> service)
        {
            _service = service;
        }


        public DateTime GetServerTime()
        {
            return _service.GetServerTime();
        }

        public void Truncate()
        {
            _service.Truncate();
        }

    }
}
