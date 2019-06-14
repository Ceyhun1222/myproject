using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Aran.Temporality.Common.Aim.Service;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Common.Util.TypeUtil;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using FluentNHibernate.Cfg.Db;
using Microsoft.Win32;
using MvvmCore;

namespace TossInstaller.ViewModel
{
    internal class ServerInstallerViewModel : ViewModelBase
    {

        #region ctor

        public ServerInstallerViewModel()
        {
            try
            {
                var serverConfig = new ServerConfig();
                serverConfig.Load();
                Configs = new ObservableCollection<ServerConfigObject>(serverConfig.Settings.Values);

                if (Configs.Count == 0)
                    Configs.Add(new ServerConfigObject());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Config file is missing. Fields were filled with default values. " +
                                "Please, Apply your changes to create config file.\n" + ex.Message,
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Information);

                if (Configs.Count == 0)
                    Configs.Add(new ServerConfigObject());
            }
        }

        #endregion

        #region Properties


        private ServerConfigObject _currentConfig = new ServerConfigObject();
        public ServerConfigObject CurrentConfig
        {
            get => _currentConfig ?? (_currentConfig = new ServerConfigObject());
            set
            {
                _currentConfig = value;
                OnPropertyChanged("CurrentConfig");
                OnPropertyChanged("ConfigName");
                OnPropertyChanged("NoDataServiceAddress");
                OnPropertyChanged("NoDataDatabase");
                OnPropertyChanged("NoDataPassword");
                OnPropertyChanged("NoDataUser");
                OnPropertyChanged("NoDataServicePort");
                OnPropertyChanged("DllRepo");
                OnPropertyChanged("License");
                OnPropertyChanged("LicenseIndex");
                OnPropertyChanged("ServicePort");
                OnPropertyChanged("HelperPort");
                OnPropertyChanged("ExternalPort");
                OnPropertyChanged("RepositoryType");
                OnPropertyChanged("RepositoryTypeIndex");
                OnPropertyChanged("MongoServerAddress");
                OnPropertyChanged("MongoServerPort");
                OnPropertyChanged("MongoUser");
                OnPropertyChanged("MongoPassword");
                OnPropertyChanged("MongoCreateGeoIndex");
                OnPropertyChanged("UseRedisForMetaCache");
                OnPropertyChanged("RedisConnectionString");
            }
        }

        private ObservableCollection<ServerConfigObject> _configs = new ObservableCollection<ServerConfigObject>();
        public ObservableCollection<ServerConfigObject> Configs
        {
            get => _configs;
            set
            {
                _configs = value;
                OnPropertyChanged("Configs");
            }
        }

        public string ConfigName
        {
            get => CurrentConfig.ConfigName;
            set
            {
                CurrentConfig.ConfigName = value;
                OnPropertyChanged("ConfigName");
            }
        }

        public List<string> AllSystemTypes => Enum.GetNames(typeof(SystemType)).ToList();

        public List<string> AllLicenses => Enum.GetNames(typeof(EsriLicense)).ToList();

        public List<string> RepositoryTypes => Enum.GetNames(typeof(RepositoryType)).ToList();

        public string NoDataServiceAddress
        {
            get => CurrentConfig.DbAddress;
            set
            {
                CurrentConfig.DbAddress = value;
                OnPropertyChanged("NoDataServiceAddress");
            }
        }

        public string NoDataDatabase
        {
            get => CurrentConfig.DbName;
            set
            {
                CurrentConfig.DbName = value;
                OnPropertyChanged("NoDataDatabase");
            }
        }

        public string NoDataPassword
        {
            get => CurrentConfig.DbPassword;
            set
            {
                CurrentConfig.DbPassword = value;
                OnPropertyChanged("NoDataPassword");
            }
        }

        public string NoDataUser
        {
            get => CurrentConfig.DbUser;
            set
            {
                CurrentConfig.DbUser = value;
                OnPropertyChanged("NoDataUser");
            }
        }

        public short NoDataServicePort
        {
            get => CurrentConfig.DbPort;
            set
            {
                CurrentConfig.DbPort = value;
                OnPropertyChanged("NoDataServicePort");
            }
        }

        public string DllRepo
        {
            get => CurrentConfig.DllRepo;
            set
            {
                CurrentConfig.DllRepo = value;
                OnPropertyChanged("DllRepo");
            }
        }

