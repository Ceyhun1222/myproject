using MvvmCore;

namespace Aran.Temporality.UserManager.ViewModel.Single
{
    public class SingleModuleAccessModel : ViewModelBase
    {
        public int Value { get; set; }

        private string _moduleName;
        public string ModuleName
        {
            get { return _moduleName; }
            set
            {
                _moduleName = value;
                OnPropertyChanged("ModuleName");
            }
        }

        private bool _isAllowed;
        public bool IsAllowed
        {
            get { return _isAllowed; }
            set
            {
                _isAllowed = value;
                OnPropertyChanged("IsAllowed");
            }
        }
    }
}
