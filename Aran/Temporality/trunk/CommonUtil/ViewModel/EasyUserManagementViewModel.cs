using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Aran.Aim.Features;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Report.UserReport;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.View;
using Aran.Temporality.CommonUtil.ViewModel.Single;
using Microsoft.Win32;
using MvvmCore;
using WebApiClient;

namespace Aran.Temporality.CommonUtil.ViewModel
{
    public class EasyUserManagementViewModel : ViewModelBase
    {
        private bool _loaded;

        public void Load()
        {
            if (_loaded) return;
            _loaded = true;

            BlockerModel.BlockForAction(
                () =>
                {
                    var groups =
                        CurrentDataContext.CurrentNoAixmDataService.GetAllUserGroups().OrderBy(t => t.Name).ToList();

                    foreach (UserGroup group in groups)
                    {
                        var group1 = group;
                        Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Background,
                            (Action) (() =>
                            {
                                GroupListFiltered.Add(new EasyUserGroupViewModel
                                {
                                    UserGroup = group1
                                });
                            }));
                    }

                    var users = CurrentDataContext.CurrentNoAixmDataService.GetAllUsers().OrderBy(t => t.Name);
                    foreach (var user in users)
                    {
                        var user1 = user;
                        Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Background,
                            (Action) (() =>
                            {
                                var versions =
                                    CurrentDataContext.CurrentNoAixmDataService.GetUserModuleVersions(user1.Id);
                                var model = new EasyUserViewModel
                                {
                                    UserGroups = GroupListFiltered,
                                    User = user1
                                };

                                foreach (var version in versions)
                                {
                                    if (version.Module.ToUpper() == "TOSSM")
                                    {
                                        model.TossmVersion = version.ActualVersion;
                                    }
                                    if (version.Module.ToUpper() == "ARENA")
                                    {
                                        model.ArenaVersion = version.ActualVersion;
                                    }
                                    if (version.Module.ToUpper() == "DELTA")
                                    {
                                        model.DeltaVersion = version.ActualVersion;
                                    }
                                    if (version.Module.ToUpper() == "OMEGA")
                                    {
                                        model.OmegaVersion = version.ActualVersion;
                                    }
                                    if (version.Module.ToUpper() == "PANDA")
                                    {
                                        model.PandaVersion = version.ActualVersion;
                                    }
                                }
                                model.UpdateVersion();
                                model.UpdateUi();
                                UserList.Add(model);
                            }));
                    }

                    OnPropertyChanged("UserListFiltered");
                });
        }

        #region User list

        private ObservableCollection<EasyUserViewModel> _userList;

        public ObservableCollection<EasyUserViewModel> UserList
        {
            get => _userList ?? (_userList = new ObservableCollection<EasyUserViewModel>());
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
                    return new ObservableCollection<EasyUserViewModel>(UserList.OrderBy(t => t.UserName));
                return new ObservableCollection<EasyUserViewModel>(
                    UserList.Where(t => t.UserName.ToLower().Contains(UserFilter.ToLower())).OrderBy(t => t.UserName));
            }
        }

        #endregion

        #region User filter

        private string _userFilter;

        public string UserFilter
        {
            get => _userFilter;
            set
            {
                _userFilter = value;
                OnPropertyChanged("UserFilter");
                OnPropertyChanged("UserListFiltered");
                OnPropertyChanged("FilterEmptyButtonVisibility");
            }
        }

        public Visibility FilterEmptyButtonVisibility => String.IsNullOrEmpty(UserFilter) ? Visibility.Collapsed : Visibility.Visible;

        private RelayCommand _onClearFilter;

        public RelayCommand OnClearFilter
        {
            get
            {
                return _onClearFilter ??
                       (_onClearFilter =
                           new RelayCommand(t1 => UserFilter = "", t2 => !String.IsNullOrEmpty(UserFilter)));
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
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged("SelectedUser");

                if (SelectedUser != null)
                {
                    SelectedUser.CurrentRoleFlag = SelectedUser.RoleFlag;
                    SelectedUser.CurrentModuleFlag = SelectedUser.ModuleFlag;
                    SelectedUser.CurrentUserName = SelectedUser.UserName;
                    SelectedUser.CurrentUserGroup = SelectedUser.UserGroup;
                    SelectedUser.UpdateUi();
                }
            }
        }

        
       
        #endregion

        #region Blocker

        private BlockerModel _blockerModel;

        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel());
            set => _blockerModel = value;
        }

        #endregion

        #region User Buttons

        private RelayCommand _onAddUser;

        public RelayCommand OnAddUser
        {
            get
            {
                return _onAddUser ?? (_onAddUser = new RelayCommand(t => BlockerModel.BlockForAction(() =>
                {
                    BlockerModel.BlockForAction(
                        () =>
                        {

                            var count = 1;
                            string username;
                            while (true)
                            {
                                username = "User-" + (count++).ToString("000");
                                if (
                                    UserList.All(
                                        p =>
                                            !String.Equals(p.UserName, username,
                                                StringComparison.CurrentCultureIgnoreCase)))
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
                                    IsEnabled = true,
                                    User = new User
                                    {
                                        Id = id,
                                        Name = username
                                    },
                                    ModuleFlag = 0,
                                    RoleFlag = 0
                                };
                                UserList.Add(newUser);
                                SelectedUser = newUser;

                                OnPropertyChanged("UserListFiltered");
                            }
                            else
                            {
                                MessageBox.Show("User can not be added");
                            }
                        }
                        );

                })));
            }
            set => _onAddUser = value;
        }

        private RelayCommand _onDeleteUser;

        public RelayCommand OnDeleteUser
        {
            get
            {
                return _onDeleteUser ?? (_onDeleteUser =
                    new RelayCommand(
                        t =>
                        {
                            if (SelectedUser == null) return;
                            BlockerModel.BlockForAction(DeleteCurrentUser);
                        },
                        t => SelectedUser != null));
            }
            set => _onDeleteUser = value;
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
            set => _onResetPassword = value;
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
                            SelectedUser.Apply();
                        },
                        t => SelectedUser != null));
            }
            set => _onApply = value;
        }

        private RelayCommand _activateCommand;

        public RelayCommand ActivateCommand
        {
            get
            {
                return _activateCommand ?? (_activateCommand =
                           new RelayCommand(
                               t =>
                               {
                                   SelectedUser.CurrentRoleFlag = Math.Abs(SelectedUser.CurrentRoleFlag);
                               },
                               t => SelectedUser?.CurrentRoleFlag < 0));
            }
        }

        private RelayCommand _deactivateCommand;

        public RelayCommand DeactivateCommand
        {
            get
            {
                return _deactivateCommand ?? (_deactivateCommand =
                           new RelayCommand(
                               t =>
                               {
                                   SelectedUser.CurrentRoleFlag = -Math.Abs(SelectedUser.CurrentRoleFlag);
                               },
                               t => SelectedUser?.CurrentRoleFlag > 0));
            }
        }

        private RelayCommand _onPrepareInstall;

        public RelayCommand OnPrepareInstall
        {
            get
            {
                return _onPrepareInstall ?? (_onPrepareInstall =
                    new RelayCommand(
                        t =>
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
                                DefaultExt = ".bat",
                                Filter = "Windows Batch File (*.bat)|*.bat",
                            };

                            if (saveFileDialog.ShowDialog() == true)
                            {
                                var batFileText =
                                    "@echo off" + "\r\n" +
                                    "set file=\"%appdata%\\RISK\\ARAN\\config.xml\"\r\n" +
                                    "mkdir \"%appdata%\\RISK\\ARAN\"\r\n" +
                                    "echo  0: Basic\r\n" +
                                    "echo  1: Standard\r\n" +
                                    "echo  2: Advanced\r\n" +
                                    "echo  3: Engine\r\n" +
                                    "echo  4: EngineGeoDB\r\n" +
                                    "echo  5: ArcServer\r\n" +
                                    "echo  6: Missing\r\n" +

                                    "set header=0" + "\r\n" +
                                    "set /p header=Please, enter ESRI License Index (default: Basic):" + "\r\n" +
                                    "set a_lic=Basic" + "\r\n" +

                                    "if %header%==1 set a_lic=Standard\r\n" +
                                    "if %header%==2 set a_lic=Advanced\r\n" +
                                    "if %header%==3 set a_lic=Engine\r\n" +
                                    "if %header%==4 set a_lic=EngineGeoDB\r\n" +
                                    "if %header%==5 set a_lic=ArcServer\r\n" +
                                    "if %header%==6 set a_lic=Missing\r\n" +

                                    "set header=0" + "\r\n" +
                                    $"set host={CurrentDataContext.ServiceHost}" + "\r\n" +
                                    $"set /p header=Please, enter Server Ip Address (default: {CurrentDataContext.ServiceHost}):" + "\r\n" +
                                    "if NOT %header%==0 set host=%header%\r\n" +
                                    
                                    " >%file% echo ^<?xml version=\"1.0\" encoding=\"utf-8\"?^>" + "\r\n" +
                                    ">>%file% echo ^<Configs^>" + "\r\n" +
                                    ">>%file% echo ^<Config name=\"Default\"^>" + "\r\n" +
                                    $">>%file% echo ^<ServiceAddress^>%host%:{CurrentDataContext.ServicePort}^</ServiceAddress^>" + "\r\n" +
                                    $">>%file% echo ^<HelperAddress^>%host%:{CurrentDataContext.HelperPort}^</HelperAddress^>" + "\r\n" +
                                    ">>%file% echo ^<StorageName^>" + CurrentDataContext.StorageName + "^</StorageName^>" + "\r\n" +
                                    ">>%file% echo ^<UserId^>" + SelectedUser.Id + "^</UserId^>\r\n" +
                                    ">>%file% echo ^<License^>%a_lic%^</License^>\r\n" +
                                    ">>%file% echo ^<UseWebApiForMetadata^>" + ConfigUtil.UseWebApiForMetadata.ToString().ToLower() + "^</UseWebApiForMetadata^>" + "\r\n" +
                                    ">>%file% echo ^<WebApiAddress^>" + UserClient.WebApiAddress + "^</WebApiAddress^>" + "\r\n" +
                                    ">>%file% echo ^</Config^>\r\n" +
                                    ">>%file% echo ^</Configs^>";

                                File.WriteAllText(saveFileDialog.FileName, batFileText);

                                //var filePath = saveFileDialog.FileName;
                                //CommonUtils.Config.WriteConfigToFolder(filePath, "UserId", SelectedUser.Id);
                                //CommonUtils.Config.WriteConfigToFolder(filePath, "ServiceAddress", CurrentDataContext.ServiceAddress);
                                //CommonUtils.Config.WriteConfigToFolder(filePath, "HelperAddress", CurrentDataContext.HelperAddress);
                                //CommonUtils.Config.WriteConfigToFolder(filePath, "StorageName", CurrentDataContext.StorageName);
                                //var serial = Crypto.EncryptStringAes ( CurrentDataContext.ConnectString, "secret" + SelectedUser.Id );
                                //CommonUtils.Config.WriteConfigToFolder ( filePath, "Serial", serial );
                                //CommonUtils.Config.WriteConfigToFolder ( filePath, "License", CurrentDataContext.CurrentLicense );


                                //var writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".bat"));
                                //writer.Write(
                                //    @"set pth=%appdata%\RISK\ARAN" + 
                                //    "\r\n mkdir %pth%" +
                                //    "\r\n copy \"%~dpn0.xml\" %pth%\\config.xml");
                                //writer.Close();



                                ////var sb = new StringBuilder();
                                ////sb.AppendLine("Windows Registry Editor Version 5.00");
                                ////sb.AppendLine();
                                ////sb.AppendLine(@"[HKEY_CURRENT_USER\Software\RISK\Aran]");
                                ////sb.AppendLine("\"UserId\"=\"" + SelectedUser.Id + "\"");
                                ////sb.AppendLine("\"ServiceAddress\"=\"" + CurrentDataContext.ServiceAddress + "\"");
                                ////sb.AppendLine("\"HelperAddress\"=\"" + CurrentDataContext.HelperAddress + "\"");
                                ////sb.AppendLine("\"StorageName\"=\"" + CurrentDataContext.StorageName + "\"");


                                //////sb.AppendLine("\"Serial\"=\"" + 
                                //////    Crypto.EncryptStringAes(CurrentDataContext.ConnectString,
                                //////                            "secret" + SelectedUser.Id) + 
                                //////    "\"");

                                ////try
                                ////{
                                ////    var writer = new StreamWriter(saveFileDialog.FileName);
                                ////    writer.Write(sb.ToString());
                                ////    writer.Close();
                                ////}
                                ////catch (Exception)
                                ////{
                                ////    MessageBox.Show("File can not be created. " +
                                ////                    "May be it is in use by another process " +
                                ////                    "or you do not have permission to create " +
                                ////                    "files in specified folder.",
                                ////        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                ////}
                            }
                        },
                        t => SelectedUser != null));
            }
            set => _onPrepareInstall = value;
        }

        public RelayCommand OnReport
        {
            get
            {
                return _onReport ?? (_onReport = new RelayCommand(
                    t =>
                    {


                        CultureInfo ci = CultureInfo.InvariantCulture;
                        Thread.CurrentThread.CurrentCulture = ci;
                        Thread.CurrentThread.CurrentUICulture = ci;

                        var dialog = new SaveFileDialog
                        {
                            Title = "Save Report",
                            Filter = "HTML Files (*.htm)|*.htm|All Files (*.*)|*.*"
                        };

                        if (dialog.ShowDialog() == true)
                        {
                            BlockerModel.BlockForAction(
                                () =>
                                {
                                    var report = new UserReportTemplate
                                    {
                                        Users = UserList.ToList()
                                    };


                                    var content=report.TransformText();
                                    File.WriteAllText(dialog.FileName, content);

                                    try
                                    {
                                        Process.Start(dialog.FileName);
                                    }
                                    catch (Exception ex)
                                    {
                                        LogManager.GetLogger(typeof(ModifyRegistry)).Error(ex, $"Can not open {dialog.FileName} in default browser.");
                                        MessageBox.Show($"Can not open {dialog.FileName} in default browser.",
                                            "Can Not Open", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }

                                });

                        }
                    }));
            }

            set => _onReport = value;
        }

        #endregion

        #region Change access buttons

        private RelayCommand _onNoAccess;
        public RelayCommand OnNoAccess
        {
            get
            {
                return _onNoAccess ?? (_onNoAccess = new RelayCommand(
                                                 t =>
                                                     {
                                                         var win = t as EasyUserGroupControl;
                                                         if (win == null) return;

                                                         foreach (var model in win.FilteredFeatureList.SelectedItems.OfType<SingleFeatureAccessModel>())
                                                         {
                                                             model.IsWrite = false;
                                                             model.IsRead = false;
                                                         }
                                                 },
                                                 t => SelectedGroup != null));
            }
            set => _onNoAccess = value;
        }

        private RelayCommand _onReadOnly;
        public RelayCommand OnReadOnly
        {
            get
            {
                return _onReadOnly ?? (_onReadOnly = new RelayCommand(
                                                 t =>
                                                 {
                                                     var win = t as EasyUserGroupControl;
                                                     if (win == null) return;

                                                     foreach (var model in win.FilteredFeatureList.SelectedItems.OfType<SingleFeatureAccessModel>())
                                                     {
                                                         model.IsWrite = false;
                                                         model.IsRead = true;
                                                     }
                                                 },
                                                 t => SelectedGroup != null));
            }
            set => _onReadOnly = value;
        }

        private RelayCommand _onFull;
        public RelayCommand OnFull
        {
            get
            {
                return _onFull ?? (_onFull = new RelayCommand(
                                                 t =>
                                                 {
                                                     var win = t as EasyUserGroupControl;
                                                     if (win == null) return;

                                                     foreach (var model in win.FilteredFeatureList.SelectedItems.OfType<SingleFeatureAccessModel>())
                                                     {
                                                         model.IsWrite = true;
                                                         model.IsRead = true;
                                                     }
                                                 },
                                                 t => SelectedGroup != null));
            }
            set => _onFull = value;
        }

        #endregion

        #region Group buttons

     
        private RelayCommand _onAddUserGroup;
        public RelayCommand OnAddUserGroup
        {
            get { return _onAddUserGroup??(_onAddUserGroup=new RelayCommand(
                t =>
                {
                    BlockerModel.BlockForAction(
                        () =>
                        {

                            var count = 1;
                            string groupName;
                            while (true)
                            {
                                groupName = "Group-" + (count++).ToString("000");
                                if (GroupListFiltered.All(p => !String.Equals(p.GroupName, groupName, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    break;
                                }
                            }
                            //
                            var id = CurrentDataContext.CurrentNoAixmDataService.CreateGroup(groupName);

                            if (id > -1)
                            {
                                var newUserGroup = new UserGroup
                                {
                                    Id = id,
                                    Name = groupName
                                };
                                var newGroupModel = new EasyUserGroupViewModel
                                {
                                    UserGroup = newUserGroup
                                };

                                Application.Current.Dispatcher.Invoke(
                                    DispatcherPriority.Background,
                                    (Action) (() =>
                                    {
                                        GroupListFiltered.Add(newGroupModel);
                                        SelectedGroup = newGroupModel;
                                    }));
                            }
                            else
                            {
                                MessageBox.Show("Group can not be added");
                            }

                            Application.Current.Dispatcher.Invoke(
                               DispatcherPriority.Background,
                               (Action)(UpdateGroups));
                        }
                        );
                }
                )); }
        }

        private RelayCommand _onDeleteUserGroup;
        public RelayCommand OnDeleteUserGroup
        {
            get { return _onDeleteUserGroup??(_onDeleteUserGroup=new RelayCommand(
                t=>
                        {
                            if (SelectedGroup == null) return;
                            BlockerModel.BlockForAction(DeleteCurrentUserGroup);
                        },
                    t => SelectedGroup != null));  
            }
        }


        private RelayCommand _onGroupApply;
        public RelayCommand OnGroupApply
        {
            get
            {
                return _onGroupApply ?? (_onGroupApply = new RelayCommand(
                t =>
                {
                    if (SelectedGroup == null) return;
                    SelectedGroup.Apply();
                    UpdateGroups();
                    if (SelectedUser != null)
                    {
                        SelectedUser.UpdateUi();
                    }
                },
                t => SelectedGroup != null));
            }
        }

       

        #endregion

        #region Groups

        private ObservableCollection<EasyUserGroupViewModel> _groupListFiltered;
        public ObservableCollection<EasyUserGroupViewModel> GroupListFiltered
        {
            get => _groupListFiltered ?? (_groupListFiltered = new ObservableCollection<EasyUserGroupViewModel>());
            set
            {
                _groupListFiltered = value;
                OnPropertyChanged("GroupListFiltered");
            }
        }


        private EasyUserGroupViewModel _selectedGroup;
        private RelayCommand _onReport;

        public EasyUserGroupViewModel SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                OnPropertyChanged("SelectedGroup");
                if (SelectedGroup != null)
                {
                    SelectedGroup.UpdateUi();
                }
            }
        }


        public void UpdateGroups()
        {
            foreach (var user in UserList)
            {
                user.UpdateUi();
            }
        }

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

        public void DeleteCurrentUserGroup()
        {
            if (MessageBox.Show("You are going to delete selected user group. Are you sure?",
                                "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) 
                                != MessageBoxResult.Yes)
            {
                return;
            }

            var id = SelectedGroup.Id;

            Application.Current.Dispatcher.Invoke(
                                  DispatcherPriority.Background,
                                  (Action)(() =>
                                  {
                                      GroupListFiltered.Remove(SelectedGroup);
                                      CurrentDataContext.CurrentNoAixmDataService.DeleteGroupById(id);

                                      var affectedUsers = UserList.Where(t => t.UserGroup != null && t.UserGroup.Id == id).ToList();
                                      foreach (var userViewModel in affectedUsers)
                                      {
                                          userViewModel.User.UserGroup = null;
                                      }

                                      SelectedGroup = null;
                                      UpdateGroups();
                                  }));
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