        public EsriLicense License
        {
            get => CurrentConfig.License;
            set
            {
                CurrentConfig.License = value;
                OnPropertyChanged("License");
            }
        }

        public int LicenseIndex
        {
            get => (int)CurrentConfig.License;
            set
            {
            }
        }

        public RepositoryType RepositoryType
        {
            get => CurrentConfig.RepositoryType;
            set
            {
                CurrentConfig.RepositoryType = value;
                OnPropertyChanged("RepositoryType");
            }
        }

        public int RepositoryTypeIndex
        {
            get => (int)CurrentConfig.RepositoryType;
            set
            {
            }
        }

        public string MongoServerAddress
        {
            get => CurrentConfig.MongoServerAddress;
            set
            {
                CurrentConfig.MongoServerAddress = value;
                OnPropertyChanged("MongoServerAddress");
            }
        }

        public int MongoServerPort
        {
            get => CurrentConfig.MongoServerPort;
            set
            {
                CurrentConfig.MongoServerPort = value;
                OnPropertyChanged("MongoServerPort");
            }
        }

        public string MongoUser
        {
            get => CurrentConfig.MongoUser;
            set
            {
                CurrentConfig.MongoUser = value;
                OnPropertyChanged("MongoUser");
            }
        }

        public string MongoPassword
        {
            get => CurrentConfig.MongoPassword;
            set
            {
                CurrentConfig.MongoPassword = value;
                OnPropertyChanged("MongoPassword");
            }
        }

        public string RedisConnectionString
        {
            get => CurrentConfig.RedisConnectionString;
            set
            {
                CurrentConfig.RedisConnectionString = value;
                OnPropertyChanged("RedisConnectionString");
            }
        }

        public string UseRedisForMetaCache
        {
            get => CurrentConfig.UseRedisForMetaCache ? "Yes" : "No";
            set
            {
                CurrentConfig.UseRedisForMetaCache = value == "Yes";
                OnPropertyChanged("UseRedisForMetaCache");
            }
        }

        public string MongoCreateGeoIndex
        {
            get => CurrentConfig.MongoCreateGeoIndex ? "Yes" : "No";
            set
            {
                CurrentConfig.MongoCreateGeoIndex = value == "Yes";
                OnPropertyChanged("MongoCreateGeoIndex");
            }
        }

        public int ServicePort
        {
            get => CurrentConfig.ServicePort;
            set
            {
                CurrentConfig.ServicePort = value;
                OnPropertyChanged("ServicePort");
            }
        }

        public int HelperPort
        {
            get => CurrentConfig.HelperPort;
            set
            {
                CurrentConfig.HelperPort = value;
                OnPropertyChanged("HelperPort");
            }
        }

        public int ExternalPort
        {
            get => CurrentConfig.ExternalPort;
            set
            {
                CurrentConfig.ExternalPort = value;
                OnPropertyChanged("ExternalPort");
            }
        }

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel => _blockerModel ?? (_blockerModel = new BlockerModel());

        #endregion

        #region Commands

        private RelayCommand _addNewCommand;
        public RelayCommand AddNewCommand => _addNewCommand ?? (_addNewCommand = new RelayCommand(AddNewAction));

        private RelayCommand _setAsDefault;
        public RelayCommand SetAsDefault => _setAsDefault ?? (_setAsDefault = new RelayCommand(SetAsDefaultAction));

        private RelayCommand _applyCommand;
        public RelayCommand ApplyCommand => _applyCommand ?? (_applyCommand = new RelayCommand(ApplyAction));

        private RelayCommand _deleteSelectedCommand;
        public RelayCommand DeleteSelectedCommand => _deleteSelectedCommand ?? (_deleteSelectedCommand = new RelayCommand(DeleteAction));

