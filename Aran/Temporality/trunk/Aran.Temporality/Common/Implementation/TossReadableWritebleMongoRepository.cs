using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.TossConverter.Interface;
using Aran.Temporality.Common.Mongo;
using Aran.Temporality.Internal.Implementation.Repository.Linq;
using Aran.Temporality.Internal.MetaData.Offset;
using MongoDB.Driver;
using Aran.Aim.Data.Filters;
using Aran.Aim;
using Aran.Temporality.Common.Config;
using MongoDB.Bson;
using Aran.Temporality.Common.Interface;

namespace Aran.Temporality.Common.Implementation
{
    internal class TossReadableWritebleMongoRepository : ITossWritableRepository, ITossReadableRepository
    {
        protected string RepositoryName { get; }
        protected readonly Dictionary<string, MongoRepository> _eventRepositories;

        public TossReadableWritebleMongoRepository(string repositoryName)
        {
            RepositoryName = repositoryName;
            _eventRepositories = new Dictionary<string, MongoRepository>();
        }

        protected MongoRepository GetEventRepository(string marker, bool rewrite, bool createIndex)
        {
            lock (this)
            {
                if (!_eventRepositories.ContainsKey(marker))
                    _eventRepositories[marker] = new MongoRepository(RepositoryName, marker, rewrite, createIndex);
                return _eventRepositories[marker];
            }   
        }

        public AimFeature AddEvent(int workPackage, AbstractEvent<AimFeature> abstractEvent, bool createGeoIndex)
        {
            var newEventRepository = GetEventRepository($"events_{workPackage}", true, createGeoIndex);

            var key = newEventRepository.Add(abstractEvent?.Data, new OffsetEventMetaData<string>(abstractEvent));

            AimFeature checkEvent = null;

            checkEvent = newEventRepository.Get(key, abstractEvent.FeatureTypeId);

            return checkEvent;
        }

        public bool IsExist()
        {
            return MongoClientCustom.Instance.IsExistDatabase(RepositoryName);
        }

        public void ClearRepository()
        {
            MongoClientCustom.Instance.DropDatabase(RepositoryName).Wait();
        }

        public List<int> GetWorkPackages()
        {
            var database = MongoClientCustom.Instance.GetDatabase(RepositoryName);
            var collections = database.ListCollections().ToList();
            collections.Sort();
            var workPackages = new List<int>();

            foreach (var collection in collections)
            {
                string stringWorkpackage = string.Join("", collection["name"].AsString.Where(char.IsDigit));

                if (int.TryParse(stringWorkpackage, out var intWorkpackage) && !workPackages.Contains(intWorkpackage))
                    workPackages.Add(intWorkpackage);
            }

            return workPackages;
        }

        public IEnumerable<AbstractEvent<AimFeature>> GetEventStorages(int workPackage)
        {
            var repository = GetEventRepository($"events_{workPackage}", false, false);
            return repository.GetEvents();
        }
    }
}
