using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Aran.Aim;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.UserManager.ViewModel.Single;
using MvvmCore;

namespace Aran.Temporality.UserManager.ViewModel
{
    public class EasyUserViewModel : ViewModelBase
    {
        #region Simple properties

        public int Id { get; set; }

        public AccessRightZipped AccessRightZipped { get; set; }

        public bool IsNotEnabled { get; set; }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                IsNotEnabled = !IsEnabled;
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
            get { return _moduleFlag; }
            set
            {
                _moduleFlag = value;

                if (Modules==null)
                {
                    Modules=new ObservableCollection<SingleModuleAccessModel>();
                }
                else
                {
                    Modules.Clear();
                }
                
                
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

        public ObservableCollection<SingleFeatureAccessModel> FeatureList { get; set; }

        public ObservableCollection<SingleFeatureAccessModel> FilteredFeatureList { get; set; }

        public ObservableCollection<SingleModuleAccessModel> Modules { get; set; }


        public override string DisplayName
        {
            get
            {
                return UserName;
            }
        }

        #endregion

        
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

        #region Commands

        public void Apply()
        {
            if (UserName != CurrentUserName)
            {
                if (CurrentDataContext.CurrentNoAixmDataService.SetUserName(Id, CurrentUserName))
                {
                    UserName = CurrentUserName;
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
                }
                else
                {
                    MessageBox.Show("Some roles can not be applied");
                    CurrentRoleFlag = RoleFlag;
                }
            }

            //from view model to model 
            var accessRights = CurrentFeatureList.Select(model
                        =>
                        new AccessRightUtil
                        {
                            MyFeatureTypeId = (int)model.Feature,
                            MyOperationFlag = model.DataOperationFlag
                        })
                        .ToList();

            //prepare zipped data
            if (CurrentAccessRightZipped == null)
            {
                //init
                CurrentAccessRightZipped = new AccessRightZipped
                {
                    UserId = Id,
                    WorkPackage = -1,
                    StorageId = -1,
                    OperationFlag = -1
                };
            }
            //set access rights
            AccessRightUtil.SetAccess(CurrentAccessRightZipped, accessRights);


            //if there is difference
            if (!AccessRightUtil.IsDataEqual(AccessRightZipped, CurrentAccessRightZipped))
            {
                if (CurrentDataContext.CurrentNoAixmDataService.SetUserRights(CurrentAccessRightZipped))
                {
                    AccessRightZipped = new AccessRightZipped(CurrentAccessRightZipped);
                }
                else
                {
                    MessageBox.Show("Some access rights can not be applied");
                    CurrentAccessRightZipped = new AccessRightZipped(AccessRightZipped);
                }
            }
        }

        public bool IsUIEnable
        {
            get { return true; }
            set { }
        }

        #endregion

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

        public void CalculateCurrentModuleFlag()
        {
            CurrentModuleFlag = Modules.Where(model => model.IsAllowed).
                Aggregate(0, (current, model) => current | model.Value);
        }

        private int _currentModuleFlag;
        public int CurrentModuleFlag
        {
            get
            {
                return _currentModuleFlag;
            }
            set
            {
                _currentModuleFlag = value;
                OnPropertyChanged("CurrentModuleFlag");
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

        public AccessRightZipped CurrentAccessRightZipped { get; set; }

        private ObservableCollection<SingleFeatureAccessModel> _currentFeatureList;
        public ObservableCollection<SingleFeatureAccessModel> CurrentFeatureList
        {
            get
            {
                if (_currentFeatureList == null)
                {
                    _currentFeatureList = new ObservableCollection<SingleFeatureAccessModel>();
                    foreach (FeatureType ft in Enum.GetValues(typeof(FeatureType)))
                    {
                        _currentFeatureList.Add(new SingleFeatureAccessModel
                        {
                            Feature = ft, 
                            OnStateChanged = ResetFeatureStates
                        });
                    }
                }
                return _currentFeatureList;
            }
            set { _currentFeatureList = value; }
        }

        private bool _isReadWriteChecked = true;
        public bool IsReadWriteChecked
        {
            get { return _isReadWriteChecked; }
            set
            {
                _isReadWriteChecked = value;
                OnPropertyChanged("IsReadWriteChecked");
            }
        }

        public ObservableCollection<SingleFeatureAccessModel> CurrentFilteredFeatureList
        {
            get
            {
                if (String.IsNullOrEmpty(FeatureFilter))
                    return new ObservableCollection<SingleFeatureAccessModel>(CurrentFeatureList.Where(
                        t => (t.IsRead && !t.IsWrite && IsReadOnlyChecked) ||
                            (t.IsWrite && IsReadWriteChecked) ||
                            (!t.IsRead && IsNoAccessChecked)
                        ).OrderBy(t => t.Feature.ToString()));

                return new ObservableCollection<SingleFeatureAccessModel>(
                CurrentFeatureList.Where(t => 
                    t.Feature.ToString().ToLower().Contains(FeatureFilter.ToLower())
                    &&((t.IsRead && IsReadOnlyChecked) ||
                            (t.IsWrite && !t.IsWrite && IsReadWriteChecked) ||
                            (!t.IsRead && IsNoAccessChecked))
                    ).OrderBy(t => t.Feature.ToString()));
            }
        }

        #endregion

        #region Feature filter

        private string _featureFilter;
        public string FeatureFilter
        {
            get { return _featureFilter; }
            set
            {
                _featureFilter = value;
                OnPropertyChanged("FeatureFilter");
                OnPropertyChanged("FeatureFilterEmptyButtonVisibility");
                OnPropertyChanged("CurrentFilteredFeatureList");
            }
        }

        public Visibility FeatureFilterEmptyButtonVisibility
        {
            get { return String.IsNullOrEmpty(FeatureFilter) ? Visibility.Collapsed : Visibility.Visible; }
        }


        private RelayCommand _onClearFeatureFilter;
        public RelayCommand OnClearFeatureFilter
        {
            get
            {
                return _onClearFeatureFilter ??
                       (_onClearFeatureFilter = new RelayCommand(t1 => FeatureFilter = "", t2 => !String.IsNullOrEmpty(FeatureFilter)));
            }
            set
            {
                _onClearFeatureFilter = value;
                OnPropertyChanged("OnClearFeatureFilter");
            }
        }


        public void ResetFeatureStates()
        {
            OnPropertyChanged("NoAccessCount");
            OnPropertyChanged("ReadOnlyCount");
            OnPropertyChanged("ReadWriteCount");
        }


        public int NoAccessCount
        {
            get
            {
                return CurrentFeatureList == null ? 0 : CurrentFeatureList.Count(t => t.DataOperationFlag == 0);
            }
        }

        public int ReadOnlyCount
        {
            get
            {
                return CurrentFeatureList == null ? 0 : CurrentFeatureList.Count(t => t.DataOperationFlag ==
                    (int)DataOperation.ReadData);
            }
        }

        public int ReadWriteCount
        {
            get
            {
                return CurrentFeatureList == null ? 0 : CurrentFeatureList.Count(t => t.DataOperationFlag ==
                    (int)DataOperation.WriteData + (int)DataOperation.ReadData);
            }
        }

        private bool _isNoAccessChecked = true;
        public bool IsNoAccessChecked
        {
            get { return _isNoAccessChecked; }
            set
            {
                _isNoAccessChecked = value;
                OnPropertyChanged("IsNoAccessChecked");
            }
        }

        private bool _isReadOnlyChecked = true;
        public bool IsReadOnlyChecked
        {
            get { return _isReadOnlyChecked; }
            set
            {
                _isReadOnlyChecked = value;
                OnPropertyChanged("IsReadOnlyChecked");
            }
        }



        private RelayCommand _filterChanged;
        public RelayCommand FilterChanged
        {
            get
            {
                return _filterChanged ??
                       (_filterChanged = new RelayCommand(t => OnPropertyChanged("CurrentFilteredFeatureList")));
            }
            set { _filterChanged = value; }
        }


        #endregion

     

        #region Util methods

        public void SetCurrentAccess()
        {
            if (FeatureList == null)
            {
                FeatureList = new ObservableCollection<SingleFeatureAccessModel>();
            }
            FeatureList.Clear();

            var accessRightsZipped = CurrentDataContext.CurrentNoAixmDataService.GetDefaultUserRights(Id);
            if (accessRightsZipped != null)
            {
                var accessRights = AccessRightUtil.DecodeRights(accessRightsZipped);
                //apply accessRights to FeatureList
                foreach (var accessRight in accessRights)
                {
                    FeatureList.Add(new SingleFeatureAccessModel
                                        {
                                            IsEnabled=IsEnabled,
                                            OnStateChanged = ResetFeatureStates,
                                            Feature = (FeatureType)accessRight.MyFeatureTypeId,
                                            IsRead = (accessRight.MyOperationFlag & (int)DataOperation.ReadData)!=0,
                                            IsWrite = (accessRight.MyOperationFlag & (int)DataOperation.WriteData) != 0,
                                        });
                }
            }
            else
            {
                foreach (FeatureType ft in Enum.GetValues(typeof(FeatureType)))
                {
                    FeatureList.Add(new SingleFeatureAccessModel
                                        {
                                            OnStateChanged = ResetFeatureStates,
                                            Feature = ft
                                        });
                }
            }

            //copy FeatureList into CurrentFeatureList
            CurrentFeatureList.Clear();
            foreach (var accessModel in FeatureList)
            {
                CurrentFeatureList.Add(new SingleFeatureAccessModel(accessModel));
            }

            OnPropertyChanged("CurrentFilteredFeatureList");
        }

        #endregion
    }
}
