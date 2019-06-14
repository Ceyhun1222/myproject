using Aran.Temporality.Common.Entity;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class ConfigurationStorage : CrudStorageTemplate<Configuration>, IConfigurationStorage
    {
        public Configuration GetConfigurationByName(string name)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                return (Configuration)session.CreateCriteria(typeof(Configuration)).
                                               Add(Restrictions.Eq("Name", name)).
                                               UniqueResult();
            }
        }

        public int UpdateConfiguration(Configuration configuration)
        {
            if (configuration == null) return -1;

            int id;
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var old = session.Get<Configuration>(configuration.Id);

                    if (old != null)
                    {
                        old.Data = configuration.Data;
                        old.Type = configuration.Type;
                        old.Name = configuration.Name;
                        session.Update(old);
                        id = old.Id;
                    }
                    else
                    {
                        id = (int)session.Save(configuration);
                    }

                    transaction.Commit();
                }
            }

            return id;
        }
    }
}
