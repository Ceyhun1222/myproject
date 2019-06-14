using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Aran.Temporality.Common.Util;
using CommonUtil.Util;
using MvvmCore;
using UserManager.ViewModel.Util;

namespace UserManager.ViewModel.Extended
{
    class UserManagerModel : ViewModelBase
    {
        private ObservableCollection<UserModel> _userList;
        public ObservableCollection<UserModel> UserList
        {
            get
            {
                if (_userList==null)
                {
                    _userList=new ObservableCollection<UserModel>();
                    var users=CurrentDataContext.CurrentNoDataService.GetAllUsers().OrderBy(t=>t.Name);
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

        public ObservableCollection<UserModel> UserListFiltered
        {
            get
            {
                if (String.IsNullOrEmpty(UserFilter))
                    return new ObservableCollection<UserModel>(UserList);
                return new ObservableCollection<UserModel>(
                    UserList.Where(t => t.UserName.ToLower().StartsWith(UserFilter.ToLower())));
            }
        }

        private string _userFilter;
        public string UserFilter
        {
            get
            {
                return _userFilter;
            }
            set
            {
                _userFilter = value;
                OnPropertyChanged("UserFilter");
                OnPropertyChanged("UserListFiltered");
                OnPropertyChanged("FilterEmptyButtonVisibility");
                
            }
        }

        private RelayCommand _onClearFilter;
        public RelayCommand OnClearFilter
        {
            get {
                return _onClearFilter ??
                       (_onClearFilter = new RelayCommand(t1 => UserFilter = "", t2 => !String.IsNullOrEmpty(UserFilter)));
            }
            set
            {
                _onClearFilter = value;
                OnPropertyChanged("OnClearFilter");
            }
        }

        private UserModel _selectedUser;
        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                if (_selectedUser!=null)
                {
                    _selectedUser.CurrentRoleFlag = _selectedUser.RoleFlag;
                    _selectedUser.CurrentUserName = _selectedUser.UserName;
                }
                
                OnPropertyChanged("SelectedUser");
            }
        }

        public Visibility FilterEmptyButtonVisibility
        {
            get { return String.IsNullOrEmpty(UserFilter)?Visibility.Collapsed:Visibility.Visible; }
        }

        private RelayCommand _onAddUser;
        public RelayCommand OnAddUser
        {
            get
            {
                return _onAddUser ?? (_onAddUser = new RelayCommand(t =>
                {
                    string username = "user";

                    var id =
                        CurrentDataContext.CurrentNoDataService.CreateUser(username);
                    var newUser = new UserModel
                                        {
                                            Id = (int) id,
                                            UserName = username,
                                        };
                    UserList.Add(newUser);
                    SelectedUser = newUser;

                    OnPropertyChanged("UserList");
                    OnPropertyChanged("UserListFiltered");
                }));
            }
            set
            {
                _onAddUser = value;
            }
        }
    }
}
