using ChartManager;
using ChartManager.ChartServices;
using ChartManager.ViewModel;
using ChartManagerTools.View;
using ChartManagerTools.ViewModel;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using EsriWorkEnvironment;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Interop;
using ChartManager.Logging;
using System.Threading;
using System.Windows.Threading;
using SigmaChart;
using System.Collections.Generic;

namespace ChartManagerTools
{
    public static class GlobalController
    {
        private static readonly ViewModelLocator _viewModelLocator;
        private static ILogger _logger;

        static GlobalController()
        {
            _viewModelLocator = new ViewModelLocator();
            LogManager.Configure("ChartManager.log", "ChartManager_Errorlogs.log", LogLevel.Info);
            var preFix = new string('#', 30);
            _logger = LogManager.GetLogger(typeof(GlobalController));
            _logger.Info($"{preFix} New session is created {preFix}");
        }

        public static async Task PublishAsync(IApplication application, bool setLock = true)
        {
            _logger.InfoWithMemberName($"Started");
            if (EsriExtensionData.IsReadOnly)
            {
                _viewModelLocator.ShowMessage(
                    @"Your local chart version is not for publishing.Because it is made for working local only");
                return;
            }

            try
            {
                IMapDocument pNewDocument = (IMapDocument) application.Document;
                var foldername = Path.GetDirectoryName(pNewDocument.DocumentFilename);
                List<string> referenceIdList = ChartsHelperClass.Read_CEFID(foldername);

                PublishViewModel viewModel = new PublishViewModel(application, _viewModelLocator, foldername, setLock, referenceIdList);
                _logger.InfoWithMemberName(
                    $"{nameof(viewModel.SetChartInfo)}({foldername})is calling");
                viewModel.SetChartInfo(EsriUtils.GetChartIno(foldername));
                PublishWindow publishView = new PublishWindow() {DataContext = viewModel, ShowInTaskbar = false};
                viewModel.IsEffectiveDateEnabled = !EsriExtensionData.HasUpdate;

                var parentHandle = new IntPtr(application.hWnd);
                var helper = new WindowInteropHelper(publishView) {Owner = parentHandle};

                viewModel.Close += new EventHandler((a, b) =>
                    publishView.Close()
                );
                _logger.InfoWithMemberName(
                    $"{nameof(publishView.ShowDialog)} is calling");
                publishView.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(typeof(GlobalController), ex,
                    _viewModelLocator.GetNotifyService());
            }
            finally
            {
                _logger.InfoWithMemberName($"Finished");
            }
        }

        internal static async Task UpdateAsync(IApplication mApplication)
        {
            _logger.InfoWithMemberName($"Started");
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());
            try
            {
                
                var hasPendingUpdate = await _viewModelLocator.HasPendingUpdate();
                if(hasPendingUpdate)
                {
                    _viewModelLocator.ShowMessage("You have pending update(s). Please, update all of them. After that try again.");
                    return;
                }
                var uploadViewModel = new UploadPermDataViewModel();
                var uploadView = new UploadPermDataView() { DataContext = uploadViewModel };
                uploadViewModel.PublicationDate = uploadViewModel.AiracList[uploadViewModel.AiracList.Count / 2];

                uploadViewModel.Close += async () =>                 {
                    _logger.InfoWithMemberName($"{nameof(uploadView.Close)} is calling");
                    uploadView.Close();
                    if (uploadViewModel.ApplyClicked)
                    {
                        _logger.InfoWithMemberName($"{nameof(_viewModelLocator.OpenAsync)} is calling");
                        await _viewModelLocator.OpenAsync(mApplication, uploadViewModel.Result);
                    }
                };
                var parentHandle = new IntPtr(mApplication.hWnd);
                var helper = new WindowInteropHelper(uploadView) { Owner = parentHandle };
                uploadView.ShowInTaskbar = false;
                uploadView.Show();
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(typeof(GlobalController), ex,
                    _viewModelLocator.GetNotifyService());
            }
            finally
            {
                _logger.InfoWithMemberName($"Finished");
            }            
        }

