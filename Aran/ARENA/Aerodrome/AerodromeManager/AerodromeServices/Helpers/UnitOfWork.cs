using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.ServiceModel;
using Aran.Temporality.CommonUtil.Util;
using FluentNHibernate.Automapping;
using AerodromeServices.DataContract;
using AerodromeServices.Hibernate.Config;
using AerodromeServices.Logging;
using AerodromeServices.MappingOverrides;
using Microsoft.Win32;

namespace AerodromeServices.Helpers
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly ISessionFactory SessionFactory;
        private ITransaction _transaction;
        public static string FolderPath { get; private set; }    

        public ISession Session { get; }

        static UnitOfWork()
        {
            string connectionString = null;

            var registry = new ModifyRegistry
            {
                ShowError = false,
                BaseRegistryKey = Registry.CurrentUser,
                SubKey = Common.ConfigLocation
            };

            FolderPath = registry.Read(Common.FolderPathXmlKeyName);
            var connectString = registry.Read(Common.ConnectStrXmlKeyName);
            if (!string.IsNullOrWhiteSpace(connectString))
            {
                connectString = Crypto.DecryptStringAes(connectString, Common.CryptoPass);
                var parts = connectString.Split('|');
                if (parts.Length == 5)
                {
                    connectionString = $"Server={parts[0]};Port={parts[1]};" +
                                       $"User Id={parts[2]};Password={parts[3]};Database={parts[4]}";
                }
            }

            if (connectionString == null)
                throw new FaultException("ConnectionString is null");

            try
            {
                SessionFactory = Fluently.Configure()
                    .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString))
                    .Mappings(x => x.AutoMappings.Add(AutoMap
                        .AssemblyOf<IEntity>(new AutomappingConfiguration())
                        .UseOverridesFromAssemblyOf<AmdbUserOverride>()
                        .Conventions.Add(new CascadeConvention(),
                            new LazyLoadConvention(), new StringColumnLengthConvention(),
                            new EnumConvention())))
                    .BuildSessionFactory();
            }
            catch (Exception e)
            {
                LogManager.GetLogger(nameof(UnitOfWork)).Error(e, $"Constructor||{e.Message}");
                throw;
            }
        }

        public UnitOfWork()
        {
            Session = SessionFactory.OpenSession();
        }

        public void BeginTransaction()
        {
            _transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Commit();
            }
            catch
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();

                throw;
            }
        }

        public void Rollback()
        {
            if (_transaction != null && _transaction.IsActive)
                _transaction.Rollback();
        }
    }
}