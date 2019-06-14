using System;
using System.ComponentModel;
using System.Windows.Input;
using DataImporter.Models;

namespace DataImporter.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged,IImportPageVM
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        RelayCommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RelayCommand(
                           param => Close()
                       ));
            }
        }

        public event Action RequestClose;

        public virtual void Close()
        {
            RequestClose?.Invoke();
        }

        public virtual bool CanClose()
        {
            return true;
        }

        public abstract void Save();

        public abstract void Load(string fileName);
        public abstract IFeatType FeatType { get;}
    }
}
