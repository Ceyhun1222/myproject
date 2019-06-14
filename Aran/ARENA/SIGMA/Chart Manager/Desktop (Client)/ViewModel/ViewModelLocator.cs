using GalaSoft.MvvmLight.Ioc;
using System;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Xml;
using ChartManager.ChartServices;
using ChartManager.Helper;
using ChartManager.Logging;
using ESRI.ArcGIS.Framework;
using SigmaChart;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace ChartManager.ViewModel
{
    public class ViewModelLocator
    {
        private static string keyForClient;
        private static ILogger _logger;

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<INotifyService, NotifyService>();
            SimpleIoc.Default.Register<MainViewModel>();
            RegisterServiceClient();
            SimpleIoc.Default.Register<IChartManagerService>(() =>
                SimpleIoc.Default.GetInstance<ChartManagerServiceClient>());
            _logger = LogManager.GetLogger(typeof(ViewModelLocator));
        }

        private static void RegisterServiceClient()
        {
            keyForClient = Guid.NewGuid().ToString();
            SimpleIoc.Default.Register<ChartManagerServiceClient>(() =>
            {
                NetTcpBinding binding = new NetTcpBinding()
                {
                    Security =
                    {
                        Mode = SecurityMode.Message,
                        Message = {ClientCredentialType = MessageCredentialType.UserName}
                    },
                    ReaderQuotas = XmlDictionaryReaderQuotas.Max,
                    MaxReceivedMessageSize = 1024 * 1014 * 100,
                    MaxBufferSize = 1024 * 1014 * 100,
                    MaxBufferPoolSize = 1024 * 1014 * 100,
                    SendTimeout = new TimeSpan(0, 10, 0),
                    ReceiveTimeout = new TimeSpan(0, 10, 0)
                };

                WSDualHttpBinding wsDualHttpBinding = new WSDualHttpBinding()
                {
                    Security =
                    {
                        Mode = WSDualHttpSecurityMode.Message,
                        Message = {ClientCredentialType = MessageCredentialType.UserName}
                    }
                };


                EndpointAddress address = new EndpointAddress(new Uri(Config.AddressTCP),
                    EndpointIdentity.CreateDnsIdentity(Config.CertificateName));
                ChartManagerServiceClient res = new ChartManagerServiceClient(
                    new InstanceContext(ServiceLocator.Current.GetInstance<MainViewModel>().ViewModel), binding,
                    address);
                res.ClientCredentials.UserName.UserName = Config.Username;
                res.ClientCredentials.UserName.Password = Config.Password;
                return res;
            }, keyForClient);
        }

        private MainViewModel _mainViewModel
        {
            get
            {
                var res = ServiceLocator.Current.GetInstance<MainViewModel>();
                var client = ServiceLocator.Current.GetInstance<ChartManagerServiceClient>(keyForClient);
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                    client.Close();
                    //ServiceLocator.Current.GetInstance<ChartManagerServiceClient>().Open();
                    SimpleIoc.Default.Unregister<ChartManagerServiceClient>(keyForClient);
                    RegisterServiceClient();
                    client = ServiceLocator.Current.GetInstance<ChartManagerServiceClient>(keyForClient);
                }

                res.ChartManagerService = client;
                return res;
            }
        }

        public Task<bool> HasPendingUpdate()
        {
            return _mainViewModel.ChartManagerService.HasPendingUpdateAsync();
        }

        public Task Unlock(long chartId, bool locked)
        {
            return _mainViewModel.ChartManagerService.LockChartAsync(chartId, locked);
        }

        //public async Task UploadUpdateDataAsync(ChartUpdateData updateData, byte[] source)
        //{
        //    await _mainViewModel.UploadUpdateDataAsync(updateData, source);
        //}

        public async Task PublishAsync(string chartName, DateTime publicationDate, string previewFileName, string sourceFileName,
            string organization, string airport, string rwyDir, string note, List<string> referenceIdList, bool setLock = false)
        {
            {
                ChartWithReference chartData = new ChartWithReference
                {
                    Identifier = EsriExtensionData.Id,
                    Name = chartName,
                    BeginEffectiveDate = EsriExtensionData.EffectiveDate,
                    PublicationDate = publicationDate,
                    Airport = airport,
                    Organization = organization,
                    RunwayDirection = rwyDir,
                    Version = EsriExtensionData.ChartVersion.ToString(),
                    IsLocked = setLock,
                    Type = ConvertChartType(SigmaDataCash.SigmaChartType),
                    FeatureIdList = referenceIdList,
                    Note = note
                };
                _logger.InfoWithMemberName(
                        $"UploadAsync(Id:{EsriExtensionData.Id},BeginEffective:{EsriExtensionData.EffectiveDate},Version:{EsriExtensionData.ChartVersion},ChartType:{SigmaDataCash.SigmaChartType}) is calling");

                var previewFile = File.ReadAllBytes(previewFileName);
                var sourceFile = File.ReadAllBytes(sourceFileName);
                if (!EsriExtensionData.HasUpdate)
                    await _mainViewModel.ChartManagerService.UploadAsync(chartData, previewFile, sourceFile);
                else
                    await _mainViewModel.ChartManagerService.UploadWithUpdateAsync(chartData, previewFile, sourceFile, EsriExtensionData.UpdateId);
            }            
        }

        public Task<Chart> GetLatestChartAsync()
        {
            return _mainViewModel.ChartManagerService.GetLatestChartVersionAsync(EsriExtensionData.Id,
                EsriExtensionData.EffectiveDate);
        }

        public Task<Chart> GetChartAsync()
        {
            return _mainViewModel.ChartManagerService.GetChartAsync(EsriExtensionData.Id,
                EsriExtensionData.EffectiveDate, EsriExtensionData.ChartVersion);
        }

        public async Task OpenAsync(IApplication application, ChartUpdateData chartUpdateData = default(ChartUpdateData), bool getOnlyPendingUpdates = false)
        {
            _logger.InfoWithMemberName($"Started");
            MainWindow mainWindow = new MainWindow
            {
                ShowInTaskbar = false,
                DataContext = _mainViewModel
            };
            var parentHandle = new IntPtr(application.hWnd);
            var helper = new WindowInteropHelper(mainWindow) {Owner = parentHandle};
            _mainViewModel.Close += new EventHandler((a, b) =>
                mainWindow.Close());
            mainWindow.Show();

            await _mainViewModel.InitializeAsync(application, chartUpdateData, getOnlyPendingUpdates);
            _logger.InfoWithMemberName($"Finished");
        }

        public void ShowMessage(string message)
        {
            GetNotifyService().ShowMessage(message);
        }

        public INotifyService GetNotifyService()
        {
            return SimpleIoc.Default.GetInstance<INotifyService>();
        }

        private ChartType ConvertChartType(int sigmaChartType)
        {
            SigmaChartTypes sigmaType = (SigmaChartTypes) sigmaChartType;

            switch (sigmaType)
            {
                case SigmaChartTypes.EnrouteChart_Type:
                    return ChartType.Enroute;
                case SigmaChartTypes.SIDChart_Type:
                    return ChartType.Sid;
                case SigmaChartTypes.ChartTypeA:
                    return ChartType.A;
                case SigmaChartTypes.STARChart_Type:
                    return ChartType.Star;
                case SigmaChartTypes.IAPChart_Type:
                    return ChartType.Iac;
                case SigmaChartTypes.PATChart_Type:
                    return ChartType.Pat;
                case SigmaChartTypes.AreaChart:
                    return ChartType.Area;
                case SigmaChartTypes.AerodromeElectronicChart:
                    return ChartType.AerodromeElectronicChart;
                case SigmaChartTypes.AerodromeParkingDockingChart:
                    return ChartType.AerodromeParkingDockingChart;
                case SigmaChartTypes.AerodromeGroundMovementChart:
                    return ChartType.AerodromeGroundMovementChart;
                case SigmaChartTypes.AerodromeBirdChart:
                    return ChartType.AerodromeBirdChart;
                case SigmaChartTypes.AerodromeChart:
                    return ChartType.Aerodrome;
                case SigmaChartTypes.MinimumAltitudeChart:
                    return ChartType.AreaMinimumChart;
                default:
                    throw new ArgumentOutOfRangeException(nameof(SigmaChartTypes));
            }
        }

        public void CloseConnection()
        {
            if (SimpleIoc.Default.GetInstance<ChartManagerServiceClient>(keyForClient)?.State !=
                CommunicationState.Faulted)
                SimpleIoc.Default.GetInstance<ChartManagerServiceClient>(keyForClient)?.Close();
        }
    }
}