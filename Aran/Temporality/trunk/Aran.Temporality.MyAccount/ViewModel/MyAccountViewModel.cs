using System.Windows;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.MyAccount.View;
using Aran.Temporality.UserManager.ViewModel;
using Aran.Temporality.Util;
using MvvmCore;

namespace Aran.Temporality.MyAccount.ViewModel
{
    public class MyAccountViewModel : ViewModelBase
    {
        public MyAccountViewModel()
        {
            var user1 = CurrentDataContext.CurrentUser;
            EasyUserViewModel = new EasyUserViewModel
                                    {
                                        IsEnabled = false,
                                        UserName = user1.Name,
                                        Id = user1.Id,
                                        RoleFlag = user1.RoleFlag,
                                        ModuleFlag = user1.ModuleFlag
                                    };
            
            //init access rules
            EasyUserViewModel.SetCurrentAccess();
            foreach (var f in EasyUserViewModel.CurrentFeatureList)
            {
                f.IsEnabled = false;
            }
        }


        public EasyUserViewModel EasyUserViewModel { get; set; }

        private RelayCommand _resetPasswordCommand;
        public RelayCommand ResetPasswordCommand
        {
            get
            {
                return _resetPasswordCommand??(_resetPasswordCommand=new RelayCommand(
                    t=>
                        {
                            var control = t as MyAccountViewControl;
                            if (control==null) return;

                            if (!string.IsNullOrEmpty(CurrentDataContext.CurrentPassword))
                            {
                                if (CurrentDataContext.CurrentPassword != HashUtil.ComputeHash(
                                    control.OldPasswordBox.Password))
                                {
                                    MessageBox.Show("Old password is not correct", "Can not change password",
                                                MessageBoxButton.OK, MessageBoxImage.Warning);
                                    return;
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(control.OldPasswordBox.Password))
                                {
                                    MessageBox.Show("Old password is not correct", "Can not change password",
                                               MessageBoxButton.OK, MessageBoxImage.Warning);
                                    return;
                                }
                            }

                            if (control.NewPasswordBox.Password != control.NewPasswordBox2.Password)
                            {
                                MessageBox.Show("New passwords are not equal", "Can not change password",
                                                MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }

                            if (!CurrentDataContext.CurrentNoAixmDataService.ChangeMyPassword(
                                CurrentDataContext.CurrentUser.Id,
                                HashUtil.ComputeHash( control.OldPasswordBox.Password),
                                HashUtil.ComputeHash(control.NewPasswordBox.Password)))
                            {
                                 MessageBox.Show("Error occured while changing password", "Can not change password",
                                                MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Password was successfully changed. Please re-login.", "Change password",
                                               MessageBoxButton.OK, MessageBoxImage.Information);
                                CurrentDataContext.CurrentPassword =
                                    HashUtil.ComputeHash(control.NewPasswordBox.Password);
                            }
                        },
                    t=>
                        {
                            var control = t as MyAccountViewControl;
                            if (control == null)
                            {
                                return false;
                            }

                            if (!string.IsNullOrEmpty(CurrentDataContext.CurrentPassword))
                            {
                                if (CurrentDataContext.CurrentPassword != HashUtil.ComputeHash(
                                    control.OldPasswordBox.Password))
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(control.OldPasswordBox.Password))
                                {
                                    return false;
                                }
                            }

                            return control.NewPasswordBox.Password == control.NewPasswordBox2.Password;
                        }));
            }
            set { _resetPasswordCommand = value; }
        }
    }
}
