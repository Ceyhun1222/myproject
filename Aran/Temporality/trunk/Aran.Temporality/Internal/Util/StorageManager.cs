using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Internal.Abstract.Storage;
using Aran.Temporality.Internal.Implementation.Repository;
using Aran.Temporality.Internal.Implementation.Repository.Linq;
using Aran.Temporality.Internal.Implementation.Storage.Final;
using Aran.Temporality.Internal.MetaStorage;

namespace Aran.Temporality.Internal.Util
{
    internal class StorageManager<TDataType> where TDataType : class
    {
        #region Private

        private static Type GetTypeForRepository(RepositoryType repository)
        {
            switch (repository)
            {
                case RepositoryType.MongoRepository:
                case RepositoryType.MongoWithBackupRepository:
                    return typeof(string);
                default:
                    return typeof(long);
            }
        }

        #endregion

        #region Public

        public static AbstractEventStorage<TDataType> GetAbstractEventStorage()
        {
            var eventStorageType = ConfigUtil.RepositoryType;
            var t = typeof(AimEventStorage<>);

            Type[] typeArgs = { GetTypeForRepository(eventStorageType) };

            var makeme = t.MakeGenericType(typeArgs);
            return (AbstractEventStorage<TDataType>)Activator.CreateInstance(makeme);
        }

        public static IEnumerable GetEventRepository(string path, string marker)
        {
            var repository = ConfigUtil.RepositoryType;

            switch (repository)
            {
                case RepositoryType.MongoRepository:
                    return new MongoRepository(path, marker, false, ConfigUtil.MongoCreateGeoIndex);
                case RepositoryType.MongoWithBackupRepository:
                    return new MongoWithBackupRepository(path, marker, false, ConfigUtil.MongoCreateGeoIndex);
                default:
                    return new NoDeleteOffsetRepository<TDataType>(path, marker);
            }
        }

        #endregion
    }
}
