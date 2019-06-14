using System;
using System.Windows;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.View;
using MvvmCore;

namespace Aran.Temporality.CommonUtil.ViewModel
{
    public class LoginModel : ViewModelBase
    {

        #region Properties

        private string _host;
        public string Host
        {
            get { return _host; }
            set
            {
                _host = value;
                OnPropertyChanged("Host");
            }
        }

        private int _port;
        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged("Port");
            }
        }

        private string _user;
        public string User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        #endregion

        #region Commands

        private RelayCommand _closeCommand;
        public RelayCommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand=new RelayCommand(
                    t =>
                    {
                        var window = (LoginWindow)t;

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
            set { _closeCommand = value; }
        }

        private RelayCommand _connectCommand;
        public RelayCommand ConnectCommand
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new RelayCommand(
                    t =>
                        {
                            var window = (LoginWindow) t;

                            var control = window.PasswordBox;
                            var password = HashUtil.ComputeHash(control.Password);

                            if (ConnectAction != null)
                            {
                                ConnectAction(password, window);
                            }
                            else
                            {
                                window.Close();
                            }
                        }));
            }
            set { _connectCommand = value; }
        }


        #endregion

        public Action<string,Window> ConnectAction { get; set; }
        public Action<Window> CloseAction { get; set; }
    }
}
