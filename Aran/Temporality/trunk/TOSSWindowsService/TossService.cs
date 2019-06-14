using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using Aran.Temporality.Common.Remote;
using Aran.Temporality.CommonUtil.Context;
using System.Runtime.InteropServices;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Logging;
using ESRI.ArcGIS.esriSystem;
using Aran.Temporality.Common.Enum;

namespace TOSSWindowsService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public long dwServiceType;
        public ServiceState dwCurrentState;
        public long dwControlsAccepted;
        public long dwWin32ExitCode;
        public long dwServiceSpecificExitCode;
        public long dwCheckPoint;
        public long dwWaitHint;
    };

    public partial class TossService : ServiceBase
    {
        private readonly EventLog _eventLog;
        private const string TossSource = "TossSource";
        private const string TossLog = "TossLog";

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        public TossService()
        {
            InitializeComponent();
            if (!EventLog.SourceExists(TossSource))
            {
                EventLog.CreateEventSource(TossSource, TossLog);
            }
            _eventLog = new EventLog {Source = TossSource, Log = TossLog};

        }

        protected override void OnContinue()
        {
            _eventLog.WriteEntry("Continue.");
        }  


        protected override void OnStart(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (sender, eventArgs) =>
                {
                    LogManager.GetLogger(typeof(Program)).Fatal(eventArgs.ExceptionObject as Exception, $"Unhandled exception.");
                    LogManager.Flush();
                };

            _eventLog.WriteEntry("Starting...");
            LogManager.GetLogger(typeof(Program)).Info($"TOSS Server is starting.");
            var serviceStatus = new ServiceStatus
                                    {
                                        dwCurrentState = ServiceState.SERVICE_START_PENDING, 
                                        dwWaitHint = 100000
                                    };
            SetServiceStatus(ServiceHandle, ref serviceStatus);

            try
            {
                var configName = args.Length > 0 ? args[0] : null;
                ConnectionProvider.InitServerSettings(configName);
                LogManager.GetLogger(typeof(Program)).Info($"Registry data has been read.");

                //init esri license in the same thread

                if (CurrentDataContext.CurrentLicense != EsriLicense.Missing)
                {
                    esriLicenseProductCode esriLicenseProductCode;
                    if (CurrentDataContext.CurrentLicense == EsriLicense.Basic)
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeBasic;
                    else if (CurrentDataContext.CurrentLicense == EsriLicense.Standard)
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeStandard;
                    else if (CurrentDataContext.CurrentLicense == EsriLicense.Enginegeodb)
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB;
                    else if (CurrentDataContext.CurrentLicense == EsriLicense.Advanced)
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeAdvanced;
                    else if (CurrentDataContext.CurrentLicense == EsriLicense.Arcserver)
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeArcServer;
                    else if (CurrentDataContext.CurrentLicense == EsriLicense.Engine)
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeEngine;
                    else
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeBasic;

                    var initializeIsSuccess = LicenseInitializer.Instance.InitializeApplication(new[] { esriLicenseProductCode }, new esriLicenseExtensionCode[] { });

                    if (!initializeIsSuccess)
                    {
                        throw new Exception("Error initializing ESRI License.");
                    }

                    LogManager.GetLogger(typeof(Program)).Info($"ESRI lisence has been initialized.");
                }

                TemporalityServer.StartServer(CurrentDataContext.ServicePort, CurrentDataContext.HelperPort, CurrentDataContext.ExternalPort);
                LogManager.GetLogger(typeof(Program)).Info($"TOSS Server has been started.");
                _eventLog.WriteEntry("Started.");
            }
            catch (Exception exception)
            {
                LogManager.GetLogger(typeof(Program)).Fatal(exception, $"Unhandled exception.");
                LogManager.Flush();
                _eventLog.WriteEntry("Exception:" + exception.Message);
                throw;
            }
          

            serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_RUNNING,
            };
            SetServiceStatus(ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            _eventLog.WriteEntry("Stopping...");
            var serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_STOP_PENDING,
                dwWaitHint = 100000
            };
            SetServiceStatus(ServiceHandle, ref serviceStatus);

            TemporalityServer.StopServer();
            _eventLog.WriteEntry("Stopped.");

            serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_STOPPED,
            };
            SetServiceStatus(ServiceHandle, ref serviceStatus);
        }
    }
}
