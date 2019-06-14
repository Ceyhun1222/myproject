using GalaSoft.MvvmLight.Ioc;
using System;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Xml;
using AmdbManager.Helper;
using AmdbManager.Logging;
using ESRI.ArcGIS.Framework;
using Microsoft.Practices.ServiceLocation;
using SigmaChart;
using AerodromeManager.AmdbService;
using Aerodrome.Metadata;

namespace AmdbManager.ViewModel
{
    public class ViewModelLocator
    {
        private static string keyForClient;

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<INotifyService, NotifyService>();
            SimpleIoc.Default.Register<MainViewModel>();
            RegisterServiceClient();
            SimpleIoc.Default.Register<IAmdbManagerService>(() =>
                SimpleIoc.Default.GetInstance<AmdbManagerServiceClient>());
        }

        private static void RegisterServiceClient()
        {
            keyForClient = Guid.NewGuid().ToString();
            SimpleIoc.Default.Register<AmdbManagerServiceClient>(() =>
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
                AmdbManagerServiceClient res = new AmdbManagerServiceClient(
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
                var client = ServiceLocator.Current.GetInstance<AmdbManagerServiceClient>(keyForClient);
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                    client.Close();
                    //ServiceLocator.Current.GetInstance<ChartManagerServiceClient>().Open();
                    SimpleIoc.Default.Unregister<AmdbManagerServiceClient>(keyForClient);
                    RegisterServiceClient();
                    client = ServiceLocator.Current.GetInstance<AmdbManagerServiceClient>(keyForClient);
                }

                res.AmdbManagerService = client;
                return res;
            }
        }

        public async Task Unlock(long chartId, bool locked)
        {
            await _mainViewModel.AmdbManagerService.LockAmdbAsync(chartId, locked);
        }

        public async Task PublishAsync(string chartName, string sourceFileName,
            string organization, string airport, bool setLock = false)
        {
           
            {
                AmdbMetadata amdbData = new AmdbMetadata
                {
                    Identifier = ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Id,
                    Name = chartName,                   
                    Airport = airport,
                    Organization = organization,                  
                    Version = ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.AmdbVersion.ToString(),
                    IsLocked = setLock,
                   
                };
               
                await _mainViewModel.AmdbManagerService.UploadAsync(amdbData, File.ReadAllBytes(sourceFileName));
            }            
        }

        public  AmdbMetadata GetLatestAmdb()
        {
            return  _mainViewModel.AmdbManagerService.GetLatestAmdbVersion(ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Id);
        }

        public async Task<AmdbMetadata> GetAmdb()
        {
            return await _mainViewModel.AmdbManagerService.GetAmdbAsync(ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Id,
                 ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.AmdbVersion.ToString());
        }

        public async Task OpenAsync(IApplication application)
        {
            LogManager.GetLogger(this).InfoWithMemberName($"Started");
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

            await _mainViewModel.InitializeAsync(application);
            LogManager.GetLogger(this).InfoWithMemberName($"Finished");
        }

        public void ShowMessage(string message)
        {
            GetNotifyService().ShowMessage(message);
        }

        public INotifyService GetNotifyService()
        {
            return SimpleIoc.Default.GetInstance<INotifyService>();
        }

       

        public void CloseConnection()
        {
            if (SimpleIoc.Default.GetInstance<AmdbManagerServiceClient>(keyForClient)?.State !=
                CommunicationState.Faulted)
                SimpleIoc.Default.GetInstance<AmdbManagerServiceClient>(keyForClient)?.Close();
        }
    }
}