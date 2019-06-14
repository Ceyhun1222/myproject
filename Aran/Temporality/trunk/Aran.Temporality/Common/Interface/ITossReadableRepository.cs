using System.Collections.Generic;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;

namespace Aran.Temporality.Common.Interface
{
    public interface ITossReadableRepository
    {
        List<int> GetWorkPackages();
        IEnumerable<AbstractEvent<AimFeature>> GetEventStorages(int workPackage);
    }
}
