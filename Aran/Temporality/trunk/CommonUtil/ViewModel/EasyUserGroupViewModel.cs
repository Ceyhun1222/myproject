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
using Aran.Temporality.CommonUtil.ViewModel.Single;
using MvvmCore;

namespace Aran.Temporality.CommonUtil.ViewModel
{
    public class EasyUserGroupViewModel : ViewModelBase
    {
        public UserGroup UserGroup { get; set; }

        public void UpdateUi()
        {
            SetCurrentAccess();
            CurrentGroupName = GroupName;
            OnPropertyChanged("CurrentGroupName");
            OnPropertyChanged("GroupName");
        }

        #region Simple properties

        public int Id {
            get { return UserGroup == null ? -1 : UserGroup.Id; }
        }

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
                OnPropertyChanged("IsEnabled");
                OnPropertyChanged("IsNotEnabled");
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

        public string GroupName
        {
            get
            {
                return UserGroup==null?"No Group":UserGroup.Name;
            }
        }

        public ObservableCollection<SingleFeatureAccessModel> FeatureList { get; set; }

        public ObservableCollection<SingleFeatureAccessModel> FilteredFeatureList { get; set; }

        
        #endregion

        
       

        #region Commands

        public void Apply()
        {
            if (GroupName != CurrentGroupName)
            {
                if (CurrentDataContext.CurrentNoAixmDataService.SetGroupName(Id, CurrentGroupName))
                {
                    UserGroup.Name = CurrentGroupName;
                }
                else
                {
                    MessageBox.Show("Group with name " + CurrentGroupName +
                        " is already exists, so the name was restored as " +
                        GroupName);
                    CurrentGroupName = GroupName;
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
                    UserGroupId = Id,
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

            UpdateUi();
        }

        public bool IsUiEnable
        {
            get { return true; }
        }

        #endregion

        #region Temporary value holders

        private string _currentGroupName;
        public string CurrentGroupName
        {
            get
            {
                return _currentGroupName;
            }
            set
            {
                _currentGroupName = value;
                OnPropertyChanged("CurrentGroupName");
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
                return CurrentFeatureList == null ? 0 : CurrentFeatureList.Count(t => t!=null && t.DataOperationFlag == 0);
            }
        }

        public int ReadOnlyCount
        {
            get
            {
                return CurrentFeatureList == null ? 0 : CurrentFeatureList.Count(t => t != null && t.DataOperationFlag ==
                    (int)DataOperation.ReadData);
            }
        }

        public int ReadWriteCount
        {
            get
            {
                return CurrentFeatureList == null ? 0 : CurrentFeatureList.Count(t => t != null && t.DataOperationFlag ==
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
