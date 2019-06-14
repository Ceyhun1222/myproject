using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Util;
using CommonUtil.Util;
using MvvmCore;
using UserManager.ViewModel.Util;

namespace UserManager.ViewModel.Extended
{
    class UserModel : ViewModelBase
    {
        public int Id { get; set; }

        private int _roleFlag;
        public int RoleFlag
        {
            get { return _roleFlag; }
            set
            {
                _roleFlag = value;
                CurrentRoleFlag = value;
                OnPropertyChanged("RoleFlag");
            }
        }

        private string _userName;
        public string UserName
        {
            get
            {   
                return _userName;
            }
            set
            {   
                _userName = value;
                CurrentUserName = value;
                OnPropertyChanged("UserName");
                OnPropertyChanged("DisplayName");
            }
        }

        public override string DisplayName
        {
            get
            {
                return UserName;
            }
        }

        #region Is Role Properties

        public bool IsAdmin
        {
            get
            {
                return (_currentRoleFlag & (int)UserRole.Admin) != 0;
            }
            set
            {
                if (value)
                {
                    _currentRoleFlag |= (int)UserRole.Admin;  
                }
                else
                {
                    _currentRoleFlag = _currentRoleFlag & ~(int)UserRole.Admin;
                }
                
                OnPropertyChanged("IsAdmin");
            }
        }

        public bool IsSuperAdmin
        {
            get
            {
                return (_currentRoleFlag & (int)UserRole.SuperAdmin) != 0;
            }
            set
            {
                if (value)
                {
                    _currentRoleFlag |= (int)UserRole.SuperAdmin;
                }
                else
                {
                    _currentRoleFlag = _currentRoleFlag & ~(int)UserRole.SuperAdmin;
                }

                OnPropertyChanged("IsSuperAdmin");
            }
        }

        public bool IsCreator
        {
            get
            {
                return (_currentRoleFlag & (int)UserRole.Creator) != 0;
            }
            set
            {
                if (value)
                {
                    _currentRoleFlag |= (int)UserRole.Creator;
                }
                else
                {
                    _currentRoleFlag = _currentRoleFlag & ~(int)UserRole.Creator;
                }

                OnPropertyChanged("IsCreator");
            }
        }


        public bool IsDestroyer
        {
            get
            {
                return (_currentRoleFlag & (int)UserRole.Destroyer) != 0;
            }
            set
            {
                if (value)
                {
                    _currentRoleFlag |= (int)UserRole.Destroyer;
                }
                else
                {
                    _currentRoleFlag = _currentRoleFlag & ~(int)UserRole.Destroyer;
                }

                OnPropertyChanged("IsDestroyer");
            }
        }

        public bool IsObserver
        {
            get
            {
                return (_currentRoleFlag & (int)UserRole.Observer) != 0;
            }
            set
            {
                if (value)
                {
                    _currentRoleFlag |= (int)UserRole.Observer;
                }
                else
                {
                    _currentRoleFlag = _currentRoleFlag & ~(int)UserRole.Observer;
                }

                OnPropertyChanged("IsObserver");
            }
        }

        public bool IsTester
        {
            get
            {
                return (_currentRoleFlag & (int)UserRole.Tester) != 0;
            }
            set
            {
                if (value)
                {
                    _currentRoleFlag |= (int)UserRole.Tester;
                }
                else
                {
                    _currentRoleFlag = _currentRoleFlag & ~(int)UserRole.Tester;
                }

                OnPropertyChanged("IsTester");
            }
        }

        #endregion

        private RelayCommand _onResetPassword;
        public RelayCommand OnResetPassword
        {
            get
            {
                if (_onResetPassword==null)
                {
                    //_onResetPassword=new RelayCommand();
                }
                return _onResetPassword;
            }
            set
            {
                _onResetPassword = value;
                OnPropertyChanged("OnResetPassword");
            }
        }

        private RelayCommand _onApply;
        public RelayCommand OnApply
        {
            get
            {
                if (_onApply==null)
                {
                    _onApply=new RelayCommand(t=>
                                                  {
                                                      if (CurrentDataContext.CurrentNoDataService.SetUserName(Id,CurrentUserName))
                                                      {

                                                          UserName = CurrentUserName;
                                                      }

                                                      if (CurrentDataContext.CurrentNoDataService.SetUserRole(Id, CurrentRoleFlag))
                                                      {
                                                          RoleFlag = CurrentRoleFlag;
                                                      }
                                                     
                                                  });
                }
                return _onApply;
            }
            set
            {
                _onApply = value;
                OnPropertyChanged("OnApply");
            }
        }

        public bool IsUIEnable
        {
            get { return true; }
        }

        private RelayCommand _onDelete;
        public RelayCommand OnDelete
        {
            get { return _onDelete; }
            set { _onDelete = value; }
        }

        #region Temporary value holders

        private string _currentUserName;
        public string CurrentUserName
        {
            get
            {
                return _currentUserName;
            }
            set
            {
                _currentUserName = value;
                OnPropertyChanged("CurrentUserName");
            }
        }

        private int _currentRoleFlag;
        public int CurrentRoleFlag
        {
            get
            {
                return _currentRoleFlag;
            }
            set
            {
                _currentRoleFlag = value;
                OnPropertyChanged("CurrentRoleFlag");
            }
        }

        #endregion
    }
}
