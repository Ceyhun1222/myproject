using ChartManagerTools.View;
using ChartManagerTools.ViewModel;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Threading;
using System.Windows.Threading;
using AmdbManager;
using AmdbManager.Logging;
using AmdbManager.ViewModel;
using AerodromeManager.AmdbService;
using Aerodrome.Metadata;

namespace ChartManagerTools
{
    public static class GlobalController
    {
        private static readonly ViewModelLocator _viewModelLocator;

        static GlobalController()
        {
            _viewModelLocator = new ViewModelLocator();
            LogManager.Configure("ChartManager.log", "ChartManager_Errorlogs.log", LogLevel.Info);
            var preFix = new string('#', 30);
            LogManager.GetLogger(typeof(GlobalController)).Info($"{preFix} New session is created {preFix}");
        }

        public static async Task PublishAsync(IApplication application, bool setLock = true)
        {
            LogManager.GetLogger(typeof(GlobalController)).InfoWithMemberName($"Started");
            if (ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.IsReadOnly)
            {
                _viewModelLocator.ShowMessage(
                    @"Your local chart version is not for publishing.Because it is made for working local only");
                return;
            }

            try
            {
                IMapDocument pNewDocument = (IMapDocument) application.Document;
                
                PublishViewModel viewModel = new PublishViewModel(application, _viewModelLocator, setLock);

               viewModel.SetChartInfo(ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData); 
                PublishWindow publishView = new PublishWindow() {DataContext = viewModel, ShowInTaskbar = false};
                var parentHandle = new IntPtr(application.hWnd);
                var helper = new WindowInteropHelper(publishView) {Owner = parentHandle};

                viewModel.Close += new EventHandler((a, b) =>
                    publishView.Close()
                );
                
                publishView.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(typeof(GlobalController), ex,
                    _viewModelLocator.GetNotifyService());
            }
            finally
            {
                LogManager.GetLogger(typeof(GlobalController)).InfoWithMemberName($"Finished");
            }
        }

        internal static async Task InfoAsync(IApplication application)
        {
            // Need to change SynchronizationContext to make app run always in main thread after completing worker thread
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());
            LogManager.GetLogger(typeof(GlobalController)).InfoWithMemberName($"Started");
            if (!ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.IsInitialized)
            {
                _viewModelLocator.ShowMessage("Chart has not been published yet");
            }
            else
            {
                try
                {
                    LogManager.GetLogger(typeof(GlobalController)).InfoWithMemberName(
                        $"GetChart is calling");
                    var amdb = await _viewModelLocator.GetAmdb();

                    LogManager.GetLogger(typeof(GlobalController)).InfoWithMemberName(
                        $"GetLatestChartAsync is calling");
                    var latestAmdb =  _viewModelLocator.GetLatestAmdb();

                    string msg = CheckStatus(latestAmdb);
                    if (string.IsNullOrEmpty(msg))
                        msg = "Your local amdb version is OK";
                    InfoViewModel viewModel = new InfoViewModel(amdb, msg);
                    InfoWindow infoView = new InfoWindow() {DataContext = viewModel, ShowInTaskbar = false};
                    var parentHandle = new IntPtr(application.hWnd);
                    var helper = new WindowInteropHelper(infoView) {Owner = parentHandle};
                    viewModel.Close += new EventHandler((a, b) =>
                        infoView.Close()
                    );
                    LogManager.GetLogger(typeof(GlobalController)).InfoWithMemberName($"Info Window is showing");
                    infoView.ShowDialog();
                }
                catch (Exception ex)
                {
                    ErrorHandler.Handle(typeof(GlobalController), ex,
                        _viewModelLocator.GetNotifyService());
                }
            }
            LogManager.GetLogger(typeof(GlobalController)).InfoWithMemberName($"Finished");
        }

        public static async Task OpenAsync(IApplication mApplication)
        {
            await _viewModelLocator.OpenAsync(mApplication);
        }

        public static async Task UnlockAsync()
        {
            LogManager.GetLogger(typeof(GlobalController)).InfoWithMemberName($"Started");
            try
            {
                AmdbMetadata amdb = null;
                string msg;
                if (ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.IsInitialized)
                {
                    LogManager.GetLogger(typeof(GlobalController)).InfoWithMemberName(
                        $"GetLatestChartAsync is calling");
                    amdb =  _viewModelLocator.GetLatestAmdb();

                    msg = CheckStatus(amdb);
                }
                else
                    msg = "Chart has not been published yet";
                if (!string.IsNullOrEmpty(msg))
                {
                    _viewModelLocator.ShowMessage(msg);
                    return;
                }
                LogManager.GetLogger(typeof(GlobalController)).InfoWithMemberName(
                    $"Unlock({amdb.Id},false) is calling");
                await _viewModelLocator.Unlock(amdb.Id, false);

                _viewModelLocator.ShowMessage("Actual compatible server version is unlocked");
            }            
            catch (Exception ex)
            {
                ErrorHandler.Handle(typeof(GlobalController), ex, _viewModelLocator.GetNotifyService());
            }
            LogManager.GetLogger(typeof(GlobalController)).InfoWithMemberName($"Finished");
        }

        public static void CloseConnection()
        {
            _viewModelLocator.CloseConnection();
        }

        private static string CheckStatus(AmdbMetadata chart)
        {
            if (chart == null)
                return "Your local amdb not exist in server";
            else if (ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.IsReadOnly)
                return
                    @"Your local amdb version is not for publishing. Because it is made for working local only";
            else if (chart.Version != ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.AmdbVersion.ToString())
                return "Your local amdb version is not commpatible with actual server version";
            else if (!chart.IsLocked)
                return "Actual compatible server version is not locked";
            else if (chart.IsLocked && chart.LockedBy.UserName != Config.Username)
                return "Actual compatible server version is locked on server by another user";

            return string.Empty;
        }
    }
}