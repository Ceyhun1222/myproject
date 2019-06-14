using System.IO;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Internal.Abstract.Repository;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config;
using FluentNHibernate.Automapping;
using NHibernate;
using NHibernate.Cfg;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;

namespace Aran.Temporality.Internal.Implementation.Repository.NHibernate
{
    class SqliteRepository<T> : AbstractNHibernateRepository<T> where T : class
    {
        static protected AutoPersistenceModel CreateAutomappings()
        {
            // This is the actual automapping - use AutoMap to start automapping,
            // then pick one of the static methods to specify what to map (in this case
            // all the classes in the assembly that contains Employee), and then either
            // use the Setup and Where methods to restrict that behaviour, or (preferably)
            // supply a configuration instance of your definition to control the automapper.
            return AutoMap.AssemblyOf<TemporalityLogicOptions>(new ContainerConfiguration()).Conventions.Add<CascadeConvention>();
        }

        public SqliteRepository(string path, string marker)
        {
            RepositoryName = path;
            Marker = marker;
            Open();
        }

        public override void Remove(T item)
        {
            throw new System.NotImplementedException();
        }

        private void BuildSchema(Configuration config, bool rewrite)
        {
            if (File.Exists(FileName) && rewrite)
            {
                File.Delete(FileName);
            }

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config).Create(false, rewrite);
        }

        protected override ISessionFactory CreateSessionFactory(bool rewrite)
        {
            var model = CreateAutomappings();
            model.
                OverrideAll(p => p.IgnoreProperties(
                x => x.PropertyType.GetCustomAttributes(typeof(NotPersisted), false).Length > 0));

            var sqlCfg = SQLiteConfiguration.Standard.UsingFile(FileName);

            return Fluently.Configure().Database(sqlCfg)
                .Mappings(m => m.AutoMappings.Add(model))
                .ExposeConfiguration(t=>BuildSchema(t,rewrite))
                .BuildSessionFactory();
        }

       
    }
}
