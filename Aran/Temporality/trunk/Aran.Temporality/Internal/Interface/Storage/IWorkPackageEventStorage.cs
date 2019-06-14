using System;
using Aran.Temporality.Common.Abstract.Event;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IWorkPackageEventStorage
    {
        void SetWorkPackageEvent(int storageId, AbstractEventMetaData newEvent, AbstractEventMetaData lastEventCorrection);
        DateTime? GetFirstWorkPackageEvent(string storage, int workPackage);
    }
}
