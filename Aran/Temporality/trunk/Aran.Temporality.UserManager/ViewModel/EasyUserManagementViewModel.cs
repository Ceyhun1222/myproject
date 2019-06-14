using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.ViewModel;
using Aran.Temporality.UserManager.View;
using Aran.Temporality.UserManager.ViewModel.Single;
using MvvmCore;

namespace Aran.Temporality.UserManager.ViewModel
{
    public class EasyUserManagementViewModel : ViewModelBase
    {
        private bool _loaded;
        public void Load()
        {
            if (_loaded) return;
            _loaded = true;

            BlockerModel.BlockForAction(
                ()=>
                    {
                        var users = CurrentDataContext.CurrentNoAixmDataService.GetAllUsers().OrderBy(t => t.Name);
                        foreach (var user in users)
                        {
                            var user1 = user;
                            Application.Current.Dispatcher.Invoke(
                                DispatcherPriority.Background,
                                (Action)(() => UserList.Add(new EasyUserViewModel
                                                                 {
                                                                     UserName = user1.Name,
                                                                     Id = user1.Id,
                                                                     RoleFlag = user1.RoleFlag,
                                                                     ModuleFlag = user1.ModuleFlag
                                                                 })));
                        }
                        OnPropertyChanged("UserListFiltered");
                    });
        }

        #region User list

        private ObservableCollection<EasyUserViewModel> _userList;
        public ObservableCollection<EasyUserViewModel> UserList
        {
            get { return _userList ?? (_userList = new ObservableCollection<EasyUserViewModel>()); }
            set
            {
                _userList = value;
                OnPropertyChanged("UserList");
            }
        }

        public ObservableCollection<EasyUserViewModel> UserListFiltered
        {
            get
            {
                if (String.IsNullOrEmpty(UserFilter))
                    return new ObservableCollection<EasyUserViewModel>(UserList.OrderBy(t=>t.UserName));
                return new ObservableCollection<EasyUserViewModel>(
                    UserList.Where(t => t.UserName.ToLower().Contains(UserFilter.ToLower())).OrderBy(t=>t.UserName));
            }
        }

        #endregion

        #region User filter

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

        public Visibility FilterEmptyButtonVisibility
        {
            get { return String.IsNullOrEmpty(UserFilter) ? Visibility.Collapsed : Visibility.Visible; }
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

        #endregion

        #region Selected user

        private EasyUserViewModel _selectedUser;
        public EasyUserViewModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                if (_selectedUser!=null)
                {
                    _selectedUser.CurrentRoleFlag = _selectedUser.RoleFlag;
                    _selectedUser.CurrentModuleFlag = _selectedUser.ModuleFlag;
                    _selectedUser.CurrentUserName = _selectedUser.UserName;
                    _selectedUser.SetCurrentAccess();
                }
                
                OnPropertyChanged("SelectedUser");
            }
        }

        #endregion

