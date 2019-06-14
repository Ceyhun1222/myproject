using System;
using System.Collections;
using System.Collections.Generic;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.Mongo;
using Aran.Temporality.Internal.Abstract.Storage;
using Aran.Temporality.Internal.Interface;
using Aran.Temporality.Internal.Interface.Storage;
using MongoDB.Driver;
using NHibernate.Persister.Entity;

namespace Aran.Temporality.Internal.Implementation.Storage.MongoStorage
{
    internal class CrudStorageTemplate<T> : ICrudStorage<T> where T : class
    {
        private const string DatabaseName = "NoAixm";

        protected IMongoCollection<T> Collection { get; } = MongoClientCustom.Instance.GetDatabase(DatabaseName).GetCollection<T>(typeof(T).Name);

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public int CreateEntity(T entity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteEntityById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAllEntities()
        {
            throw new NotImplementedException();
        }

        public T GetEntityById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
