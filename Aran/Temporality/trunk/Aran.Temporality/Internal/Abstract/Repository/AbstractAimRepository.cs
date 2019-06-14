using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.MetaData;
using Aran.Temporality.Internal.MetaData.Offset;

namespace Aran.Temporality.Internal.Abstract.Repository
{
    internal abstract class AbstractAimRepository<TKeyType> : AbstractOffsetDataRepository<AimFeature, TKeyType>
    {
        public abstract AimEvent Get(int featureTypeId, Guid identifier, int sequenceNumber, int correctionNumber, Projection projection = null);

        public abstract List<AimEvent> Get(int featureTypeId, Guid? identifier, Projection projection);

        public abstract AimEvent GetEvent(string key, int featureTypeId, Projection projection = null);

        public abstract List<AimEvent> GetEvents(IList<string> keys, int featureTypeId, Projection projection = null);

        public abstract IEnumerable<AimEvent> GetEvents();

        public List<AimEvent> Get(int featureTypeId, Guid identifier, Projection projection)
        {
            return Get(featureTypeId, new Guid?(identifier), projection);
        }

        public List<AimEvent> Get(IFeatureId featureId, Projection projection)
        {
            return Get(featureId.FeatureTypeId, featureId.Guid, projection);
        }

        public AimEvent Get(IFeatureId featureId, int sequenceNumber, int correctionNumber, Projection projection = null)
        {
            return featureId?.Guid == null ? null : Get(featureId.FeatureTypeId, featureId.Guid.Value, sequenceNumber, correctionNumber, projection);
        }

    }
}
