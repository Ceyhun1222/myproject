using AIMSLServiceClient.CTSDownload;
using AIMSLServiceClient.CTSPubSub;
using AIMSLServiceClient.Remote;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using AIMSLServiceClient.Config;
using AttachedFileType = AIMSLServiceClient.Remote.AttachedFileType;

namespace AIMSLServiceClient.Services
{
    public class AimslService
    {
        private static readonly ConcurrentDictionary<ServiceType, IService> Services = new ConcurrentDictionary<ServiceType, IService>();
        private static bool _init;

        public static void Init()
        {
            if(_init)
                return;
            AimslSettings.Configure();
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            _init = true;
        }

        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        {
            var serialNumberString = cert.GetSerialNumberString();
            return serialNumberString != null && serialNumberString.Equals(AimslSettings.Settings.SerialNumber);
            //return true;
        }

        public static IService GetService()
        {
            return Services.GetOrAdd(AimslSettings.Settings.System, ValueFactory);
        }

        public static IService GetService(ServiceType type)
        {
            return Services.GetOrAdd(type, ValueFactory);
        }

        private static IService ValueFactory(ServiceType serviceType)
        {
            if (AimslSettings.Settings.UseConfig)
            {
                if (serviceType == ServiceType.FTS)
                    return new FTSService();

                if (serviceType == ServiceType.CTS)
                    return new CTSService();

                if (serviceType == ServiceType.TR)
                    return new TRService();
            }

            // service created in runtime based on config
            return new Implementations.Service();
        }
    }
}
