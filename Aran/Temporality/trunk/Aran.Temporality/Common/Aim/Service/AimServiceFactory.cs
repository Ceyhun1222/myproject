#region

using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.Implementation.Storage;
using Aran.Temporality.Internal.Service;
using ImpromptuInterface;

#endregion

namespace Aran.Temporality.Common.Aim.Service
{
    public class AimServiceFactory
    {
        #region Get storage

        private static readonly Dictionary<string, ITemporalityService<AimFeature>> Storages =
            new Dictionary<string, ITemporalityService<AimFeature>>();

        private static ITemporalityService<AimFeature> GetLocalStorage(string storage, out INoAixmDataService noDataService)
        {
            if (String.IsNullOrEmpty(storage))
            {
                throw new Exception("Storage can not be null");
            }

            lock (Storages)
            {
                ITemporalityService<AimFeature> result;

                if (!Storages.TryGetValue(storage, out result))
                {
                    result = new AimTemporalityServiceProxy(storage);
                    Storages[storage] = result;
                }

                noDataService = result as AimTemporalityServiceProxy;
                return result;
            }
        }

        private static ITemporalityService<AimFeature> GetLocalStorage(string storage)
        {
            if (String.IsNullOrEmpty(storage))
            {
                throw new Exception("Storage can not be null");
            }

            lock (Storages)
            {
                ITemporalityService<AimFeature> result;

                if (!Storages.TryGetValue(storage, out result))
                {
                    result = new AimTemporalityServiceProxy(storage);
                    Storages[storage] = result;
                }

                return result;
            }
        }

        public static void Setup()
        {
            StorageService.NeedSetup = true;
        }

        #endregion

        #region Open Storage (local and remote)


        public static ITemporalityService<AimFeature> OpenLocal(string storage, out INoAixmDataService noDataService)
        {
            StorageService.Init();
            return GetLocalStorage(storage, out noDataService);
        }

        public static ITemporalityService<AimFeature> OpenLocal(string storage)
        {
            StorageService.Init();
            return GetLocalStorage(storage);
        }

        public static ITemporalityService<AimFeature> OpenRemote(string storage,
            string userId,
            string userPassword,
            string serviceAddress,
            out INoAixmDataService noDataService)
        {
            var remoteStorage = new AimRemoteService(storage, userId, userPassword, serviceAddress);
            if (!remoteStorage.IsOpened)
            {
                remoteStorage.Close();
                noDataService = null;
                return null;
            }

            noDataService = remoteStorage.ActLike<INoAixmDataService>();

            return remoteStorage.ActLike<ITemporalityService<AimFeature>>();
        }

        #endregion

    }
}