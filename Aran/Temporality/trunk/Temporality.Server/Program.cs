using System;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.Remote;
using Aran.Temporality.CommonUtil.Context;
using CommonUtils;
using ESRI.ArcGIS.esriSystem;


namespace Temporality.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (sender, eventArgs) =>
                { 
                    LogManager.GetLogger(typeof(Program)).Fatal(eventArgs.ExceptionObject as Exception, $"Unhandled exception.");
                    LogManager.Flush();
                };


            ConfigUtil.OwnChannelName = "Toss";
            ConfigUtil.RemoteChannelName = "TossWebApi";

            LogManager.GetLogger(typeof(Program)).Info($"TOSS Server is starting.");
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
                    LogManager.GetLogger(typeof(Program)).Info("Error initializing ESRI License");
                    Console.WriteLine(@"Error initializing ESRI License.");
                    Console.WriteLine(@"Press <ENTER> to exit.");
                    Console.ReadLine();
                    return;
                }

                LogManager.GetLogger(typeof(Program)).Info($"ESRI license has been initialized.");
            }

            TemporalityServer.StartServer(CurrentDataContext.ServicePort, CurrentDataContext.HelperPort, CurrentDataContext.ExternalPort);
            LogManager.GetLogger(typeof(Program)).Info($"TOSS Server has been started.");
            Console.Title = $"TOSS: localhost:{CurrentDataContext.ServicePort}, {ConfigUtil.NoDataDatabase}, {ConfigUtil.RepositoryType}, {CurrentDataContext.CurrentLicense}";
            Console.WriteLine(@"Service is available. Press <ENTER> to exit.");
            Console.ReadLine();
            LogManager.GetLogger(typeof(Program)).Info($"TOSS Server is stopping.");
            TemporalityServer.StopServer();
            LogManager.GetLogger(typeof(Program)).Info($"TOSS Server has been stopped.");
        }
    }
}
