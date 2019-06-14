using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.View;
using MvvmCore;

namespace Aran.Temporality.CommonUtil.ViewModel
{
    public class ConfigSelectModel : ViewModelBase
    {

        #region Properties

        public Action<Window> CloseAction { get; set; }
        public Func<string, Window, bool> SelectAction { get; set; }
        public bool IsSuccess = false;

        public ObservableCollection<string> Settings { get; set; } = new ObservableCollection<string>();
        private string _setting;

        public string Setting
        {
            get => _setting;
            set
            {
                _setting = value;
                OnPropertyChanged("Setting");
            }
        }

        #endregion

        #region Commands

        private RelayCommand _closeCommand;
        public RelayCommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RelayCommand(
                    t =>
                    {
                        var window = (ConfigSelectWindow)t;
                        window.SettingComboBox.SelectedItem = null;

                        if (CloseAction != null)
                        {
                            CloseAction(window);
                        }
                        else
                        {
                            window.Close();
                        }
                    }));
            }
            set => _closeCommand = value;
        }

        private RelayCommand _connectCommand;
        public RelayCommand ConnectCommand
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new RelayCommand(
                    t =>
                    {
                        var window = (ConfigSelectWindow)t;

                        if (SelectAction != null)
                        {
                            IsSuccess = SelectAction(window.SettingComboBox.SelectedItem.ToString(), window);
                        }
                        else
                        {
                            window.Close();
                        }
                    }));
            }
            set => _connectCommand = value;
        }

        #endregion
    }
}
