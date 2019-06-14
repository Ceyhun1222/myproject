using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using Aran.Temporality.Common.Aim.Service;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Common.Util.TypeUtil;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using Microsoft.Win32;
using MvvmCore;
using System.Linq;

namespace ClientInstaller.ViewModel
{
    internal class ClientInstallerViewModel : ViewModelBase
    {
        public ClientInstallerViewModel()
        {
            try
            {
                var clientConfig = new ClientConfig();
                clientConfig.Load();
                Configs = new ObservableCollection<ClientConfigObject>(clientConfig.Settings.Values);
            }
            catch
            {
                MessageBox.Show("Config file is missing. Fields were filled with default values. " +
                                "Please, Apply your changes to create config file.",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Information);

                Configs = new ObservableCollection<ClientConfigObject>();
                Configs.Add(new ClientConfigObject
                {
                    ConfigName = "Default",
                    UserId = 1
                });
            }
        }

        #region Properties

        private ClientConfigObject _currentConfig = new ClientConfigObject();
        public ClientConfigObject CurrentConfig
        {
            get => _currentConfig ?? (_currentConfig = new ClientConfigObject());
            set
            {
                _currentConfig = value;
                OnPropertyChanged("CurrentConfig");
                OnPropertyChanged("ConfigName");
                OnPropertyChanged("TossServerAddress");
                OnPropertyChanged("License");
                OnPropertyChanged("Storage");
                OnPropertyChanged("UserId");
                OnPropertyChanged("ServicePort");
                OnPropertyChanged("HelperPort");
                OnPropertyChanged("LicenseIndex");
                OnPropertyChanged("UseWebApiForMetadata");
                OnPropertyChanged("WebApiAddress");
            }
        }

        private ObservableCollection<ClientConfigObject> _configs = new ObservableCollection<ClientConfigObject>();
        public ObservableCollection<ClientConfigObject> Configs
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

        public string TossServerAddress
        {
            get
            {
                return CurrentConfig.GetServiceAddress();
            } 
            set
            {
                CurrentConfig.ServiceAddress = value + ":" + CurrentConfig.ServicePort;
                CurrentConfig.HelperAddress = value + ":" + CurrentConfig.HelperPort;
            }
        }

        public string ServicePort
        {
            get => CurrentConfig.ServicePort;
            set
            {
                CurrentConfig.ServicePort = value;
                CurrentConfig.ServiceAddress = CurrentConfig.GetServiceAddress() + ":" + value;
                OnPropertyChanged("ServicePort");
            }
        }

        public string HelperPort
        {
            get => CurrentConfig.HelperPort;
            set
            {
                CurrentConfig.HelperPort = value;
                CurrentConfig.HelperAddress = CurrentConfig.GetServiceAddress() + ":" + value;
                OnPropertyChanged("HelperPort");
            }
        }
        
        public int UserId
        {
            get => CurrentConfig.UserId;
            set
            {
                CurrentConfig.UserId = value;
                OnPropertyChanged("UserId");
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

        public string Storage
        {
            get => CurrentConfig.StorageName;
            set
            {
                CurrentConfig.StorageName = value;
                OnPropertyChanged("Storage");
            }
        }

        public string UseWebApiForMetadata
        {
            get => CurrentConfig.UseWebApiForMetadata ? "Yes" : "No";
            set
            {
                CurrentConfig.UseWebApiForMetadata = value == "Yes";
                OnPropertyChanged("UseWebApiForMetadata");
            }
        }

        public string WebApiAddress
        {
            get => CurrentConfig.WebApiAddress;
            set
            {
                CurrentConfig.WebApiAddress = value;
                OnPropertyChanged("WebApiAddress");
            }
        }

        private string _newSettingName;
        public string NewSettingName
        {
            get => _newSettingName;
            set
            {
                _newSettingName = value;
                OnPropertyChanged("NewSettingName");
            }
        }

        public ObservableCollection<string> AllSystemTypes
        {
            get
            {
                ObservableCollection<string> result = new ObservableCollection<string>();
                string[] names = Enum.GetNames(typeof(SystemType));
                foreach (var name in names)
                {
                    result.Add(name);
                }
                return result;
            }
        }

        public List<string> AllLicenses => Enum.GetNames(typeof(EsriLicense)).ToList();

        #endregion

        #region Commands

        private RelayCommand _addNewCommand;
        public RelayCommand AddNewCommand => _addNewCommand ?? (_addNewCommand = new RelayCommand(AddNewAction));

        private RelayCommand _setAsDefault;
        public RelayCommand SetAsDefault => _setAsDefault ?? (_setAsDefault = new RelayCommand(SetAsDefaultAction));

        private RelayCommand _deleteSelectedCommand;
        public RelayCommand DeleteSelectedCommand => _deleteSelectedCommand ?? (_deleteSelectedCommand = new RelayCommand(DeleteAction));
        
        private RelayCommand _applyCommand;
        public RelayCommand ApplyCommand => _applyCommand ?? (_applyCommand = new RelayCommand(ApplyAction));
        
        private RelayCommand _selectUserCommand;
        public RelayCommand SelectUserCommand => _selectUserCommand ?? (_selectUserCommand = new RelayCommand(SelectUserAction));

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
                CurrentConfig = new ClientConfigObject { ConfigName = configNameViewModel.ConfigName };
                Configs.Add(CurrentConfig);
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

        public void DeleteAction(object t)
        {
            var result = MessageBox.Show("Do you really want to delete selected config?",
                        "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            if (result != MessageBoxResult.Yes)
                return;

            Configs.Remove(CurrentConfig);
            if (Configs.Count == 0)
                Configs.Add(new ClientConfigObject { ConfigName = "Default" });
            CurrentConfig = Configs.First();
        }

        public void ApplyAction(object t)
        {
            var config = new ClientConfig();
            foreach (var configObject in Configs)
            {
                config.Settings.Add(configObject.ConfigName, configObject);
            }
            config.Save();

            MessageBox.Show("Config file saved successfully",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
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
                    var config = new ClientConfig();
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

        private void SelectUserAction(object t)
        {
            if (TossServerAddress == null)
            {
                MessageBox.Show("Incorrect server address", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Storage == null)
            {
                MessageBox.Show("Incorrect storage name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (TossServerAddress != "localhost" && TossServerAddress != "127.0.0.1")
                {
                    MessageBox.Show("Only for local server (localhost or 127.0.0.1)", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                ConnectionProvider.InitServerSettings();
                IList<User> users = null;
                using (var temporalityService = AimServiceFactory.OpenLocal(Storage))
                {
                    if (!(temporalityService is INoAixmDataService noData))
                    {
                        MessageBox.Show("Error connecting to server", "Error", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }

                    users = noData.GetAllUsers();
                }

                if (users == null || users.Count <= 0)
                {
                    MessageBox.Show("Error connecting to server", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var usersListWindow = new UsersListWindow();
                var usersListViewModel = new UsersListViewModel(users, UserId);
                usersListWindow.DataContext = usersListViewModel;
                usersListWindow.ShowDialog();
                if (usersListViewModel.IsSuccess)
                    UserId = usersListViewModel.SelectedUserId;
            }
            catch
            {
                MessageBox.Show("Error connecting to server", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

    }
}
