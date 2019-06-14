using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Temporality.Internal.Abstract.Repository;
using Aran.Temporality.Common.Mongo;
using MongoDB.Driver;
using Aran.Temporality.Internal.MetaData.Offset;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Interface;
using Aran.Aim.Utilities;
using MongoDB.Bson;

namespace Aran.Temporality.Internal.Implementation.Repository.Linq
{
    internal class MongoRepository : AbstractAimRepository<string>
    {
        #region Private fields

        private IMongoDatabase _database;
        private SortedList<int, IMongoCollection<MongoWrapper<AimEvent>>> _collections;
        private bool _createGeoIndex;
        private List<int> _featuresTypeId;

        #endregion

        #region Constants

        private const string FeatureTypeIdPath = "Value.p1_MetaData.p1_FeatureTypeId";
        private const string GuidPath = "Value.p1_MetaData.p2_Guid";
        private const string SequenceNumberPath = "Value.p1_MetaData.p6_SequenceNumber";
        private const string CorrectionNumberPath = "Value.p1_MetaData.p7_CorrectionNumber";

        #endregion

        #region Constructors

        private MongoRepository()
        {
            _collections = new SortedList<int, IMongoCollection<MongoWrapper<AimEvent>>>();
            _featuresTypeId = Enum.GetValues(typeof(FeatureType)).Cast<int>().ToList();
        }

        private MongoRepository(string repositoryName, string marker, bool rewrite) : this()
        {
            RepositoryName = repositoryName;
            Marker = marker.Replace("events\\", "");
            Open(rewrite);
        }

        public MongoRepository(string repositoryName, string marker, bool rewrite, bool createGeoIndex) : this(repositoryName, marker, rewrite)
        {
            _createGeoIndex = createGeoIndex;
        }

        #endregion

        #region Methods

        private IMongoCollection<MongoWrapper<AimEvent>> Collection(int featureTypeId)
        {
            if (!_collections.Keys.Contains(featureTypeId))
            {
                _collections[featureTypeId] = _database.GetCollection<MongoWrapper<AimEvent>>($"{Marker}_{(FeatureType)featureTypeId}");

                if (_createGeoIndex)
                {
                    CreateGeoIndexes(featureTypeId);
                }
            }

            return _collections[featureTypeId];
        }

        public List<string> CreateGeoIndexes(int featureTypeId)
        {
            if (!_collections.Keys.Contains(featureTypeId))
            {
                _collections[featureTypeId] = _database.GetCollection<MongoWrapper<AimEvent>>($"{Marker}_{(FeatureType)featureTypeId}");
            }

            var pathes = AimMetadataUtility.GetGeometryPathes(featureTypeId).Select(x => MongoFilter.GenerateMongoPath(featureTypeId, x)).ToList();

            if (pathes.Count > 0)
            {
                return _collections[featureTypeId].Indexes.CreateMany(pathes.Select((path, number) => new CreateIndexModel<MongoWrapper<AimEvent>>(
                        Builders<MongoWrapper<AimEvent>>.IndexKeys.Geo2DSphere(path),
                        new CreateIndexOptions { Name = $"geo_index_{number}" }
                )))?.ToList();
            }

            return null;
        }

        public bool IsFeaturesExist(int featureTypeId)
        {
            if (_collections.Keys.Contains(featureTypeId))
                return true;

            var filter = new BsonDocument("name", $"{Marker}_{(FeatureType)featureTypeId}");
            var options = new ListCollectionsOptions { Filter = filter };

            return _database.ListCollections(options).Any();
        }

        #endregion

        #region Implementation of AbstractDataRepository

        public sealed override void Open(bool rewrite = false)
        {
            _database = MongoClientCustom.Instance.GetDatabase(RepositoryName);

            if (rewrite)
                RemoveAll();
        }

        public override void Close()
        {
            // empty
        }

