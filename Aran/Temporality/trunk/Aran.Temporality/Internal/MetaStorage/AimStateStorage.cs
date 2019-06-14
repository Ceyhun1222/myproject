#region

using System;
using Aran.Aim;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Internal.Implementation.Storage.Final;

#endregion

namespace Aran.Temporality.Internal.MetaStorage
{
    internal class AimStateStorage : StateStorage<AimFeature>
    {
        #region Overrides of StateStorage<Feature>

        public override AbstractState<AimFeature> GetStateFromData(AimFeature data, AbstractStateMetaData meta)
        {
            if (meta == null)
            {
                return GetByDefaultData(meta);
            }

            return new AimState(meta)
                       {
                           Data = data
                       };
        }


        public override AbstractState<AimFeature> GetByDefaultData(AbstractStateMetaData meta)
        {
            var featureType = (FeatureType) Enum.ToObject(typeof (FeatureType), meta.FeatureTypeId);
            var feature = AimObjectFactory.CreateFeature(featureType);

            if (meta.Guid != null) feature.Identifier = (Guid) meta.Guid;

            return new AimState(meta)
                       {
                           Data = new AimFeature { Feature = feature }
                       };
        }

       

        #endregion
    }
}