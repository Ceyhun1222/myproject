using System;
using Aran.Aim.Data;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Internal.Abstract.Repository;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Convention;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Aran.Temporality.Internal.Implementation.Repository.NHibernate.Repository
{
    class PostgreSqlRepository<T> : AbstractNHibernateRepository<T> where T : class
    {

        protected static AutoPersistenceModel CreateAutomappings()
        {
            // This is the actual automapping - use AutoMap to start automapping,
            // then pick one of the static methods to specify what to map (in this case
            // all the classes in the assembly that contains Employee), and then either
            // use the Setup and Where methods to restrict that behaviour, or (preferably)
            // supply a configuration instance of your definition to control the automapper.
            return AutoMap.AssemblyOf<TemporalityLogicOptions>(new ContainerConfiguration()).
                Conventions.Add(new CascadeConvention(), new LazyLoadConvention(), new StringColumnLengthConvention(), new EnumConvention());
        }

        public PostgreSqlRepository(string path, string marker)
        {
            RepositoryName = path;
            Marker = marker;
            Open();
        }


        #region Overrides of AbstractNHibernateRepository<T>

        private static void BuildSchema(Configuration config, bool rewrite)
        {
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config).Create(false, rewrite);
        }

        protected override ISessionFactory CreateSessionFactory(bool rewrite)
        {
            var connectionString = $"Server={ConfigUtil.NoDataServiceAddress};Port={ConfigUtil.NoDataServicePort};" +
                                   $"User Id={ConfigUtil.NoDataUser};Password={ConfigUtil.NoDataPassword};Database={ConfigUtil.NoDataDatabase};";

            var model = CreateAutomappings();

            //create index
            //model = model.Override<StateMetaContainer>(t =>
            //{
            //    t.LazyLoad();

            //    t.Map(c => c.WorkPackage).Index("IDX_sts");
            //    t.Map(c => c.FeatureType).Index("IDX_sts");
            //    t.Map(c => c.Guid).Index("IDX_sts");
            //    t.Map(c => c.SequenceNumber).Index("IDX_sts");
            //    t.Map(c => c.CorrectionNumber).Index("IDX_sts");
            //    t.Map(c => c.BeginPosition).Index("IDX_sts");
            //    t.Map(c => c.EndPosition).Index("IDX_sts");

            //    t.Map(c => c.Data).LazyLoad();

            //}
            //);


            model = model.OverrideAll(p => p.IgnoreProperties(
                x => x.MemberInfo.GetCustomAttributes(typeof(NotPersisted), false).Length > 0));



            var sqlCfg = PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString);

            return Fluently.Configure().Database(sqlCfg)
                .Mappings(m => m.AutoMappings.Add(model))
                .ExposeConfiguration(t => BuildSchema(t, rewrite))
                .BuildSessionFactory();
        }

        public override T Get(int key, int featureTypeId, Projection projection = null)
        {
            throw new NotImplementedException();
        }

        public override void RemoveByKey(int key, int featureTypeId)
        {
            throw new NotImplementedException();
        }



        #endregion
    }
}
