using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using AIMSLServiceClient.Services;

namespace AIMSLServiceClient.Config
{
    public class AimslSettings: ConfigurationSection
    {
        public static AimslSettings Settings { get; } = ConfigurationManager.GetSection(nameof(AimslSettings)) as AimslSettings;

        public static ClientCredentials ClientCredential { get; set; } = new ClientCredentials();

        [ConfigurationProperty("system", DefaultValue = ServiceType.CTS, IsRequired = false)]
        public ServiceType System
        {
            get => (ServiceType)this["system"];
            set => this["system"] = value;
        }

        [ConfigurationProperty("host", DefaultValue = "vead03.cts.ead-itp.com", IsRequired = false)]
        public string Host
        {
            get => (string)this["host"];
            set => this["host"] = value;
        }

        [ConfigurationProperty("useConfig", DefaultValue = true, IsRequired = false)]
        public bool UseConfig
        {
            get => (bool)this["useConfig"];
            set => this["useConfig"] = value;
        }

        [ConfigurationProperty("dnsIdentity", DefaultValue = "AIMSL CA non-productive System", IsRequired = false)]
        public string DnsIdentity
        {
            get => (string)this["dnsIdentity"];
            set => this["dnsIdentity"] = value;
        }

        [ConfigurationProperty("subscriptionTimeout", DefaultValue = 4320, IsRequired = false)]
        [IntegerValidator(MinValue = 60)]
        public int SubscriptionTimeout
        {
            get => (int)this["subscriptionTimeout"];
            set => this["subscriptionTimeout"] = value;
        }


        [ConfigurationProperty("expectationTimeout", DefaultValue = 4320, IsRequired = false)]
        [IntegerValidator(MinValue = 60)]
        public int ExpectationTimeout
        {
            get => (int)this["expectationTimeout"];
            set => this["expectationTimeout"] = value;
        }

        [ConfigurationProperty("serialNumber", DefaultValue = "07D72DA69C41E0B84D2071F6CCA36A5F", IsRequired = false)]
        public string SerialNumber
        {
            get => (string)this["serialNumber"];
            set => this["serialNumber"] = value;
        }
        
        public static void Configure()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var group = ServiceModelSectionGroup.GetSectionGroup(config);
            var endpointBehaviors = group.Behaviors.EndpointBehaviors;

            foreach (EndpointBehaviorElement endpointBehavior in endpointBehaviors)
            {
                if (endpointBehavior.Name == Settings.System.ToString())
                {
                    var clientCredential = (ClientCredentialsElement)endpointBehavior.First();

                    ClientCredential.ClientCertificate.SetCertificate(
                        findValue: clientCredential.ClientCertificate.FindValue,
                        findType: clientCredential.ClientCertificate.X509FindType,
                        storeName: clientCredential.ClientCertificate.StoreName,
                        storeLocation: clientCredential.ClientCertificate.StoreLocation
                        );

                    ClientCredential.ServiceCertificate.SetDefaultCertificate(
                        findValue: clientCredential.ServiceCertificate.DefaultCertificate.FindValue,
                        storeName: clientCredential.ServiceCertificate.DefaultCertificate.StoreName,
                        findType: clientCredential.ServiceCertificate.DefaultCertificate.X509FindType,
                        storeLocation: clientCredential.ServiceCertificate.DefaultCertificate.StoreLocation
                        );

                    break;
                }
            }

            
            
        }
    }
}
