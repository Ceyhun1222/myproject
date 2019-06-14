#region

using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.Implementation.Storage;
using ImpromptuInterface;

#endregion

namespace Aran.Temporality.Aim.Storage
{
    public class AimStorageFactory
    {
        #region Get storage

        private static readonly Dictionary<string, IStorage<Feature>> Storages =
            new Dictionary<string, IStorage<Feature>>();

        public static IStorage<Feature> GetLocalStorage(string path)
        {
            lock (Storages)
            {
                IStorage<Feature> result;

                if (!Storages.TryGetValue(path, out result))
                {
                    result = CreateLocal(path);
                    Storages[path] = result;
                }

                return result;
            }
        }

        #endregion

        #region Create Storage (local and remote)

        public static IStorage<Feature> CreateLocal(string path)
        {
            StorageFactory.Init();
            var result = new AimStorage(path);
            return result;
        }

        public static IStorage<Feature> CreateRemote(string path, string username, string password,
                                                     string serviceAddress = "localhost:8523")
        {
            StorageFactory.Init();
            var remoteStorage = new AimRemoteStorage(path, username, password, serviceAddress);
            return remoteStorage.ActLike<IStorage<Feature>>();
        }

        #endregion
    }
}