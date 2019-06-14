using AIMSLServiceClient.CTSDownload;
using AIMSLServiceClient.CTSPubSub;
using AIMSLServiceClient.NotificationBroker;
using AIMSLServiceClient.Remote;

namespace AIMSLServiceClient.Services
{
    class CTSService: AbstracService
    {
        public CTSService()
        {
            Init();
        }

        protected override void Init()
        {
            if (ToInit(AimslPubSubExtConsumerClient))
                AimslPubSubExtConsumerClient = new AIMSLPubSubExtConsumerClient("CTSPubSubPort");
            if (ToInit(SddUploadClient))
                SddUploadClient = new SDDUploadPortTypeClient("CTSUploadPort");
            if (ToInit(SddDownloadClient))
                SddDownloadClient = new SDDDownloandPortTypeClient("CTSDownloadPort");
            if (ToInit(CreatePullPointClient))
                CreatePullPointClient = new CreatePullPointClient("CTSCreatePullPoint");
            if (ToInit(PullPointClient))
                PullPointClient = new PullPointClient("CTSPullPoint");
            if (ToInit(NotificationBrokerClient))
                NotificationBrokerClient = new NotificationBrokerClient("CTSNotificationBroker");
        }
    }
}
