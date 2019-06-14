using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.ViewModel.Single;
using MvvmCore;

namespace Aran.Temporality.CommonUtil.ViewModel
{
    public class EasyUserViewModel : ViewModelBase
    {
        public string PandaVersion { get; set; }
        public string DeltaVersion { get; set; }
        public string OmegaVersion { get; set; }
        public string ArenaVersion { get; set; }
        public string TossmVersion { get; set; }

        public void UpdateVersion()
        {
            OnPropertyChanged("PandaVersion");
            OnPropertyChanged("DeltaVersion");
            OnPropertyChanged("OmegaVersion");
            OnPropertyChanged("ArenaVersion");
            OnPropertyChanged("TossmVersion");
        }

        public User User { get; set; }

        #region Simple properties

        public int Id => User?.Id ?? -1;

        public bool IsNotEnabled { get; set; }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                IsNotEnabled = !IsEnabled;
                IsObserverVisible = IsEnabled ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged("IsEnabled");
                OnPropertyChanged(nameof(IsGroupsEnabled));
                OnPropertyChanged(nameof(IsNotEnabled));
            }
        }

        public static string GetDescription(Enum en)
        {
            var type = en.GetType();
            var memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }

        private int _moduleFlag;
        public int ModuleFlag
        {
            get => _moduleFlag;
            set
            {
                _moduleFlag = value;

                 Modules.Clear();
                
                
                foreach (Module module in Enum.GetValues(typeof(Module)))
                {
                    if (module==Module.None) continue;
                    var description = GetDescription(module);
                    var v = (int) module;
                    Modules.Add(new SingleModuleAccessModel
                            {
                                ModuleName = description,
                                Value = v,
                                IsAllowed = (ModuleFlag&v)!=0
                            });
                }

                OnPropertyChanged("ModuleFlag");
            }
        }

        private int _roleFlag;
        public int RoleFlag
        {
            get => _roleFlag;
            set
            {
                _roleFlag = value;
                CurrentRoleFlag = value;
                OnPropertyChanged("RoleFlag");

                IsTester = IsTester;
            }
        }

        public string UserName => User == null ? "No user" : User.Name;

        private ObservableCollection<SingleModuleAccessModel> _modules;
        public ObservableCollection<SingleModuleAccessModel> Modules
        {
            get => _modules??(_modules=new ObservableCollection<SingleModuleAccessModel>());
            set => _modules = value;
        }


        public EasyUserGroupViewModel UserGroup
        {
            get
            {
                var userGroupId = User.UserGroup?.Id ?? -1;
                return UserGroups?.FirstOrDefault(t => t.Id == userGroupId); 
            }
        }


        public ObservableCollection<EasyUserGroupViewModel> UserGroups { get; set; }

        public void UpdateUi()
        {
            if (User == null) return;

            ModuleFlag = User.ModuleFlag;
            RoleFlag = User.RoleFlag;
            CurrentUserName = UserName;

            IsGroupsEnabled = UserGroups != null && UserGroups.Count > 0;
          
            if (UserGroup != null && UserGroups != null)
            {
                var id=UserGroup.Id;
                CurrentUserGroup = UserGroups.FirstOrDefault(t => t.Id == id);

                CurrentUserGroup?.UpdateUi();
            }
            else
            {
                CurrentUserGroup = null;
            }

            OnPropertyChanged("UserGroups");
            OnPropertyChanged("UserGroup");
            OnPropertyChanged("UserName");
            OnPropertyChanged("CurrentUserGroup");
        }

        private bool _isGroupsEnabled;
        public bool IsGroupsEnabled
        {
            get => _isGroupsEnabled && IsEnabled;
            set
            {
                _isGroupsEnabled = value;
                OnPropertyChanged("IsGroupsEnabled");
            }
        }


        #endregion

        
        #region Is Role Properties

        public bool IsAdmin
        {
            get => (_currentRoleFlag & (int)UserRole.Admin) != 0;
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
            get => (_currentRoleFlag & (int)UserRole.SuperAdmin) != 0;
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
            get => (_currentRoleFlag & (int)UserRole.Creator) != 0;
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
            get => (_currentRoleFlag & (int)UserRole.Destroyer) != 0;
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
            get => (_currentRoleFlag & (int)UserRole.Observer) != 0;
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


        private Visibility _isObserverVisible=Visibility.Hidden;
        public Visibility IsObserverVisible
        {
            get => _isObserverVisible;
            set
            {
                _isObserverVisible = value;
                OnPropertyChanged("IsObserverVisible");
            }
        }

        private bool _isObserverEnabled=true;
        public bool IsObserverEnabled
        {
            get => _isObserverEnabled;
            set
            {
                _isObserverEnabled = value;
                OnPropertyChanged("IsObserverEnabled");
            }
        }

        public bool IsTester
        {
            get => (_currentRoleFlag & (int)UserRole.Tester) != 0;
            set
            {
                if (value)
                {
                    _currentRoleFlag |= (int)UserRole.Tester;
                    IsObserver = false;
                    IsObserverEnabled = false;
                    IsObserverVisible=Visibility.Visible;
                }
                else
                {
                    IsObserverEnabled = true;
                    IsObserverVisible = Visibility.Visible;
                    _currentRoleFlag = _currentRoleFlag & ~(int)UserRole.Tester;
                }

                OnPropertyChanged("IsTester");
            }
        }

        public bool IsAimslUser
        {
            get => (_currentRoleFlag & (int)UserRole.Aimsl) != 0;
            set
            {
                if (value)
                {
                    _currentRoleFlag |= (int)UserRole.Aimsl;
                }
                else
                {
                    _currentRoleFlag = _currentRoleFlag & ~(int)UserRole.Aimsl;
                }

                OnPropertyChanged(nameof(IsAimslUser));
            }
        }

        public bool IsPublisher
        {
            get => (_currentRoleFlag & (int)UserRole.Publisher) != 0;
            set
            {
                if (value)
                {
                    _currentRoleFlag |= (int)UserRole.Publisher;
                }
                else
                {
                    _currentRoleFlag = _currentRoleFlag & ~(int)UserRole.Publisher;
                }

                OnPropertyChanged(nameof(IsPublisher));
            }
        }

        #endregion

        #region Commands

        public void Apply()
        {
            if (UserName != CurrentUserName)
            {
                if (CurrentDataContext.CurrentNoAixmDataService.SetUserName(Id, CurrentUserName))
                {
                    User.Name = CurrentUserName;
                }
                else
                {
                    MessageBox.Show("User with name " + CurrentUserName +
                        " is already exists, so the name was restored as " +
                        UserName);
                    CurrentUserName = UserName;
                }
            }


            CalculateCurrentModuleFlag();
            if (ModuleFlag != CurrentModuleFlag)
            {
                if (CurrentDataContext.CurrentNoAixmDataService.SetUserModules(Id, CurrentModuleFlag))
                {
                    _moduleFlag = CurrentModuleFlag;
                    User.ModuleFlag = CurrentModuleFlag;
                }
                else
                {
                    MessageBox.Show("Some roles can not be applied");
                    CurrentModuleFlag = ModuleFlag;
                }
            }


            if (RoleFlag != CurrentRoleFlag)
            {
                if (CurrentDataContext.CurrentNoAixmDataService.SetUserRole(Id, CurrentRoleFlag))
                {
                    RoleFlag = CurrentRoleFlag;
                    User.RoleFlag = CurrentRoleFlag;
                }
                else
                {
                    MessageBox.Show("Some roles can not be applied");
                    CurrentRoleFlag = RoleFlag;
                }
            }

            if (UserGroup != CurrentUserGroup)
            {
                if (CurrentDataContext.CurrentNoAixmDataService.SetUserGroup(Id, CurrentUserGroup == null ? -1 : CurrentUserGroup.Id))
                {
                    User.UserGroup = CurrentUserGroup?.UserGroup;
                }
                else
                {
                    MessageBox.Show("Group can not be set, it was reset to old value");
                    CurrentUserGroup = UserGroup;
                }
            }

            UpdateUi();
        }


        #endregion

        #region Temporary value holders

        private string _currentUserName;
        public string CurrentUserName
        {
            get => _currentUserName;
            set
            {
                _currentUserName = value;
                OnPropertyChanged("CurrentUserName");
            }
        }

        private EasyUserGroupViewModel _currentUserGroup;
        public EasyUserGroupViewModel CurrentUserGroup
        {
            get => _currentUserGroup;
            set
            {
                _currentUserGroup = value;
                OnPropertyChanged("CurrentUserGroup");
            }
        }

        public void CalculateCurrentModuleFlag()
        {
            CurrentModuleFlag = Modules.Where(model => model.IsAllowed).
                Aggregate(0, (current, model) => current | model.Value);
        }

        private int _currentModuleFlag;
        public int CurrentModuleFlag
        {
            get => _currentModuleFlag;
            set
            {
                _currentModuleFlag = value;
                OnPropertyChanged("CurrentModuleFlag");
            }
        }

        private int _currentRoleFlag;

        public int CurrentRoleFlag
        {
            get => _currentRoleFlag;
            set
            {
                _currentRoleFlag = value;
                if (_currentRoleFlag >= 0)
                    IsEnabled = true;
                if (_currentRoleFlag < 0)
                    IsEnabled = false;
                OnPropertyChanged(nameof(CurrentRoleFlag));
                OnPropertyChanged(nameof(Active));
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        private bool _active;

        public bool Active
        {
            get => _currentRoleFlag > 0;
            set
            {
                _active = value;
                OnPropertyChanged(nameof(Active));
            }
        }

        public bool IsUiEnable => true;

        #endregion 

       
      
     
    }
}
