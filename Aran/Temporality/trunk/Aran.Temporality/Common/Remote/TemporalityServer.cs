#region

using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Xml;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Internal.Implementation.Storage;
using Aran.Temporality.Internal.Remote.Service;
using Aran.Temporality.Internal.Remote.Util;

#endregion

namespace Aran.Temporality.Common.Remote
{
    public class TemporalityServer
    {
        #region Common server

        private static ServiceHost _mainHost;
        private static ServiceHost _helperHost;
        private static ServiceHost _externalHost;

        public static void StopServer()
        {
            _mainHost?.Abort();
            _mainHost = null;


            _helperHost?.Abort();
            _helperHost = null;

            _externalHost?.Abort();
            _externalHost = null;
        }

        private static void StartMain(int servicePort)
        {
            _mainHost = new ServiceHost(typeof(RemoteService), new Uri($"net.tcp://localhost:{servicePort}"));

            var binding = new NetTcpBinding
            {
                Security =
                {
                    Mode = SecurityMode.Message,
                    Message = { ClientCredentialType = MessageCredentialType.UserName }
                },
                ReaderQuotas = XmlDictionaryReaderQuotas.Max,
                MaxReceivedMessageSize = 1024 * 1014 * 100,
                MaxBufferSize = 1024 * 1014 * 100,
                MaxBufferPoolSize = 1024 * 1014 * 100,
                SendTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0)
            };


            _mainHost.Credentials.ServiceCertificate.SetCertificate(
                StoreLocation.LocalMachine,
                StoreName.My,
                //X509FindType.FindBySubjectName,
                X509FindType.FindByIssuerName,
                ConfigUtil.TemporalityServerCertificateIssuer);

            _mainHost.Credentials.UserNameAuthentication.UserNamePasswordValidationMode =
                UserNamePasswordValidationMode.Custom;
            _mainHost.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new AuthenticationUtil();

            _mainHost.AddServiceEndpoint(typeof(IRemoteService), binding, ConfigUtil.TemporalityServerEndPoint);

            _mainHost.Open();
        }

        private static void StartHelper(int helperPort)
        {
            _helperHost = new ServiceHost(typeof(RemoteHelperService), new Uri($"net.tcp://localhost:{helperPort}"));


            _helperHost.AddServiceEndpoint(typeof (IRemoteHelperService),
                                      new NetTcpBinding
                                          {
                                              Security =
                                                  {
                                                      Mode = SecurityMode.None
                                                  },
                                              ReaderQuotas = XmlDictionaryReaderQuotas.Max,
                                              MaxReceivedMessageSize = 1024*1014*100,
                                              MaxBufferSize = 1024*1014*100,
                                              MaxBufferPoolSize = 1024*1014*100,
                                              SendTimeout = new TimeSpan(0, 10, 0),
                                              ReceiveTimeout = new TimeSpan(0, 10, 0)
                                          }, 
                                          ConfigUtil.HelperServerEndPoint);
            _helperHost.Open();
        }


        private static void StartExternal(int externalPort)
        {
            _externalHost = new ServiceHost(typeof(ExternalSystemService), new Uri($"http://localhost:{externalPort}/"));

           _externalHost.AddServiceEndpoint(typeof(IExternalSystemService), new WebHttpBinding
            {
                Security =
                {
                    Mode = WebHttpSecurityMode.None
                },
                ReaderQuotas = XmlDictionaryReaderQuotas.Max,
                MaxReceivedMessageSize = 1024 * 1014 * 100,
                MaxBufferSize = 1024 * 1014 * 100,
                MaxBufferPoolSize = 1024 * 1014 * 100,
                SendTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0)
            }, "").Behaviors.Add(new WebHttpBehavior());

            _externalHost.Description.Behaviors.Add(new ServiceMetadataBehavior{ HttpGetEnabled = true });

            _externalHost.Open();

        }

        public static void StartServer(int servicePort, int helperPort, int externalPort)
        {

            //PreLoadEsriCsm(true);

            StorageService.Init();
            StorageService.PreStart();

            //init main service
            StartMain(servicePort);

            //init helper service
            StartHelper(helperPort);

            //init external systems service
            StartExternal(externalPort);
        }

        #endregion


        private static void PreLoadEsriCsm(bool debugOnly)
        {
            if (!debugOnly || System.Diagnostics.Debugger.IsAttached)
            {
                string path = System.IO.Path.Combine(ESRI.ArcGIS.RuntimeManager.ActiveRuntime.Path, "Bin\\CSM.dll");
                if (System.IO.File.Exists(path))
                {
                    bool isConflict = false;
                    bool isMatch = false;
                    using (var p = System.Diagnostics.Process.GetCurrentProcess())
                    {
                        foreach (System.Diagnostics.ProcessModule m in p.Modules)
                        {
                            if (m.ModuleName.ToLower() == "csm.dll")
                            {
                                if (path.ToLower() != m.FileName.ToLower())
                                    isConflict = true;
                                else
                                {
                                    isMatch = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (isConflict && !isMatch)
                    {
                        System.Diagnostics.Debug.WriteLine("It may be necessary to call this method earlier to be effective.");
                    }
                    if (!isMatch)
                        LoadLibrary(path);
                }
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string dllname);


    }
}