        #region Blocker

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get
            {
                return _blockerModel ?? (_blockerModel=new BlockerModel());
            }
            set { _blockerModel = value; }
        }

        #endregion

        #region Commands

        private RelayCommand _onAddUser;
        public RelayCommand OnAddUser
        {
            get
            {
                return _onAddUser ?? (_onAddUser = new RelayCommand(t => BlockerModel.BlockForAction(() =>
                {
                    var count = 1;
                    string username;
                    while (true)
                    {
                        username = "user-" + (count++).ToString("000");
                        if (!UserList.Where(p => p.UserName.ToLower() == username.ToLower()).Any())
                        {
                            break;
                        }
                    }
                    //
                    var id = CurrentDataContext.CurrentNoAixmDataService.CreateUser(username);

                    if (id > -1)
                    {
                        var newUser = new EasyUserViewModel
                                        {
                                            Id = (int)id,
                                            UserName = username
                                        };
                        UserList.Add(newUser);
                        SelectedUser = newUser;

                        OnPropertyChanged("UserListFiltered");
                    }
                    else
                    {
                        MessageBox.Show("User can not be added");
                    }
                })));
            }
            set
            {
                _onAddUser = value;
            }
        }

        private RelayCommand _onDeleteUser;
        public RelayCommand OnDeleteUser
        {
            get { return _onDeleteUser ?? (_onDeleteUser=
                new RelayCommand(
                    t=>
                        {
                            if (SelectedUser==null) return;
                            BlockerModel.BlockForAction(DeleteCurrentUser);
                        },
                    t => SelectedUser != null)); }
            set { _onDeleteUser = value; }
        }

        private RelayCommand _onResetPassword;
        public RelayCommand OnResetPassword
        {
            get
            {
                return _onResetPassword ?? (_onResetPassword =
                    new RelayCommand(
                  t =>
                  {
                      if (SelectedUser == null) return;
                      BlockerModel.BlockForAction(ResetPasswordForCurrentUser);
                  },
                  t => SelectedUser != null));
            }
            set { _onResetPassword = value; }
        }

        private RelayCommand _onApply;
        public RelayCommand OnApply
        {
            get
            {
                return _onApply ?? (_onApply =
                    new RelayCommand(
                  t =>
                  {
                      if (SelectedUser == null) return;
                      BlockerModel.BlockForAction(SelectedUser.Apply);
                  },
                  t => SelectedUser != null));
            }
            set { _onApply = value; }
        }


        #region Change access buttons

        private RelayCommand _onNoAccess;
        public RelayCommand OnNoAccess
        {
            get
            {
                return _onNoAccess ?? (_onNoAccess = new RelayCommand(
                                                 t =>
                                                     {
                                                         var win = t as EasyUserViewControl;
                                                         if (win == null) return;

                                                         foreach (var model in win.FilteredFeatureList.SelectedItems.OfType<SingleFeatureAccessModel>())
                                                         {
                                                             model.IsRead = false;
                                                             model.IsWrite = false;
                                                         }
                                                 },
                                                 t => SelectedUser != null));
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
                                                     var win = t as EasyUserViewControl;
                                                     if (win == null) return;

                                                     foreach (var model in win.FilteredFeatureList.SelectedItems.OfType<SingleFeatureAccessModel>())
                                                     {
                                                         model.IsRead = true;
                                                         model.IsWrite = false;
                                                     }
                                                 },
                                                 t=> SelectedUser != null));
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
                                                     var win = t as EasyUserViewControl;
                                                     if (win == null) return;

                                                     foreach (var model in win.FilteredFeatureList.SelectedItems.OfType<SingleFeatureAccessModel>())
                                                     {
                                                         model.IsRead = true;
                                                         model.IsWrite = true;
                                                     }
                                                 },
                                                 t=> SelectedUser != null));
            }
            set { _onFull = value; }
        }

        private RelayCommand _onPrepareInstall;
        public RelayCommand OnPrepareInstall
        {
            get
            {
                return _onPrepareInstall??(_onPrepareInstall=
                    new RelayCommand(
                        t=>
                            {
                                var normalName = SelectedUser.UserName;
                                if (string.IsNullOrWhiteSpace(normalName))
                                {
                                    normalName = "NoName";
                                }
                                normalName = normalName.Replace("/", "_");
                                normalName = normalName.Replace("\\", "_");
                                normalName = normalName.Replace("?", "_");
                                normalName = normalName.Replace("*", "_");
                                normalName = normalName.Replace("%", "_");
                                normalName = normalName.Replace(":", "_");
                                normalName = normalName.Replace("|", "_");
                                normalName = normalName.Replace("\"", "_");
                                normalName = normalName.Replace(">", "_");
                                normalName = normalName.Replace("<", "_");
                                normalName = normalName.Replace(".", "_");
                                

                                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                                              {
                                                  FileName = normalName,
                                                  DefaultExt = ".reg",
                                                  Filter = "Registry files (.reg)|*.reg",
                                              };
                              
                                if (saveFileDialog.ShowDialog() == true)
                                {
                                    var sb = new StringBuilder();
                                    sb.AppendLine("Windows Registry Editor Version 5.00");
                                    sb.AppendLine();
                                    sb.AppendLine(@"[HKEY_CURRENT_USER\Software\RISK\Aran]");
                                    sb.AppendLine("\"UserId\"=\"" + SelectedUser.Id + "\"");
                                    sb.AppendLine("\"ServiceAddress\"=\""+CurrentDataContext.ServiceAddress +"\"");
                                    sb.AppendLine("\"StorageName\"=\""+CurrentDataContext.StorageName +"\"");

                                    try
                                    {
                                        var writer = new StreamWriter(saveFileDialog.FileName);
                                        writer.Write(sb.ToString());
                                        writer.Close();
                                    }
                                    catch (Exception)
                                    {
                                        MessageBox.Show("File can not be created. " +
                                                        "May be it is in use by another process " +
                                                        "or you do not have permission to create " +
                                                        "files in specified folder.", 
                                                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                   
                                }
                            },
                        t => SelectedUser != null));
            }
            set { _onPrepareInstall = value; }
        }

        #endregion


        #endregion

        #region Logic

        public void DeleteCurrentUser()
        {
            if (MessageBox.Show("You are going to delete selected user. Are you sure?",
                                "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) 
                                != MessageBoxResult.Yes)
            {
                return;
            }

            var id = SelectedUser.Id;
            UserList.Remove(SelectedUser);
            OnPropertyChanged("UserListFiltered");
            CurrentDataContext.CurrentNoAixmDataService.DeleteUserById(id);
            SelectedUser = null;
        }

        public void ResetPasswordForCurrentUser()
        {
            if (MessageBox.Show("You are going to reset password for selected user. Are you sure?",
               "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                CurrentDataContext.CurrentNoAixmDataService.ResetPasswordById(SelectedUser.Id);
            }
        }

        #endregion
    }
    
}
