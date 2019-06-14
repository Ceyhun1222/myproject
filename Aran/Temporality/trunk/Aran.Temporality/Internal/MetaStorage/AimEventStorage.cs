#region

using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim.Data.Filters;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.Implementation.Storage.Final;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Consistency;
using Aran.Temporality.Common.Util;

#endregion

namespace Aran.Temporality.Internal.MetaStorage
{
#if SQL_DB

     internal class AimEventStorage : SqlEventStorage<AimFeature>
    {
    #region Overrides of EventStorage<Feature>

        public override AbstractEvent<AimFeature> GetEventFromData(AimFeature data, AbstractEventMetaData meta)
        {
            return new AimEvent(meta)
            {
                Data = data,
            };
        }


    #endregion

    }
    

#else


    internal class AimEventStorage<TKeyType> : EventStorage<AimFeature, TKeyType>
    {
        #region Properties

        private AbstractConsistencyRepository<AbstractEvent<AimFeature>> ConsistencRepository { get; set; }

        #endregion

        #region Overrides of EventStorage<Feature>

        public override AbstractEvent<AimFeature> GetEventFromData(AimFeature data, AbstractEventMetaData meta)
        {
            return new AimEvent(meta)
            {
                Data = data
            };
        }

        public override IList<Guid> GetFilteredIds(IFeatureId featureId, Filter filter, bool slotOnly, IList<Guid> ids = null)
        {
            var result = Repository(featureId.WorkPackage).GetFilteredId(filter, featureId.FeatureTypeId, ids);

            if (featureId.WorkPackage != 0 && !slotOnly)
                result = result.Union(Repository(0).GetFilteredId(filter, featureId.FeatureTypeId, ids)).ToList();

            return result;
        }

        protected override void SaveDataConsistency(AbstractEvent<AimFeature> value)
        {
            ConsistencRepository.Add(value);
        }

        public override void Load()
        {
            ConsistencRepository = new EventConsistencyRepository(ConfigUtil.RepositoryType, StorageName);
            base.Load();
        }

        public override bool CheckFilterRuntime(AimFeature abstractStateData, Filter filter)
        {
            if (filter == null)
                return true;

            if (abstractStateData?.Feature == null)
                return false;

            return abstractStateData.Feature.IsFilterOk(filter);
        }

        #endregion

    }

#endif
}