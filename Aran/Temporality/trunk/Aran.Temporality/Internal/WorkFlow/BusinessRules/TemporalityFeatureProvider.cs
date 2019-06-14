using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.BusinessRules;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.Routines;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    class TemporalityFeatureProvider : IFeatureProvider
    {
        public TemporalityFeatureProvider(RoutineContext context)
        {
            Context = context;
        }
        public RoutineContext Context { get; }

        public Feature GetFeature(FeatureType featType, Guid identifier)
        {
            return Context.LoadFeature(featType, identifier)?.Feature;
        }

        public int GetFeatureCount(FeatureType featType, Guid identifier)
        {
            return Context.LoadFeature(featType, identifier) != null ? 1 : 0;
        }

        public IEnumerable<Feature> GetList(List<FeatureType> typeFilter = null)
        {
            if (typeFilter == null)
                return Context.Load().Select(t => t.Feature).ToList();

            List<Feature> result = new List<Feature>();
            foreach (var featureType in typeFilter)
            {
                result.AddRange(Context.Load(featureType).Select(t=>t.Feature).ToList());
            }

            return result;
        }

        public IEnumerable<Tuple<FeatureType, Guid>> GetAllIdentifiers(List<FeatureType> typeFilter = null)
        {
            if (typeFilter == null)
                return Context.Load().Select(t => new Tuple<FeatureType, Guid>(t.FeatureType, t.Identifier)).ToList();

            List<Tuple<FeatureType, Guid>> result = new List<Tuple<FeatureType, Guid>>();
            foreach (var featureType in typeFilter)
            {
                result.AddRange(Context.Load(featureType).Select(t => new Tuple<FeatureType, Guid>(featureType ,t.Identifier)).ToList());
            }
            return result;
        }
    }
}
