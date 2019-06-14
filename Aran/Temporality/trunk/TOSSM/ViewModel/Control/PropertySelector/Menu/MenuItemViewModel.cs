using System.Collections.ObjectModel;
using MvvmCore;

namespace TOSSM.ViewModel.Control.PropertySelector.Menu
{
    public class MenuItemViewModel : ViewModelBase
    {
        public MenuItemViewModel(MenuItemViewModel parentViewModel)
        {
            ParentViewModel = parentViewModel;
            ItemsSource = new ObservableCollection<MenuItemViewModel>();
        }

        private ObservableCollection<MenuItemViewModel> _itemsSource;
        public ObservableCollection<MenuItemViewModel> ItemsSource
        {
            get => _itemsSource;
            set
            {
                _itemsSource = value;
                OnPropertyChanged("ItemsSource");
            }
        }

        private bool _staysOpenOnClick;
        public bool StaysOpenOnClick
        {
            get => _staysOpenOnClick;
            set
            {
                _staysOpenOnClick = value;
                OnPropertyChanged("StaysOpenOnClick");
            }
        }

        private bool _isCheckable;
        public bool IsCheckable
        {
            get => _isCheckable;
            set
            {
                if (_isCheckable == value) return;
                _isCheckable = value;
                OnPropertyChanged("IsCheckable");
            }
        }

        public virtual void OnCheckedChanged()
        {
        }


        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked==value) return;
                _isChecked = value;
                OnPropertyChanged("IsChecked");
                OnCheckedChanged();
            }
        }

        private string _header;
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged("Header");
            }
        }

        public MenuItemViewModel ParentViewModel { get; set; }


        public object IconSource { get; set; }

        public override string ToString()
        {
            return Header + (IsCheckable ? (IsChecked ? ", +" : ", -") : "");
        }
    }
}
