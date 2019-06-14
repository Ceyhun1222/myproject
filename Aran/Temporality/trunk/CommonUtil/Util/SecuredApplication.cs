using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Aran.Temporality.Common.Aim.Service;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.View;
using Aran.Temporality.CommonUtil.ViewModel;
using Aran.Temporality.Internal.Remote.ClientServer;
using Microsoft.Win32;

namespace Aran.Temporality.CommonUtil.Util
{
    public class SecuredApplication : Application
    {
        public String[] StartupArgs { get; set; }
        public Action MainAction { get; set; }

        #region Resources init/release procedures

        public virtual void Init()
        {
        }

        public virtual void Release()
        {
            ConnectionProvider.Close();
        }

        #endregion

        #region Event handlers

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                Release();
                base.OnExit(e);
            }
            catch (Exception exception)
            {
                LogManager.GetLogger(typeof(SecuredApplication)).Error(exception, "Application failed to exit");
            }
        }

        [STAThread]
        protected override void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);
            try
            {
                StartupArgs = e.Args;

                Init();

                //uncomment to crete default tables
                //AimServiceFactory.Setup();

                ConnectionProvider.MainAction = MainAction;//what to do after login succeeded
                ConnectionProvider.ShutdownAction = Shutdown;//what to do after login failed

                //ConnectionProvider.OpenMultiple();
                
                var connections = ConnectionProvider.GetConnectionStrings();
                if (connections.Count == 0 || connections.Count == 1)
                {
                    ConnectionProvider.Open();
                }
                else
                {
                    // If arguments already contains StorageName to open connection
                    if (StartupArgs.FirstOrDefault()?.StartsWith("eaip://OpenDocument/storage=") == true)
                    {
                        string storageName = StartupArgs.FirstOrDefault()?.Replace("eaip://OpenDocument/storage=", "").Split('&').FirstOrDefault();
                        string connectionName = ConnectionProvider.GetConnectionStringByStorageName(storageName);
                        // Trying to detect that connection already exist in the list
                        if (connections.Contains(connectionName))
                        {
                            ConnectionProvider.Open(connectionName);
                            return;
                        }
                    }

                    var configSelectWindow = new ConfigSelectWindow();

                    if (!(configSelectWindow.DataContext is ConfigSelectModel configSelectModel))
                    {
                        configSelectModel = new ConfigSelectModel();
                        configSelectWindow.DataContext = configSelectModel;
                    }

                    foreach (var setting in connections)
                    {
                        configSelectModel.Settings.Add(setting);
                    }

                    configSelectModel.Setting = connections.First();

                    configSelectModel.CloseAction = (window) =>
                    {
                        window?.Close();
                        Shutdown();
                    };

                    configSelectModel.SelectAction = (connection, window) =>
                    {
                        window?.Hide();

                        var clientConfig = new ClientConfig();
                        clientConfig.Load();

                        var configs = clientConfig.Settings.Values.ToList();

                        if (configs.Count > 1)
                        {
                            var defaultId = configs.Select(x => x.ConfigName).ToList().IndexOf(connection);
                            if (defaultId > 0)
                            {
                                var current = configs[defaultId];
                                configs.RemoveAt(defaultId);
                                configs.Insert(0, current);

                                clientConfig.Settings.Clear();
                                foreach (var config in configs)
                                {
                                    clientConfig.Settings.Add(config.ConfigName, config);
                                }
                                clientConfig.Save();
                            }
                        }

                        return ConnectionProvider.Open(connection);
                    };

                    configSelectWindow.ShowDialog();

                    if (!configSelectModel.IsSuccess)
                        throw new Exception("Config selection failed!");
                }

                //Application.Current.Dispatcher.BeginInvoke(
                //    DispatcherPriority.Background,
                //    (Action) (
                //                 () =>
                //                     {

                //                     }));
            }
            catch (Exception exception)
            {
                LogManager.GetLogger(typeof(SecuredApplication)).Error(exception, "Application failed to start");
                throw;
            }
        }

        #endregion
    }

}
