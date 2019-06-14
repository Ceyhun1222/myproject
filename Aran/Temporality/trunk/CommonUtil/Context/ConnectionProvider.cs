using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using Aran.Temporality.Common.Aim.Service;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.View;
using Aran.Temporality.CommonUtil.ViewModel;
using Aran.Temporality.Internal.Remote.ClientServer;
using Microsoft.Win32;
using System.Web;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Logging;
using System.Collections.Generic;
using WebApiClient;

namespace Aran.Temporality.CommonUtil.Context
{
    public static class ConnectionProvider
    {
        public static Action MainAction { get; set; }
        public static Action ShutdownAction { get; set; }

        #region Status

        public static Action OnStatusChanged { get; set; }
        public static Action OnServerTimeChanged { get; set; }
        public static DateTime ServerTime { get; set; }

        private static string _serverTimeString;
        public static string ServerTimeString
        {
            get { return _serverTimeString; }
            set
            {
                if (_serverTimeString == value) return;
                _serverTimeString = value;
                if (OnServerTimeChanged != null)
                {
                    OnServerTimeChanged();
                }
            }
        }

        private static bool _serverIsOffline;
        public static bool ServerIsOffline
        {
            get { return _serverIsOffline; }
            set
            {
                if (_serverIsOffline == value) return;
                _serverIsOffline = value;
                if (OnStatusChanged != null)
                {
                    OnStatusChanged();
                }
            }
        }

        #endregion

        private static bool _isRunning = true;

        #region Private fields

        //number of attempts allowed for login window
        private static int _tryCount = 3;

        private static readonly HelperClient HelperClient = new HelperClient();

        #endregion

        #region General logic

        private static void DoShutdown(Window window)
        {
            window?.Close();
            Shutdown();
        }

        private static void Shutdown()
        {
            ShutdownAction?.Invoke();
        }

        private static void ShowLoginWindow()
        {
            var loginWindow = new LoginWindow();
            var loginModel = loginWindow.DataContext as LoginModel;
            if (loginModel == null)
            {
                loginModel = new LoginModel();
                loginWindow.DataContext = loginModel;
            }
            loginModel.User = CurrentDataContext.CurrentUserName;
            loginModel.ConnectAction = Login;
            loginModel.CloseAction = DoShutdown;
            loginWindow.ShowDialog();
        }

