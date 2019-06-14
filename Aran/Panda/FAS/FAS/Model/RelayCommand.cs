using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FAS.Model
{
    public class RelayCommand : BaseModel, ICommand
    {
        private Action<object> _action;
        private bool _isEnabled;
        private bool _isVisible;

        public RelayCommand()
        {
            _isEnabled = true;
            _isVisible = true;
        }

        public RelayCommand(Action<object> action)
        {
            Action = action;
        }

        public Action<object> Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value;
                IsEnabled = (value != null);
            }
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _isEnabled;
        }

        public void Execute(object parameter)
        {
            if (_action != null)
                _action(parameter);
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled == value)
                    return;
                
                _isEnabled = value;
                NotifyPropertyChanged("IsEnabled");
                
                if (CanExecuteChanged != null)
                    CanExecuteChanged(this, null);
            }
        }

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                if (_isVisible == value)
                    return;

                _isVisible = value;
                NotifyPropertyChanged("IsVisible");
            }
        }
    }
}
