using System;
using System.Windows;
using MvvmCore;

namespace TOSSM.ViewModel.Control.DatabaseDate
{
    public enum DatabaseState
    {
        Now,
        Past,
    }

    public class DatabaseDateViewModel : ViewModelBase
    {
        public DatabaseState SelectedDatabaseState { get; set; }

        public Action<DatabaseDateViewModel> OnChanged { get; set; }

        private int _selectedDatabaseStateIndex;
        private Visibility _customDateVisibility=Visibility.Collapsed;
        private DateTime _selectedDate=DateTime.Now;

        public int SelectedDatabaseStateIndex
        {
            get => _selectedDatabaseStateIndex;
            set
            {
                _selectedDatabaseStateIndex = value;
                SelectedDatabaseState = (DatabaseState)_selectedDatabaseStateIndex;
                OnPropertyChanged("SelectedDatabaseStateIndex");

                CustomDateVisibility = _selectedDatabaseStateIndex == 0 ? Visibility.Collapsed : Visibility.Visible;

                
                if (OnChanged!=null)
                {
                    OnChanged(this);
                }
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged("SelectedDate");
                if (OnChanged != null)
                {
                    OnChanged(this);
                }
            }
        }


        public Visibility CustomDateVisibility
        {
            get => _customDateVisibility;
            set
            {
                _customDateVisibility = value;
                OnPropertyChanged("CustomDateVisibility");
                if (OnChanged != null)
                {
                    OnChanged(this);
                }
            }
        }
    }
}
