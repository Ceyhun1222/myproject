using AIMSLServiceClient.CTSDownload;
using AIMSLServiceClient.CTSPubSub;
using AIMSLServiceClient.NotificationBroker;
using AIMSLServiceClient.Remote;

namespace AIMSLServiceClient.Services
{
    class FTSService : AbstracService
    {
        public FTSService()
        {
            Init();
        }

        protected override void Init()
        {
            if (ToInit(AimslPubSubExtConsumerClient))
                AimslPubSubExtConsumerClient = new AIMSLPubSubExtConsumerClient("FTSPubSubPort");
            if (ToInit(SddUploadClient))
                SddUploadClient = new SDDUploadPortTypeClient("FTSUploadPort");
            if (ToInit(SddDownloadClient))
                SddDownloadClient = new SDDDownloandPortTypeClient("FTSDownloadPort");
            if (ToInit(CreatePullPointClient))
                CreatePullPointClient = new CreatePullPointClient("FTSCreatePullPoint");
            if (ToInit(PullPointClient))
                PullPointClient = new PullPointClient("FTSPullPoint");
            if (ToInit(NotificationBrokerClient))
                NotificationBrokerClient = new NotificationBrokerClient("FTSNotificationBroker");
        }
    }
}
