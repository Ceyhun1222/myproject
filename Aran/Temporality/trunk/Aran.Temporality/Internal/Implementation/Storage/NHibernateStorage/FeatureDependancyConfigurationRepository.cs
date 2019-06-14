using System.Collections.Generic;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class FeatureDependencyConfigurationStorage : CrudStorageTemplate<FeatureDependencyConfiguration>, IFeatureDependencyConfigurationStorage
    {
        public bool UpdateFeatureDependencyConfiguration(FeatureDependencyConfiguration entity)
        {
            if (entity == null) return false;
            if (entity.Id == 0) return false;

            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var config = session.Get<FeatureDependencyConfiguration>(entity.Id);

                    if (config != null)
                    {
                        //do update
                        config.Name = entity.Name;
                        config.RootFeatureType = entity.RootFeatureType;
                        config.Data = entity.Data;

                        session.Update(config);
                        transaction.Commit();
                    }
                }
            }

            return true;
        }

        public IList<FeatureDependencyConfiguration> GetFeatureDependenciesByTemplate(int templateId)
        {
            if (templateId == 0) return null;

            using (var session = Repository.SessionFactory.OpenSession())
            {
                return session.CreateCriteria(typeof(FeatureDependencyConfiguration)).
                   Add(Restrictions.Eq("DataSourceTemplate", new DataSourceTemplate { Id = templateId })).
                   List<FeatureDependencyConfiguration>();
            }
        }

        public void DeleteFeatureDependenciesByTemplateId(int id)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Delete("from FeatureDependencyConfiguration where DataSourceTemplate.Id = " + id);
                        transaction.Commit();
                    }
                    catch
                    {
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
    }
}
