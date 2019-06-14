using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Internal.Abstract.Repository;
using Aran.Temporality.Internal.Implementation.Repository.Linq;
using Aran.Temporality.Internal.MetaData.Offset;

namespace Aran.Temporality.Internal.Implementation.Repository
{
    internal class MongoWithBackupRepository : AbstractAimRepository<string>
    {
        #region Properties

        private readonly MongoRepository _mongoRepository;
        private readonly NoDeleteOffsetRepository<AimFeature> _noDeleteRepository;

        #endregion

        #region Constructors

        public MongoWithBackupRepository(string repositoryName, string marker, bool rewrite, bool createGeoIndex)
        {
            RepositoryName = repositoryName;
            Marker = marker.Replace("events\\", "");
            _mongoRepository = new MongoRepository(repositoryName, marker, rewrite, createGeoIndex);
            _noDeleteRepository = new NoDeleteOffsetRepository<AimFeature>(repositoryName, marker, rewrite);
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return _mongoRepository.ToString();
        }

        #endregion

        #region Implementation of AbstractDataRepository

        public override void Open(bool rewrite = false)
        {
        }

        public override void Close()
        {
            _mongoRepository.Close();
            _noDeleteRepository.Close();
        }

        public override string Add(AimFeature item)
        {
            _noDeleteRepository.Add(item);
            return _mongoRepository.Add(item);
        }

        public override AimFeature Get(string key, int featureTypeId, Projection projection = null)
        {
            return _mongoRepository.Get(key, featureTypeId, projection);
        }

        public override List<AimFeature> Get(IList<string> keys, int featureTypeId, Projection projection = null)
        {
            return _mongoRepository.Get(keys, featureTypeId, projection);
        }

        public override void Remove(AimFeature item)
        {
            _mongoRepository.Remove(item);
            _noDeleteRepository.Remove(item);
        }

        public override void RemoveByKey(string key, int featureTypeId)
        {
            _mongoRepository.RemoveByKey(key, featureTypeId);
        }

        public override void RemoveAll()
        {
            _mongoRepository.RemoveAll();
            _noDeleteRepository.RemoveAll();
        }

        #endregion

        #region Implementation of IDisposable

        public override void Dispose()
        {
            _mongoRepository.Dispose();
            _noDeleteRepository.Dispose();
        }

        #endregion

        #region Implementation of AbstractLinqDataRepository

        public override IEnumerable<AimFeature> Where(Func<AimFeature, bool> predicate)
        {
            return _mongoRepository.Where(predicate);
        }

        public override AimFeature Poke()
        {
            return _mongoRepository.Poke();
        }

        public override IEnumerator GetEnumerator()
        {
            return _mongoRepository.GetEnumerator();
        }

        #endregion

        #region Implementation of AbstractOffsetDataRepository

        public override IEnumerable GetMetaDatas()
        {
            return _mongoRepository.GetMetaDatas();
        }

        public override string Add(AimFeature item, OffsetEventMetaData<string> meta)
        {
            _noDeleteRepository.Add(item, new OffsetEventMetaData<long>(meta) { RelatedFeatures = meta.RelatedFeatures });
            return _mongoRepository.Add(item, meta);
        }

        public override List<Guid> GetFilteredId(Filter filter, int featureTypeId, IList<Guid> ids)
        {
            return _mongoRepository.GetFilteredId(filter, featureTypeId, ids);
        }

        #endregion

        #region Implementation of AbstractAimRepository

        public override AimEvent Get(int featureTypeId, Guid identifier, int sequenceNumber, int correctionNumber,
            Projection projection = null)
        {
            return _mongoRepository.Get(featureTypeId, identifier, sequenceNumber, correctionNumber, projection);
        }

        public override List<AimEvent> Get(int featureTypeId, Guid? identifier, Projection projection)
        {
            return _mongoRepository.Get(featureTypeId, identifier, projection);
        }

        public override AimEvent GetEvent(string key, int featureTypeId, Projection projection = null)
        {
            return _mongoRepository.GetEvent(key, featureTypeId, projection);
        }

        public override List<AimEvent> GetEvents(IList<string> keys, int featureTypeId, Projection projection = null)
        {
            return _mongoRepository.GetEvents(keys, featureTypeId, projection);
        }

        public override IEnumerable<AimEvent> GetEvents()
        {
            return _mongoRepository.GetEvents();
        }

        #endregion
    }
}