        private RelayCommand _rebuildCommand;
        public RelayCommand RebuildCommand => _rebuildCommand ?? (_rebuildCommand = new RelayCommand(RebuildAction));

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(SaveAction));

        public void AddNewAction(object t)
        {
            var configNameWindow = new ConfigNameWindow();
            if (!(configNameWindow.DataContext is ConfigNameViewModel configNameViewModel))
            {
                configNameViewModel = new ConfigNameViewModel();
                configNameWindow.DataContext = configNameViewModel;
            }

            configNameViewModel.Owner = this;

            configNameWindow.ShowDialog();

            if (configNameViewModel.IsSuccess)
            {
                CurrentConfig = new ServerConfigObject { ConfigName = configNameViewModel.ConfigName };
                Configs.Add(CurrentConfig);
            }
        }

        private void ApplyAction(object t)
        {
            var serverConfig = new ServerConfig();
            foreach (var configObject in Configs)
            {
                serverConfig.Settings.Add(configObject.ConfigName, configObject);
            }
            serverConfig.Save();

            MessageBox.Show("Config file saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }

        private void SaveAction(object t)
        {
            var saveFileDialog = new SaveFileDialog
            {
                FileName = "config",
                DefaultExt = ".xml",
                Filter = "XML files (.xml)|*.xml",
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    var config = new ServerConfig();
                    foreach (var configObject in Configs)
                    {
                        config.Settings.Add(configObject.ConfigName, configObject);
                    }
                    config.Save(saveFileDialog.FileName);

                    MessageBox.Show("Config file saved successfully",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
                catch (Exception)
                {
                    MessageBox.Show("File can not be created. " +
                                    "May be it is in use by another process " +
                                    "or you do not have permission to create " +
                                    "files in specified folder.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void SetAsDefaultAction(object t)
        {
            if (Configs.Count < 2)
                return;

            var defaultId = Configs.IndexOf(CurrentConfig);

            if (defaultId == 0)
                return;

            var current = Configs[defaultId];
            Configs.RemoveAt(defaultId);
            Configs.Insert(0, current);
            CurrentConfig = Configs.First();
            OnPropertyChanged("Configs");
        }

        private async void RebuildAction(object t)
        {
            await BlockerModel.BlockForAction(() =>
            {
                var connectionString = $"Server={CurrentConfig.DbAddress};Port={CurrentConfig.DbPort};" +
                                       $"User Id={CurrentConfig.DbUser};Password={CurrentConfig.DbPassword};Database=postgres;";

                var sqlCfg = PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString);
                var session = FluentNHibernate.Cfg.Fluently.Configure().Database(sqlCfg).BuildSessionFactory().OpenSession();
                var dbNamess = session.CreateSQLQuery("SELECT datname FROM pg_database;").List<string>();

                if (dbNamess.Contains(CurrentConfig.DbName))
                {
                    MessageBox.Show($"Database \"{CurrentConfig.DbName}\" exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                session.CreateSQLQuery($"CREATE DATABASE \"{CurrentConfig.DbName}\";").ExecuteUpdate();
                session.Close();

                if (MessageBoxResult.Yes != MessageBox.Show($"Are you sure you want to create the Postgres database \"{CurrentConfig.DbName}\"?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    return;

                CurrentConfig.DecryptDbConnection();
                ConnectionProvider.ApplyServerSettings(CurrentConfig);

                TypeUtil.CurrentTypeUtil = new AimTypeUtil();
                AimServiceFactory.Setup();
                try
                {
                    using (var temporalityService = AimServiceFactory.OpenLocal(CurrentConfig.DbName))
                    {
                        if (!(temporalityService is INoAixmDataService noData)) return;

                        var userId = noData.CreateUser("Admin");
                        noData.SetUserRole(userId, 68);
                        noData.UpdateConfiguration(new Configuration
                        {
                            Name = ConfigurationName.ChartingResolutionConfiguration,
                            Type = (int)ConfigurationType.PrecisionConfiguration
                        });
                        noData.UpdateConfiguration(new Configuration
                        {
                            Name = ConfigurationName.PublicationResolutionConfiguration,
                            Type = (int)ConfigurationType.PrecisionConfiguration
                        });

                        MessageBox.Show("Tables are recreated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message + (exception.InnerException != null
                                        ? "\nInner exception: " + exception.InnerException.Message
                                        : ""), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void DeleteAction(object t)
        {
            var result = MessageBox.Show("Do you really want to delete selected config?",
                        "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            if (result != MessageBoxResult.Yes)
                return;

            Configs.Remove(CurrentConfig);
            if (Configs.Count == 0)
                Configs.Add(new ServerConfigObject { ConfigName = "Default" });
            CurrentConfig = Configs.First();
        }

        #endregion
    }
}