using MvvmCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TossInstaller.ViewModel
{
    internal class ConfigNameViewModel : ViewModelBase
    {
        #region Constructors

        public ServerInstallerViewModel Owner { get; set; }

        #endregion

        #region Propertires

        private const int nameLength = 4;
        public bool IsSuccess = false;

        private string _configName = "";
        public string ConfigName
        {
            get => _configName;
            set
            {
                _configName = value;
                OnPropertyChanged("ConfigName");
            }
        }

        #endregion

        #region Commands

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(
                t =>
                {
                    if (ConfigName == null || ConfigName.Length < nameLength)
                    {
                        MessageBox.Show("Name lenth should be more then " + nameLength,
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                        return;
                    }

                    if (Owner.Configs.Any(x => x.ConfigName.ToUpper() == ConfigName.ToUpper()))
                    {
                        MessageBox.Show("Config with same name already exist",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                        return;
                    }

                    IsSuccess = true;
                    ((Window)t).Close();
                }
                ));

        private RelayCommand _escapeCommand;

        public RelayCommand EscapeCommand => _escapeCommand ?? (_escapeCommand = new RelayCommand(
                t =>
                {
                    ((Window)t).Close();
                }));

        #endregion
    }
}
