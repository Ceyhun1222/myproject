using AIMSLServiceClient.Config;
using AIMSLServiceClient.CTSDownload;
using AIMSLServiceClient.CTSPubSub;
using AIMSLServiceClient.NotificationBroker;
using AIMSLServiceClient.Remote;
using AIMSLServiceClient.Services.Behaviour;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace AIMSLServiceClient.Services.Implementations
{
    class Service : AbstracService
    {
        public Service()
        {
            Init();
        }

        protected override void Init()
        {
            var host = AimslSettings.Settings.Host;
            var clientCredential = AimslSettings.ClientCredential;
            var dnsIdentity = AimslSettings.Settings.DnsIdentity;

            if (ToInit(AimslPubSubExtConsumerClient))
            {
                var endpointAddress = new EndpointAddress(
                    new Uri("http://" + host + ":8888/aimsl/AIMSLPubSubExtConsumer/1.0"),
                    EndpointIdentity.CreateDnsIdentity(dnsIdentity),
                    new AddressHeaderCollection()
                );

                var customBinding = new CustomBinding("SDDBinding");

                AimslPubSubExtConsumerClient = new AIMSLPubSubExtConsumerClient(customBinding, endpointAddress);
                // AimslPubSubExtConsumerClient.Endpoint.EndpointBehaviors.Add(new UnsecureBehaviour());

                AimslPubSubExtConsumerClient.Endpoint.Contract = ContractDescription.GetContract(typeof(AIMSLPubSubExtConsumerClient));

                // todo remove by type, not by index
                AimslPubSubExtConsumerClient.Endpoint.EndpointBehaviors.RemoveAt(1);
                AimslPubSubExtConsumerClient.Endpoint.EndpointBehaviors.Add(clientCredential);
            }

            if (ToInit(SddUploadClient))
            {
                var endpointAddress = new EndpointAddress(
                    new Uri("http://" + host + ":8888/aimsl/SDDUploadWebService/1.0"),
                    EndpointIdentity.CreateDnsIdentity(dnsIdentity),
                    new AddressHeaderCollection()
                );

                var customBinding = new CustomBinding("MTOMBinding");

                SddUploadClient = new SDDUploadPortTypeClient(customBinding, endpointAddress);

                SddUploadClient.Endpoint.Contract = ContractDescription.GetContract(typeof(SDDUploadPortTypeClient));

                // todo remove by type, not by index
                SddUploadClient.Endpoint.EndpointBehaviors.RemoveAt(1);
                SddUploadClient.Endpoint.EndpointBehaviors.Add(clientCredential);
            }

            if (ToInit(SddDownloadClient))
            {
                var endpointAddress = new EndpointAddress(
                    new Uri("http://" + host + ":8888/aimsl/SDDDownloadWebService/1.0"),
                    EndpointIdentity.CreateDnsIdentity(dnsIdentity),
                    new AddressHeaderCollection()
                );

                var customBinding = new CustomBinding("SDDBinding");

                SddDownloadClient = new SDDDownloandPortTypeClient(customBinding, endpointAddress);

                SddDownloadClient.Endpoint.Contract = ContractDescription.GetContract(typeof(SDDDownloandPortTypeClient));

                // todo remove by type, not by index
                SddDownloadClient.Endpoint.EndpointBehaviors.RemoveAt(1);
                SddDownloadClient.Endpoint.EndpointBehaviors.Add(clientCredential);
            }

            if (ToInit(CreatePullPointClient))
            {
                var endpointAddress = new EndpointAddress(
                    new Uri("http://" + host + ":8888/aimsl/CreatePullPoint/1.3"),
                    EndpointIdentity.CreateDnsIdentity(dnsIdentity),
                    new AddressHeaderCollection()
                );

                var customBinding = new CustomBinding("SubscriptionManagerBinding");

                CreatePullPointClient = new CreatePullPointClient(customBinding, endpointAddress);

                CreatePullPointClient.Endpoint.Contract = ContractDescription.GetContract(typeof(CreatePullPointClient));

                // todo remove by type, not by index
                CreatePullPointClient.Endpoint.EndpointBehaviors.RemoveAt(1);
                CreatePullPointClient.Endpoint.EndpointBehaviors.Add(clientCredential);
            }

            if (ToInit(PullPointClient))
            {
                var endpointAddress = new EndpointAddress(
                    new Uri("http://" + host + ":8888/aimsl/PullPoint/1.3"),
                    EndpointIdentity.CreateDnsIdentity(dnsIdentity),
                    new AddressHeaderCollection()
                );

                var customBinding = new CustomBinding("SubscriptionManagerBinding");

                PullPointClient = new PullPointClient(customBinding, endpointAddress);
                //  PullPointClient.Endpoint.EndpointBehaviors.Add(new UnsecureBehaviour());

                PullPointClient.Endpoint.Contract = ContractDescription.GetContract(typeof(PullPointClient));

                // todo remove by type, not by index
                PullPointClient.Endpoint.EndpointBehaviors.RemoveAt(1);
                PullPointClient.Endpoint.EndpointBehaviors.Add(clientCredential);
            }

            if (ToInit(NotificationBrokerClient))
            {
                var endpointAddress = new EndpointAddress(
                    new Uri("http://" + host + ":8888/aimsl/NotificationBroker/1.3"),
                    EndpointIdentity.CreateDnsIdentity(dnsIdentity),
                    new AddressHeaderCollection()
                );

                var customBinding = new CustomBinding("NotificationManagerBinding");

                NotificationBrokerClient = new NotificationBrokerClient(customBinding, endpointAddress);

                NotificationBrokerClient.Endpoint.Contract = ContractDescription.GetContract(typeof(NotificationBrokerClient));

                // todo remove by type, not by index
                NotificationBrokerClient.Endpoint.EndpointBehaviors.RemoveAt(1);
                NotificationBrokerClient.Endpoint.EndpointBehaviors.Add(clientCredential);
            }
        }
    }
}