        public override string Add(AimFeature item)
        {
            throw new NotImplementedException();
        }

        public override AimFeature Get(string key, int featureTypeId, Projection projection = null)
        {
            if (!IsFeaturesExist(featureTypeId))
                return null;

            var mongoProjection = projection.GetMongoProjection(featureTypeId);
            var filterBuilder = new FilterDefinitionBuilder<MongoWrapper<AimEvent>>();
            var filter = filterBuilder.Eq(wrapper => wrapper.Id, key);
            var res = Collection(featureTypeId).Find(filter).Project<MongoWrapper<AimEvent>>(mongoProjection).FirstOrDefault();
            return res?.Value.Data;
        }

        public override List<AimFeature> Get(IList<string> keys, int featureTypeId, Projection projection = null)
        {
            if (!IsFeaturesExist(featureTypeId))
                return null;

            if (keys.Count == 1)
                return new List<AimFeature> { Get(keys.First(), featureTypeId, projection) };

            var mongoProjection = projection.GetMongoProjection(featureTypeId);
            var filterBuilder = new FilterDefinitionBuilder<MongoWrapper<AimEvent>>();
            var filter = filterBuilder.In(x => x.Id, keys.ToArray());
            return Collection(featureTypeId).Find(filter).Project<MongoWrapper<AimEvent>>(mongoProjection).ToEnumerable().Select(x => x.Value.Data).ToList();
        }

        public override void Remove(AimFeature item)
        {
            throw new NotImplementedException();
        }

        public override void RemoveByKey(string key, int featureTypeId)
        {
            Collection(featureTypeId).DeleteOne(x => x.Id.Equals(key));
        }

        public override void RemoveAll()
        {
            _database.DropCollection(Marker);
        }

        #endregion

        #region Implementation of IDisposable

        public override void Dispose()
        {
            RemoveAll();
            Close();
        }

        #endregion

        #region Implementation of AbstractLinqDataRepository

        public override IEnumerable<AimFeature> Where(Func<AimFeature, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public override AimFeature Poke()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            foreach (var featureTypeId in _featuresTypeId)
            {
                if (!IsFeaturesExist(featureTypeId))
                    continue;

                foreach (var aimFeature in Collection(featureTypeId).AsQueryable().Select(x => x.Value.Data))
                {
                    yield return aimFeature;
                }
            }
        }

        #endregion

        #region Implementation of AbstractOffsetDataRepository

        public override IEnumerable GetMetaDatas()
        {
            foreach (var featureTypeId in _featuresTypeId)
            {
                if (!IsFeaturesExist(featureTypeId))
                    continue;

                var res = Collection(featureTypeId).Find(_ => true).Project<MongoWrapper<AimEvent>>("{\"Value.p1_MetaData\":1}").ToEnumerable();

                foreach (var offsetEventMetaData in res.Select(wrapper => new OffsetEventMetaData<string>(wrapper.Value) { Offset = wrapper.Id }))
                {
                    yield return offsetEventMetaData;
                }
            }
        }

        public override string Add(AimFeature item, OffsetEventMetaData<string> meta)
        {
            var mongoObject = new MongoWrapper<AimEvent>(new AimEvent(meta) { Data = item });

            Collection(meta.FeatureTypeId).InsertOne(mongoObject);
            return mongoObject.Id;
        }

        public override List<Guid> GetFilteredId(Filter filter, int featureTypeId, IList<Guid> ids = null)
        {
            if (!IsFeaturesExist(featureTypeId))
                return new List<Guid>();

            var filters = new List<FilterDefinition<MongoWrapper<AimEvent>>>();
            var builder = new FilterDefinitionBuilder<MongoWrapper<AimEvent>>();

            if (ids != null)
                filters.Add(builder.In(GuidPath, ids));

            filters.Add(builder.Eq(FeatureTypeIdPath, featureTypeId));

            var mongoFilters = filter.GetMongoFilter<AimEvent>(featureTypeId, out var geoPathes);

            filters.Add(mongoFilters);

            var mongoFilter = builder.And(filters);

            return Collection(featureTypeId).Distinct<Guid>(GuidPath, mongoFilter).ToList();
        }

