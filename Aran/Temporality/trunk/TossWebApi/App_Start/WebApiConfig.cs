using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.CommonUtil.Context;
using ESRI.ArcGIS.esriSystem;
using TossWebApi.Controllers;
using TossWebApi.DataAccess;
using TossWebApi.Models;
using TossWebApi.Utils;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using esriLicenseExtensionCode = ESRI.ArcGIS.esriSystem.esriLicenseExtensionCode;

namespace TossWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            try
            {
                LogManager.Configure("tosswebapi.log", "tosswebapi.log", LogLevel.Info);
                LogManager.GetLogger("WebApiConfig").Info("TossWebApi is starting.");

                var tossServerConfigName = ConfigurationManager.AppSettings["TossServerConfigName"];
                if (!ConnectionProvider.InitServerSettings(tossServerConfigName))
                    throw new Exception("ConnectionProvider.InitSettingsFromRegistry");

                // Web API configuration and services
                ConfigUtil.RepositoryType = Aran.Temporality.Common.Enum.RepositoryType.MongoRepository;
                ConfigUtil.OwnChannelName = "TossWebApi";
                ConfigUtil.RemoteChannelName = "Toss";

                CurrentDataContext.StorageName = ConfigurationManager.AppSettings["StorageName"] ??
                                                 throw new Exception("StorageName missing");

                CurrentDataContext.UserId = int.Parse(ConfigurationManager.AppSettings["UserId"] ?? "0");
                if (CurrentDataContext.UserId == 0)
                    throw new Exception("UserId is incorrect");

                CurrentDataContext.ServiceAddress = "localhost";

                // Web API routes
                config.MapHttpAttributeRoutes();

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new {id = RouteParameter.Optional}
                );



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
                        LogManager.GetLogger("WebApiConfig").Info("Error initializing ESRI License");
                        throw new Exception("Error initializing ESRI License");
                    }

                    LogManager.GetLogger("WebApiConfig").Info($"ESRI license has been initialized.");
                }

                //var unity = new UnityContainer();
                //unity.RegisterType<IProductRepository, ProductRepository>(new HierarchicalLifetimeManager());
                //config.DependencyResolver = new UnityResolver(unity);
            }
            catch (Exception e)
            {
                LogManager.GetLogger("WebApiConfig").Error(e);
                throw;
            }
        }

    }
}
