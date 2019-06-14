using System;
using Aran.Temporality.Internal.Implementation.Storage;

namespace Aran.Temporality.Internal.Remote.Service
{
    internal class RemoteHelperService : IRemoteHelperService
    {
        #region Implementation of IRemoteHelperService

        public DateTime GetServerTime(int userId)
        {
            //StorageService.UserPing(userId);
            return DateTime.Now;
        }

        public string GetUserName(int userId)
        {
            return StorageService.GetUserName(userId);
        }

        public bool IsUserSecured(int userId)
        {
            return StorageService.IsUserSecured(userId);
        }


        #endregion
    }
}
