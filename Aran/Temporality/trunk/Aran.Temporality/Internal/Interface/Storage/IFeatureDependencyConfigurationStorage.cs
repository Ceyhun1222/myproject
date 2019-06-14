using System.Collections.Generic;
using Aran.Temporality.Common.Entity;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IFeatureDependencyConfigurationStorage : ICrudStorage<FeatureDependencyConfiguration>
    {
        bool UpdateFeatureDependencyConfiguration(FeatureDependencyConfiguration entity);
        void DeleteFeatureDependenciesByTemplateId(int id);
        IList<FeatureDependencyConfiguration> GetFeatureDependenciesByTemplate(int templateId);
    }
}
