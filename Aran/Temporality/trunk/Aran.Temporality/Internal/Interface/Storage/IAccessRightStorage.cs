using System.Collections.Generic;
using Aran.Temporality.Common.Entity;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IAccessRightStorage : ICrudStorage<AccessRightZipped>
    {
        //Workpackage operations 
        void DeleteAccessRightsByWorkPackage(int packageId);
        //UserRights
        AccessRightZipped GetUserRights(int userId, int storageId, int wp);
        IList<AccessRightZipped> GetUserRights(int userId);
        bool SetUserRights(AccessRightZipped rightZipped);
    }
}