        #endregion

        #region Implementation of AbstractAimRepository
        
        public override AimEvent Get(int featureTypeId, Guid identifier, int sequenceNumber, int correctionNumber,
            Projection projection = null)
        {
            if (!IsFeaturesExist(featureTypeId))
                return null;

            if (identifier == Guid.Empty)
                return null;

            var mongoProjection = projection.GetMongoProjection(featureTypeId);

            var filterBuilder = new FilterDefinitionBuilder<MongoWrapper<AimEvent>>();
            var filter = filterBuilder.Eq(FeatureTypeIdPath, featureTypeId);
            filter = filter & filterBuilder.Eq(GuidPath, identifier);
            filter = filter & filterBuilder.Eq(SequenceNumberPath, sequenceNumber);
            filter = filter & filterBuilder.Eq(CorrectionNumberPath, correctionNumber);

            var res = Collection(featureTypeId).Find(filter).Project<MongoWrapper<AimEvent>>(mongoProjection).FirstOrDefault();
            return res?.Value;
        }

        public override List<AimEvent> Get(int featureTypeId, Guid? identifier, Projection projection)
        {
            if (!IsFeaturesExist(featureTypeId))
                return new List<AimEvent>();

            var mongoProjection = projection.GetMongoProjection(featureTypeId);

            var filterBuilder = new FilterDefinitionBuilder<MongoWrapper<AimEvent>>();
            var filter = filterBuilder.Eq(FeatureTypeIdPath, featureTypeId);
            if (identifier != null)
                filter = filter & filterBuilder.Eq(GuidPath, identifier);

            var res = Collection(featureTypeId).Find(filter).Project<MongoWrapper<AimEvent>>(mongoProjection).ToList().Select(x => x.Value).ToList();
            return res;
        }

        public override AimEvent GetEvent(string key, int featureTypeId, Projection projection = null)
        {
            if (!IsFeaturesExist(featureTypeId))
                return null;

            var mongoProjection = projection.GetMongoProjection(featureTypeId);
            var filterBuilder = new FilterDefinitionBuilder<MongoWrapper<AimEvent>>();
            var filter = filterBuilder.Eq(wrapper => wrapper.Id, key);
            var res = Collection(featureTypeId).Find(filter).Project<MongoWrapper<AimEvent>>(mongoProjection).FirstOrDefault();
            return res?.Value;
        }

        public override List<AimEvent> GetEvents(IList<string> keys, int featureTypeId, Projection projection = null)
        {
            if (!IsFeaturesExist(featureTypeId))
                return new List<AimEvent>();

            if (keys.Count == 1)
                return new List<AimEvent> { GetEvent(keys.First(), featureTypeId, projection) };

            var mongoProjection = projection.GetMongoProjection(featureTypeId);
            var filterBuilder = new FilterDefinitionBuilder<MongoWrapper<AimEvent>>();
            var filter = filterBuilder.In(x => x.Id, keys.ToArray());
            var res = Collection(featureTypeId).Find(filter).Project<MongoWrapper<AimEvent>>(mongoProjection).ToList().Select(x => x.Value).ToList();
            return res;
        }

        public override IEnumerable<AimEvent> GetEvents()
        {
            foreach (var featureTypeId in _featuresTypeId)
            {
                if (!IsFeaturesExist(featureTypeId))
                    continue;

                foreach (var aimEvent in Collection(featureTypeId).AsQueryable().Select(x => x.Value))
                {
                    yield return aimEvent;
                }        
            }
        }

        #endregion

        #region Implementation of Object

        public override string ToString()
        {
            return RepositoryName + "\\" + Marker;
        }

        #endregion
    }
}
