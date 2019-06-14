using Aran.Aim.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules
{
    public interface IFeatureProvider
    {
        Feature GetFeature(FeatureType featType, Guid identifier);
        int GetFeatureCount(FeatureType featType, Guid identifier);
        IEnumerable<Feature> GetList(List<FeatureType> typeFilter = null);
        IEnumerable<Tuple<FeatureType, Guid>> GetAllIdentifiers(List<FeatureType> typeFilter = null);
    }
}