        private static void Login(string password, Window loginWindow)
        {
            //init password from UI
            CurrentDataContext.CurrentPassword = password;

            LogManager.GetLogger(typeof(ConnectionProvider)).Info("CHECKPOINT number 8");
            //try login with specified username/password
            if (CurrentDataContext.Login())
            {
                LogManager.GetLogger(typeof(ConnectionProvider)).Info("CHECKPOINT number 9");
                LogManager.GetLogger(typeof(ConnectionProvider)).Info(
                    $"Login succeed. UserId: {CurrentDataContext.UserId}, username: {CurrentDataContext.CurrentUserName}");
                //login was successful
                loginWindow?.Hide();

                //try
                //{
                //    CurrentDataContext.InitDataConnection();
                //}
                //catch (Exception)
                //{
                //    MessageBox.Show("System can not retrieve data from server using specified credentials. It is possible that service is down. " +
                //                    "If problem persists contact your system administrator.",
                //                    "Can not retrieve data",
                //                    MessageBoxButton.OK,
                //                    MessageBoxImage.Error);

                //    Shutdown();
                //    return;
                //}


                LogManager.GetLogger(typeof(ConnectionProvider)).Info("CHECKPOINT number 10");
                var pingThread = new Thread(
                    () =>
                    {
                        LogManager.GetLogger(typeof(ConnectionProvider)).Info("START pingThread");
                        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                        while (_isRunning)
                        {
                            Thread.Sleep(500);
                            try
                            {
                                ServerTime = HelperClient.Proxy.GetServerTime(CurrentDataContext.UserId);
                                ServerIsOffline = false;
                                ServerTimeString = ServerTime.ToString("HH:mm");
                            }
                            catch (Exception exception)
                            {
                                LogManager.GetLogger(typeof(ConnectionProvider)).Info("Ping failed. Helper service is down.");
                                LogManager.GetLogger(typeof(ConnectionProvider)).Error(exception, exception.Message);
                                ServerIsOffline = true;
                            }
                        }
                    })
                {
                    IsBackground = true,
                    Priority = ThreadPriority.BelowNormal,
                };
                pingThread.Start();

                LogManager.GetLogger(typeof(ConnectionProvider)).Info("CHECKPOINT number 11");

                MainAction?.Invoke();

                _isRunning = false;

                loginWindow?.Close();

                Shutdown();
                return;
            }

            LogManager.GetLogger(typeof(ConnectionProvider)).Info("CHECKPOINT number 12");
            //login was unsuccessful
            if (_tryCount-- > 0)
            {
                LogManager.GetLogger(typeof(ConnectionProvider)).Info(
                    $"Unsuccessful login attempt. Password is wrong. UserId: {CurrentDataContext.UserId}, username: {CurrentDataContext.CurrentUserName}");
                MessageBox.Show("Password is wrong. Please type it again. After several unsuccessful attempts application will be closed.", "Login error", MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);

                loginWindow?.Hide();

                ShowLoginWindow();

                loginWindow?.Close();
            }
            else
            {
                LogManager.GetLogger(typeof(ConnectionProvider)).Info(
                    $"Password is wrong. Login error. UserId: {CurrentDataContext.CurrentUser}, username: {CurrentDataContext.CurrentUserName}");
                MessageBox.Show("Password is wrong. Application will be closed.", "Login error", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                loginWindow?.Close();
                Shutdown();
            }
        }

        #endregion

        #region Public methods

        public static bool InitClientSettings(string connection = null)
        {
            var config = new ClientConfig();
            config.Load();

            ClientConfigObject setting;

            if (connection == null)
                setting = config.Settings.FirstOrDefault().Value;
            else
            {
                if (!config.Settings.ContainsKey(connection))
                    return false;
                setting = config.Settings[connection];
            }

            if (setting == null)
                return false;

            CurrentDataContext.CurrentLicense = setting.License;
            CurrentDataContext.UserId = setting.UserId;
            CurrentDataContext.StorageName = setting.StorageName;
            CurrentDataContext.ServiceAddress = setting.ServiceAddress;
            CurrentDataContext.HelperAddress = setting.HelperAddress;

            ConfigUtil.UseEsri = setting.License != EsriLicense.Missing;

            ConfigUtil.UseWebApiForMetadata = setting.UseWebApiForMetadata;

            UserClient.WebApiAddress = setting.WebApiAddress;

            return CurrentDataContext.UserId != 0 && CurrentDataContext.ServiceAddress != null
                && CurrentDataContext.HelperAddress != null && !String.IsNullOrEmpty(CurrentDataContext.StorageName);
        }

        public static bool InitServerSettings(string configName = null)
        {

            var config = new ServerConfig();
            config.Load();

            ServerConfigObject setting;

            if (string.IsNullOrWhiteSpace(configName))
            {
                setting = config.Settings.FirstOrDefault().Value;
            }
            else if (config.Settings.ContainsKey(configName))
            {

                setting = config.Settings[configName];
            }
            else
            {
                return false;
            }

            if (setting == null)
                return false;

            ApplyServerSettings(setting);

            return true;
        }

        public static void ApplyServerSettings(ServerConfigObject setting)
        {
            CurrentDataContext.CurrentLicense = setting.License;
            CurrentDataContext.DllRepo = setting.DllRepo;

            if (CurrentDataContext.DllRepo != null)
                ConfigUtil.DllRepo = CurrentDataContext.DllRepo;

            CurrentDataContext.ConnectString = setting.ConnectionString;
            CurrentDataContext.ServicePort = setting.ServicePort;
            CurrentDataContext.HelperPort = setting.HelperPort;
            CurrentDataContext.ExternalPort = setting.ExternalPort;
            
            ConfigUtil.NoDataServiceAddress = setting.DbAddress;
            ConfigUtil.NoDataServicePort = setting.DbPort.ToString();
            ConfigUtil.NoDataUser = setting.DbUser;
            ConfigUtil.NoDataPassword = setting.DbPassword;
            ConfigUtil.NoDataDatabase = setting.DbName;

            ConfigUtil.RepositoryType = setting.RepositoryType;
            ConfigUtil.MongoServerAddress = setting.MongoServerAddress;
            ConfigUtil.MongoServerPort = setting.MongoServerPort;
            ConfigUtil.MongoUser = setting.MongoUser;
            ConfigUtil.MongoPassword = setting.MongoPassword;
            ConfigUtil.MongoCreateGeoIndex = setting.MongoCreateGeoIndex;

            ConfigUtil.RedisConnectionString = setting.RedisConnectionString;
            ConfigUtil.UseRedisForMetaCache = setting.UseRedisForMetaCache;

            ConfigUtil.UseEsri = setting.License != EsriLicense.Missing;
        }

        public static bool Open(string connection = null)
        {
            LogManager.GetLogger(typeof(ConnectionProvider)).Info("Start Init Settings");

            if (!InitClientSettings(connection))
            {
                LogManager.GetLogger(typeof(ConnectionProvider)).Error("System can not find settings in registry.");
                ErrorHandling("System can not find settings in registry. Please reinstall software.\n" +
                                "If problem persists contact your system administrator.",
                                "Software was not installed correctly");
                return false;
            }

            LogManager.GetLogger(typeof(ConnectionProvider)).Info("End Init Settings");

            return OpenConnections();
        }

        public static List<string> GetConnectionStrings()
        {
            var config = new ClientConfig();
            config.Load();
            return config.Settings.Keys.ToList();
        }

        public static string GetConnectionStringByStorageName(string storageName)
        {
            try
            {
                var config = new ClientConfig();
                config.Load();
                return config.Settings?
                             .FirstOrDefault(x => x.Value.StorageName.ToString() == storageName)
                             .Key;
            }
            catch (Exception exception)
            {
                LogManager.GetLogger(typeof(ConnectionProvider)).Error(exception, exception.Message);
                ErrorHandling("System can not connect to server using specified credentials (" +
                              exception.Message +
                              "). " +
                              "If problem persists contact your system administrator.",
                    "Server is not accessible");
                return null;
            }
        }

        private static bool OpenConnections()
        {
            try
            {
                LogManager.GetLogger(typeof(ConnectionProvider)).Info("CHECKPOINT number 1");
                HelperClient.Open(CurrentDataContext.HelperAddress);
                LogManager.GetLogger(typeof(ConnectionProvider)).Info("CHECKPOINT number 2");
                ServerTime = HelperClient.Proxy.GetServerTime(CurrentDataContext.UserId);
            }
            catch (Exception exception)
            {
                LogManager.GetLogger(typeof(ConnectionProvider)).Info("Ping failed. Helper service is down.");
                LogManager.GetLogger(typeof(ConnectionProvider)).Error(exception, exception.Message);
                ErrorHandling("System can not connect to server. It seems server is offline.",
                                "Server is not accessible");
                return false;
            }

            try
            {
                LogManager.GetLogger(typeof(ConnectionProvider)).Info("CHECKPOINT number 3");
                CurrentDataContext.CurrentUserName = HelperClient.Proxy.GetUserName(CurrentDataContext.UserId);

                LogManager.GetLogger(typeof(ConnectionProvider)).Info("CHECKPOINT number 4");
                CurrentDataContext.IsUserSecured = HelperClient.Proxy.IsUserSecured(CurrentDataContext.UserId);

                if (string.IsNullOrEmpty(CurrentDataContext.CurrentUserName))
                {
                    throw new Exception("Null user name");
                }
            }
            catch (Exception exception)
            {
                LogManager.GetLogger(typeof(ConnectionProvider)).Error(exception, exception.Message);
                ErrorHandling("System can not connect to server using specified credentials (" +
                    exception.Message +
                                "). " +
                                "If problem persists contact your system administrator.",
                                "Server is not accessible");
                return false;
            }
            
            if (CurrentDataContext.IsUserSecured)
            {
                LogManager.GetLogger(typeof(ConnectionProvider)).Info("CHECKPOINT number 5");
                ShowLoginWindow();
            }
            else
            {
                //no password is required for this user, connect without delay
                LogManager.GetLogger(typeof(ConnectionProvider)).Info("CHECKPOINT number 6");
                Login(null, null);
            }

            return true;
        }

        public static void Close()
        {
            //_isRunning = false;
            if (CurrentDataContext.CurrentService == null) return;
            if (!ServerIsOffline)
            {
                CurrentDataContext.CurrentService.Dispose();
            }
        }

        private static void ErrorHandling(string text, string caption)
        {
            if (HttpRuntime.AppDomainAppId == null)
                MessageBox.Show(text,
                            caption,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

            Shutdown();
            if (HttpRuntime.AppDomainAppId != null)
                throw new Exception(text);
        }

        #endregion
    }
}
