using System;
using System.ComponentModel;

namespace Aran.Aim.CAWProvider
{
    public class ConnectionInfo : INotifyPropertyChanged
    {
        private PropertyChangedEventHandler _propChangedEventHandler;
        private Uri _server;
        private string _userName;
        private string _password;

        
        public ConnectionInfo ()
        {
        }

        public ConnectionInfo (Uri server, string userName, string password)
        {
            Server = server;
            UserName = userName;
            Password = password;
        }

        
        public Uri Server
        {
            get { return _server; }
            set
            {
                _server = value;
                DoPropChanged("Server");
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName == value)
                    return;
                _userName = value;
                DoPropChanged("UserName");
            }
        }
        
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                    return;
                _password = value;
                DoPropChanged("Password");
            }
        }

        private void DoPropChanged(string propName)
        {
            if (_propChangedEventHandler != null)
                _propChangedEventHandler(this, new PropertyChangedEventArgs(propName));
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { _propChangedEventHandler += value; }
            remove { _propChangedEventHandler -= value; }
        }
    }
}
