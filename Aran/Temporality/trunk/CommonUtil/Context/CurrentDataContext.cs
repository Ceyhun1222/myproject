using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Aim.Service;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Remote.ClientServer;
using ImpromptuInterface;
using UMCommonModels.Dto;
using WebApiClient;

namespace Aran.Temporality.CommonUtil.Context
{
    public static class CurrentDataContext
    {
        public static SystemType SystemType;

        public static string ServiceAddress;

        public static string ServiceHost
        {
            get
            {
                if (ServiceAddress == null) return null;
                var colonIndex = ServiceAddress.IndexOf(':');
                return colonIndex == -1 ? null : ServiceAddress.Substring(0, colonIndex);
            }
        }

        public static string HelperAddress;
        public static string StorageName;
        public static int UserId;
        public static bool IsUserSecured;

        public static int ServicePort = 8523;
        public static int HelperPort = 8524;
        public static int ExternalPort = 8525;

        public static string Application = "TOSSM";

        public static EsriLicense CurrentLicense;

        public static string ConnectString;

        public static string CurrentPassword;
        public static string CurrentUserName;

        public static User CurrentUser { get; set; }

        public static UserDto UserDto { get; set; }

        public static string ProviderLogFile { get; set; }
        public static string DllRepo { get; set; }

        public static INoAixmDataService CurrentNoAixmDataService;
        public static ITemporalityService<AimFeature> CurrentService;

        private static string _version;
        public static string Version
        {
            get => _version ?? (_version = (System.Reflection.Assembly.GetEntryAssembly() != null?System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString(): System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            set => _version = value;
        }


        private static List<BusinessRuleUtil> _businessRules;


        public static List<BusinessRuleUtil> GetBusinessRules()
        {
            return _businessRules ?? (_businessRules = CurrentNoAixmDataService.GetBusinessRules().ToList());

        }

        public static List<FeatureDependencyConfiguration> GetFeatureDependencies()
        {
            return CurrentNoAixmDataService.GetFeatureDependencies().ToList();
        }

        public static bool DeleteFeatureDependencyConfiguration(int id)
        {
            return CurrentNoAixmDataService.DeleteFeatureDependencyConfiguration(id);
        }

        public static bool Login()
        {

            try
            {
                LogManager.GetLogger(typeof(CurrentDataContext)).Info("Start CurrentDataContext.Login()");
                LogManager.GetLogger(typeof(CurrentDataContext)).Info($"Params: {ServiceAddress}, {StorageName}, {UserId}, {Application}, {Version}, {CurrentPassword}");

                CurrentService = ServiceAddress.ToLower().StartsWith("localhost") ?
                                    AimServiceFactory.OpenLocal(StorageName, out CurrentNoAixmDataService) :
                                    AimServiceFactory.OpenRemote(StorageName,
                                    UserId + "\\" + Application + ":" + Version,
                                    CurrentPassword,
                                    ServiceAddress,
                                    out CurrentNoAixmDataService);

                LogManager.GetLogger(typeof(CurrentDataContext)).Info("CHECKPOINT number 13");
                if (CurrentService != null)
                {
                    CurrentUser = CurrentNoAixmDataService.Login(UserId, CurrentPassword);

                    LogManager.GetLogger(typeof(CurrentDataContext)).Info("CHECKPOINT number 14");
                    if (CurrentUser != null)
                    {
                        SystemType = CurrentService.GetServerType();

                        LogManager.GetLogger(typeof(CurrentDataContext)).Info("CHECKPOINT number 15");
                        if (ConfigUtil.UseWebApiForMetadata)
                        {
                            LogManager.GetLogger(typeof(CurrentDataContext)).Info("CHECKPOINT number 16");
                            var userClient = new UserClient();
                            UserDto = userClient.GetUserById(UserId);
                            LogManager.GetLogger(typeof(CurrentDataContext)).Info("CHECKPOINT number 17");
                        }

                        return true;
                    }
                    CurrentService.Dispose();
                    CurrentService = null;
                    CurrentNoAixmDataService = null;
                }
               
            }
            catch (Exception exception)
            {
                LogManager.GetLogger(typeof(CurrentDataContext)).Error(exception, exception.Message);
            }

            return false;
        }

    }
}
