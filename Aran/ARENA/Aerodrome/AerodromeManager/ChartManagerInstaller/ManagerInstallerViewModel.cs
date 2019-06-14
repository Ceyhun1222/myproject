using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using Aran.Temporality.CommonUtil.Util;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;

using AerodromeServices.Helpers;
using AerodromeServices.Hibernate.Config;
using AerodromeServices.MappingOverrides;
using AerodromeServices.DataContract;
using AerodromeServices.Service;
using AerodromeServices;
using AerodromeServices.Repositories;

namespace ChartManagerInstaller
{
    internal class ManagerInstallerViewModel : ViewModelBase
    {
        private RelayCommand _rebuildCommand;
        private string _noDataDatabase = "Amdb";
        private string _noDataPassword = "admin";
        private string _noDataUser = "postgres";
        private string _noDataServicePort = "5432";
        private string _noDataServiceAddress = "localhost";
        private string _addressTcp = "localhost";
        private string _addressHttp = "localhost";
        private string _folderPath;

        public ManagerInstallerViewModel()
        {
            Init();
        }

        private void Init()
        {
            var registry = new ModifyRegistry
            {
                ShowError = false,
                BaseRegistryKey = Registry.CurrentUser,
                SubKey = Common.ConfigLocation
            };


            var connectString = registry.Read(Common.ConnectStrXmlKeyName);
            if (!string.IsNullOrWhiteSpace(connectString))
            {
                connectString = Crypto.DecryptStringAes(connectString, Common.CryptoPass);
                var parts = connectString.Split('|');
                if (parts.Length == 5)
                {
                    NoDataServiceAddress = parts[0];
                    NoDataServicePort = parts[1];
                    NoDataUser = parts[2];
                    NoDataPassword = parts[3];
                    NoDataDatabase = parts[4];
                }
            }

            FolderPath = @"C:\AmdbManager";
        }

        private string GetConnectStr()
        {
            return Crypto.EncryptStringAes(NoDataServiceAddress + "|" +
                                           NoDataServicePort + "|" +
                                           NoDataUser + "|" +
                                           NoDataPassword + "|" +
                                           NoDataDatabase, Common.CryptoPass);
        }

        public string NoDataDatabase
        {
            get => _noDataDatabase;
            set
            {
                _noDataDatabase = value;
                OnPropertyChanged(nameof(NoDataDatabase));
            }
        }

        public string NoDataPassword
        {
            get => _noDataPassword;
            set
            {
                _noDataPassword = value;
                OnPropertyChanged(nameof(NoDataPassword));
            }
        }

        public string NoDataUser
        {
            get => _noDataUser;
            set
            {
                _noDataUser = value;
                OnPropertyChanged(nameof(NoDataUser));
            }
        }

        public string NoDataServicePort
        {
            get => _noDataServicePort;
            set
            {
                _noDataServicePort = value;
                OnPropertyChanged(nameof(NoDataServicePort));
            }
        }

        public string NoDataServiceAddress
        {
            get => _noDataServiceAddress;
            set
            {
                _noDataServiceAddress = value;
                OnPropertyChanged(nameof(NoDataServiceAddress));
            }
        }

        public string AddressTCP
        {
            get => _addressTcp;
            set
            {
                _addressTcp = value;
                OnPropertyChanged(nameof(AddressTCP));
            }
        }

        public string AddressHTTP
        {
            get => _addressHttp;
            set
            {
                _addressHttp = value;
                OnPropertyChanged(nameof(AddressHTTP));
            }
        }

        public string FolderPath { get => _folderPath;
            set
            {
                _folderPath = value;
                OnPropertyChanged(nameof(FolderPath));
            }
        }

        public RelayCommand RebuildCommand
        {
            get
            {
                return _rebuildCommand ?? (_rebuildCommand = new RelayCommand(
                           t =>
                           {

                               try
                               {
                                   var registry = new ModifyRegistry
                                   {
                                       ShowError = false,
                                       BaseRegistryKey = Registry.CurrentUser,
                                       SubKey = Common.ConfigLocation
                                   };

                                   registry.Write(Common.ConnectStrXmlKeyName, GetConnectStr());
                                   registry.Write(Common.FolderPathXmlKeyName, FolderPath);
                                   Directory.CreateDirectory(FolderPath);
                                   //registry.Write("AddressTCP", AddressTCP);
                                   //registry.Write("AddressHTTP", AddressHTTP);

                                   var server = NoDataServiceAddress;
                                   var port = NoDataServicePort;
                                   var username = NoDataUser;
                                   var password = NoDataPassword;
                                   var db = NoDataDatabase;

                                   var connectionString = $"Server={server};Port={port};" +
                                                          $"User Id={username};Password={password};Database={db}";
                                   try
                                   {
                                       Fluently.Configure()
                                           .Database(
                                               PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString))
                                           .Mappings(x => x.AutoMappings.Add(AutoMap
                                               .AssemblyOf<IEntity>(new AutomappingConfiguration())                                               
                                               .UseOverridesFromAssemblyOf<AmdbUserOverride>()
                                               .Conventions.Add(new CascadeConvention(),
                                                   new LazyLoadConvention(), new StringColumnLengthConvention(),
                                                   new EnumConvention())))
                                           .ExposeConfiguration(
                                               config => new SchemaUpdate(config)
                                                   .Execute(false, true)) //SchemaExport(config).Create(false, false))
                                           .BuildSessionFactory();
                                       AmdbManagerService service = new AmdbManagerService();

                                       User admin = new User()
                                       {
                                           FirstName = "Admin",
                                           LastName = "Admin",
                                           UserName = "admin",
                                           Password = HelperMethods.Sha256_hash("admin"),
                                           IsAdmin = true,
                                           Privilege = UserPrivilege.Full,
                                       };
                                       RepositoryContext dbContext = new RepositoryContext();
                                       dbContext.Repository<User>().Create(admin);

                                   }
                                   catch (Exception e)
                                   {
                                       Console.WriteLine(e);
                                       Console.WriteLine(e.InnerException);
                                       throw;
                                   }

                                   MessageBox.Show("Tables are recreated.", "Success", MessageBoxButton.OK,
                                       MessageBoxImage.Information);

                               }
                               catch (Exception exception)
                               {
                                   Console.WriteLine(exception.Message);
                                   MessageBox.Show(
                                       exception.Message +
                                       (exception.InnerException != null
                                           ? "\nInner exception: " + exception.InnerException.Message
                                           : ""), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                               }

                           }));
            }
        }
    }
}