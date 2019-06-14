using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Aran.Aim;
using CommonUtil.Util;
using MvvmCore;

namespace UserManager.ViewModel.Extended
{
    class AccessManagerModel : ViewModelBase
    {
        private ObservableCollection<UserModel> _userList;
        public ObservableCollection<UserModel> UserList
        {
            get
            {
                //if (_userList == null)
                {
                    _userList = new ObservableCollection<UserModel>();
                    var users = CurrentDataContext.CurrentNoDataService.GetAllUsers().OrderBy(t => t.Name);
                    foreach (var user in users)
                    {
                        _userList.Add(new UserModel
                        {
                            UserName = user.Name,
                            Id = user.Id,
                            RoleFlag = user.RoleFlag
                        });
                    }

                }
                return _userList;
            }
            set
            {
                _userList = value;
                OnPropertyChanged("UserList");
            }
        }

        private string _storageFilter;
        public string StorageFilter
        {
            get { return _storageFilter; }
            set
            {
                _storageFilter = value;
                OnPropertyChanged("StorageFilter");
                OnPropertyChanged("StorageFilterEmptyButtonVisibility");
            }
        }

        private RelayCommand _onClearStorageFilter;
        public RelayCommand OnClearStorageFilter
        {
            get
            {
                return _onClearStorageFilter ??
                       (_onClearStorageFilter = new RelayCommand(t1 => StorageFilter = "", t2 => !String.IsNullOrEmpty(StorageFilter)));
            }
            set
            {
                _onClearStorageFilter = value;
                OnPropertyChanged("OnClearStorageFilter");
            }
        }



        public Visibility StorageFilterEmptyButtonVisibility
        {
            get { return String.IsNullOrEmpty(StorageFilter) ? Visibility.Collapsed : Visibility.Visible; }
        }

        private string _featureFilter;
        public string FeatureFilter
        {
            get { return _featureFilter; }
            set
            {
                _featureFilter = value;
                OnPropertyChanged("FeatureFilter");
                OnPropertyChanged("FeatureFilterEmptyButtonVisibility");
                OnPropertyChanged("FilteredFeatureList");
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

        private ObservableCollection<SingleFeatureAccessModel> _featureList;
        public ObservableCollection<SingleFeatureAccessModel> FeatureList
        {
            get
            {
                if (_featureList==null)
                {
                    _featureList = new ObservableCollection<SingleFeatureAccessModel>();
                    foreach (FeatureType ft in Enum.GetValues(typeof(FeatureType)))
                    {
                        _featureList.Add(new SingleFeatureAccessModel{Feature = ft});
                    }
                }
                return _featureList;
            }
            set { _featureList = value; }
        }


        public ObservableCollection<SingleFeatureAccessModel> FilteredFeatureList
        {
            get
            {
                if (String.IsNullOrEmpty(FeatureFilter))
                    return new ObservableCollection<SingleFeatureAccessModel>(FeatureList.OrderBy(t=>t.Feature));

                return new ObservableCollection<SingleFeatureAccessModel>(
                FeatureList.Where(t => t.Feature.ToString().ToLower().StartsWith(FeatureFilter.ToLower())).OrderBy(t => t.Feature));
            }
        }

        private RelayCommand _onNoAccess;
        public RelayCommand OnNoAccess
        {
            get
            {
                return _onNoAccess ?? (_onNoAccess = new RelayCommand(
                                                 t =>
                                                 {
                                                     foreach (var model in
                                                             ((IList)t).OfType<SingleFeatureAccessModel>())
                                                     {
                                                         model.IsRead = false;
                                                         model.IsWrite = false;
                                                     }
                                                 }));
            }
            set { _onNoAccess = value; }
        }

        private RelayCommand _onReadOnly;
        public RelayCommand OnReadOnly
        {
            get
            {
                return _onReadOnly ?? (_onReadOnly = new RelayCommand(
                                                 t =>
                                                 {
                                                     foreach (var model in
                                                             ((IList)t).OfType<SingleFeatureAccessModel>())
                                                     {
                                                         model.IsRead = true;
                                                         model.IsWrite = false;
                                                     }
                                                 }));
            }
            set { _onReadOnly = value; }
        }

        private RelayCommand _onFull;
        public RelayCommand OnFull
        {
            get
            {
                return _onFull ?? (_onFull = new RelayCommand(
                                                 t =>
                                                     {
                                                         foreach (var model in
                                                                 ((IList) t).OfType<SingleFeatureAccessModel>())
                                                         {
                                                             model.IsRead = true;
                                                             model.IsWrite = true;
                                                         }
                                                     }));
            }
            set { _onFull = value; }
        }

        private RelayCommand _onApply;
        public RelayCommand OnApply
        {
            get
            {
                if (_onApply==null)
                {
                   // _onApply=new RelayCommand();
                }
                return _onApply;
            }
            set { _onApply = value; }
        }
    }
}
