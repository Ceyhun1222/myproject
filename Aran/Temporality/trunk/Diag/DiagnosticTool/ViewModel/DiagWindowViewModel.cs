using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Config;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using Aran.Temporality.Internal.Remote.ClientServer;
using ESRI.ArcGIS.esriSystem;
using Microsoft.Win32;
using MvvmCore;

namespace DiagnosticTool.ViewModel
{
    public class DiagWindowViewModel : ViewModelBase
    {
        private RelayCommand _connectCommand;
        private string _message;
        private BlockerModel _blockerModel;

        private static readonly HelperClient HelperClient = new HelperClient();

        private void Report(String s)
        {
            Message += "\n● " + s;
        }

        public BlockerModel BlockerModel
        {
            get { return _blockerModel ?? (_blockerModel = new BlockerModel()); }
        }

        public RelayCommand ConnectCommand
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new RelayCommand(
                     t =>
                     {
                         BlockerModel.BlockForAction(
                             () =>
                             {

                                 Message = "● Started at [" + DateTime.Now + "]";

                                 var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                                 try
                                 {
                                     File.Delete(path + "\\delete.me");
                                 }
                                 catch 
                                 {
                                 }

                                 try
                                 {
                                    File.CreateText(path + "\\delete.me");
                                    Report("Started from [" + path + "], write access rights are granted, auto-update system will work fine");
                                 }
                                 catch (Exception exception)
                                 {
                                     Report("Started from [" + path + "], write access rights are NOT granted, auto-update system WILL FAIL");
                                 }

                                 try
                                 {
                                     File.Delete(path + "\\delete.me");
                                 }
                                 catch
                                 {
                                 }


                                 ////init esri license in the same thread
                                 if (LicenseInitializer.Instance.InitializeApplication(
                                         new[] { esriLicenseProductCode.esriLicenseProductCodeBasic },
                                         new esriLicenseExtensionCode[] { }))
                                 {
                                     Report("esri Basic license found");
                                 }
                                 else
                                 {
                                     Report("NO esri Basic license found");
                                 }
                                 if (LicenseInitializer.Instance.InitializeApplication(
                                         new[] { esriLicenseProductCode.esriLicenseProductCodeStandard },
                                         new esriLicenseExtensionCode[] { }))
                                 {
                                     Report("esri Standard license found");
                                 }
                                 else
                                 {
                                     Report("NO esri Standard license found");
                                 }
                                 if (LicenseInitializer.Instance.InitializeApplication(
                                         new[] { esriLicenseProductCode.esriLicenseProductCodeAdvanced },
                                         new esriLicenseExtensionCode[] { }))
                                 {
                                     Report("esri Advances license found");
                                 }
                                 else
                                 {
                                     Report("NO esri Advances license found");
                                 }


                                 if (LicenseInitializer.Instance.InitializeApplication(
                                         new[] { esriLicenseProductCode.esriLicenseProductCodeEngine },
                                         new esriLicenseExtensionCode[] { }))
                                 {
                                     Report("esri Engine license found");
                                 }
                                 else
                                 {
                                     Report("NO esri Engine license found");
                                 }


                                 if (LicenseInitializer.Instance.InitializeApplication(
                                         new[] { esriLicenseProductCode.esriLicenseProductCodeArcServer },
                                         new esriLicenseExtensionCode[] { }))
                                 {
                                     Report("esri ArcServer license found");
                                 }
                                 else
                                 {
                                     Report("NO esri ArcServer license found");
                                 }

                                 if (LicenseInitializer.Instance.InitializeApplication(
                                         new[] { esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB },
                                         new esriLicenseExtensionCode[] { }))
                                 {
                                     Report("esri GeoDB license found");
                                 }
                                 else
                                 {
                                     Report("NO esri GeoDB license found");
                                 }

                                 if (!ConnectionProvider.InitClientSettings())
                                 {
                                     Report("System can not find settings in registry. To fix it:\n" +
                                            "   1) Make sure TOSS/TOSSM is installed on this PC\n" +
                                            "   2) Install certificate if it does not exist on this PC (ask IT Administrator for permission to install it)\n" +
                                            "   3) Ask TOSS Administrator to create new user if user does not exist in TOSS\n" +
                                            "   4) Ask TOSS Administrator to create and export installation registry file\n" +
                                            "   5) Ask IT Administrator for permission to install mentioned above registry file on this PC");

                                     ModifyRegistry registry = null;
                                     try
                                     {
                                         registry = new ModifyRegistry
                                         {
                                             ShowError = false,
                                             BaseRegistryKey = Registry.CurrentUser,
                                             SubKey = @"SOFTWARE\RISK\Aran"
                                         };
                                         Report(@"Opened registry key [HKEY_CURRENT_USER\SOFTWARE\RISK\Aran]");
                                     }
                                     catch (Exception exception)
                                     {
                                         Report( @"Can not open registry key [HKEY_CURRENT_USER\SOFTWARE\RISK\Aran], exception is [" + exception.Message + "]");
                                         return;
                                     }

                                     try
                                     {
                                         CurrentDataContext.UserId = Convert.ToInt32(registry.Read("UserId"));
                                         Report(@"Opened value UserId [" + CurrentDataContext.UserId + "]");
                                     }
                                     catch (Exception exception)
                                     {
                                         Report(@"Can not open value UserId, exception is [" + exception.Message + "]");
                                         return;
                                     }

                                     try
                                     {
                                         CurrentDataContext.StorageName = registry.Read("StorageName");
                                         Report(@"Opened value StorageName [" + CurrentDataContext.StorageName + "]");
                                     }
                                     catch (Exception exception)
                                     {
                                         Report(@"Can not open value StorageName, exception is [" + exception.Message + "]");
                                         return;
                                     }

                                     try
                                     {
                                         CurrentDataContext.ServiceAddress = registry.Read("ServiceAddress");
                                         Report(@"Opened value ServiceAddress [" + CurrentDataContext.ServiceAddress + "]");
                                     }
                                     catch (Exception exception)
                                     {
                                         Report(@"Can not open value StorageName, exception is [" + exception.Message +  "]");
                                         return;
                                     }

                                     try
                                     {
                                         CurrentDataContext.HelperAddress = registry.Read("HelperAddress");
                                         Report(@"Opened value HelperAddress [" + CurrentDataContext.HelperAddress + "]");
                                     }
                                     catch (Exception exception)
                                     {
                                         Report(@"Can not open value StorageName, exception is [" + exception.Message +  "]");
                                         return;
                                     }
                                 }


                                 Report("Current User ID is [" + CurrentDataContext.UserId + "]");
                                 Report("Main Service Address is [" + CurrentDataContext.ServiceAddress + "]");
                                 Report("Helper Storage Address is [" + CurrentDataContext.HelperAddress + "]");
                                 Report("Storage Name is [" + CurrentDataContext.StorageName + "]");

                                 try
                                 {
                                     HelperClient.Open(CurrentDataContext.HelperAddress);
                                     var serverTime = HelperClient.Proxy.GetServerTime(CurrentDataContext.UserId);
                                     Report("Server time is [" + serverTime + "]");
                                     var d = Math.Abs((serverTime - DateTime.Now).TotalMinutes);
                                     if (d > 2)
                                     {
                                         Report("Time difference can make impossible for server to trust local certificate, " +
                                                "consider time adjustment if time difference more than 5 minutes, current difference is " + d + " minutes");
                                     }
                                 }
                                 catch (Exception exception)
                                 {
                                     Report("System can not connect to server, it seems server is offline");
                                     return;
                                 }

                                 ConnectionProvider.MainAction = () => { Report("Successfully connected"); };
                                 ConnectionProvider.ShutdownAction = () => { };

                                 try
                                 {
                                     CurrentDataContext.CurrentUserName = HelperClient.Proxy.GetUserName(CurrentDataContext.UserId);
                                     CurrentDataContext.IsUserSecured = HelperClient.Proxy.IsUserSecured(CurrentDataContext.UserId);

                                     Report("Current User is [" + CurrentDataContext.CurrentUserName + ", " + (CurrentDataContext.IsUserSecured ? "password required" : "NO password set yet") + "]");
                                 }
                                 catch (Exception exception)
                                 {
                                     Report("System tries to identify current user but can not get data from helper service: " + exception.Message);
                                 }

                                 try
                                 {
                                     var loggedIn=CurrentDataContext.Login();
                                     Report("Secured connection established, certificate is present" + (loggedIn?", logged in by default":", password required to log in"));
                                 }
                                 catch (Exception exception)
                                 {
                                     Report("Secured connection can not be established, if time is correct, it seems certificate is missing, install it (ask IT Administrator for permission to do that): " + exception.Message);
                                 }
                                 
                                 var thread = new Thread(() =>
                                 {
                                     try
                                     {
                                         ConnectionProvider.Open();
                                     }
                                     catch (Exception exception)
                                     {
                                         Report("Exception while logging in: " + exception.Message);
                                     }

                                     if (CurrentDataContext.IsUserSecured)
                                     {
                                         Report("Password hash was specified [" + CurrentDataContext.CurrentPassword + "]");
                                     }

                                     Report(CurrentDataContext.CurrentUser == null
                                         ? "Login failed, if you want to reset password use sql command [ UPDATE \"User\" SET password = NULL WHERE id = " +CurrentDataContext.UserId+" ]"
                                         : "Login was successful");
                                 });
                                 thread.SetApartmentState(ApartmentState.STA);
                                 thread.Start();


                             }
                             );

                     },
                     t => true));
            }
        }

        public String Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }
    }
}
