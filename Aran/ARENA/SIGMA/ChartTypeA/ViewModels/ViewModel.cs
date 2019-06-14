using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using ChartTypeA.Models;

namespace ChartTypeA.ViewModels
{
    public abstract class ViewModel : NotifyClass
    {

        public string Header { get; set; }

        RelayCommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(
                       param => Close()
                       );
                }
                return _closeCommand;
            }
        }

        public event Action RequestClose;

        public virtual void Close()
        {
            if (RequestClose != null)
            {
                RequestClose();
            }
        }

        public virtual bool CanClose()
        {
            return true;
        }



        public abstract void Clear();

        
    }
}