        internal static async Task InfoAsync(IApplication application)
        {
            // Need to change SynchronizationContext to make app run always in main thread after completing worker thread
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());
            _logger.InfoWithMemberName($"Started");
            if (!EsriExtensionData.IsInitialized)
            {
                _viewModelLocator.ShowMessage("Chart has not been published yet");
            }
            else
            {
                try
                {
                    _logger.InfoWithMemberName(
                        $"{nameof(_viewModelLocator.GetChartAsync)} is calling");
                    var chart = await _viewModelLocator.GetChartAsync();

                    _logger.InfoWithMemberName(
                        $"{nameof(_viewModelLocator.GetLatestChartAsync)} is calling");
                    var latestChart = await _viewModelLocator.GetLatestChartAsync();

                    string msg = CheckStatus(latestChart);
                    if (string.IsNullOrEmpty(msg))
                        msg = "Your local chart version is OK";
                    InfoViewModel viewModel = new InfoViewModel(chart, msg);
                    InfoWindow infoView = new InfoWindow() {DataContext = viewModel, ShowInTaskbar = false};
                    var parentHandle = new IntPtr(application.hWnd);
                    var helper = new WindowInteropHelper(infoView) {Owner = parentHandle};
                    viewModel.Close += new EventHandler((a, b) =>
                        infoView.Close()
                    );
                    _logger.InfoWithMemberName($"{nameof(infoView.ShowDialog)} is calling");
                    infoView.ShowDialog();
                }
                catch (Exception ex)
                {
                    ErrorHandler.Handle(typeof(GlobalController), ex,
                        _viewModelLocator.GetNotifyService());
                }
            }
            _logger.InfoWithMemberName($"Finished");
        }

        public static Task OpenAsync(IApplication mApplication, bool onlyPendingUpdates = false)
        {
            return _viewModelLocator.OpenAsync(mApplication, getOnlyPendingUpdates: onlyPendingUpdates);
        }

        public static async Task UnlockAsync()
        {
            _logger.InfoWithMemberName($"Started");
            try
            {
                Chart chart = null;
                string msg;
                if (EsriExtensionData.IsInitialized)
                {
                    _logger.InfoWithMemberName(
                        $"{nameof(_viewModelLocator.GetLatestChartAsync)} is calling");
                    chart = await _viewModelLocator.GetLatestChartAsync();

                    msg = CheckStatus(chart);
                }
                else
                    msg = "Chart has not been published yet";
                if (!string.IsNullOrEmpty(msg))
                {
                    _viewModelLocator.ShowMessage(msg);
                    return;
                }
                _logger.InfoWithMemberName(
                    $"{nameof(_viewModelLocator.Unlock)}({chart.Id},false) is calling");
                await _viewModelLocator.Unlock(chart.Id, false);

                _viewModelLocator.ShowMessage("Actual compatible server version is unlocked");
            }            
            catch (Exception ex)
            {
                ErrorHandler.Handle(typeof(GlobalController), ex, _viewModelLocator.GetNotifyService());
            }
            _logger.InfoWithMemberName($"Finished");
        }

        public static void CloseConnection()
        {
            _viewModelLocator.CloseConnection();
        }

        private static string CheckStatus(Chart chart)
        {
            if (chart == null)
                return "Your local chart is deleted from server";
            else if (EsriExtensionData.IsReadOnly)
                return
                    @"Your local chart version is not for publishing.Because it is made for working local only";
            else if (chart.Version != EsriExtensionData.ChartVersion.ToString())
                return "Your local chart version is not commpatible with actual server version";
            else if (!chart.IsLocked)
                return "Actual compatible server version is not locked";
            else if (chart.IsLocked && chart.LockedBy.UserName != Config.Username)
                return "Actual compatible server version is locked on server by another user";

            return string.Empty;
        }
    }
}