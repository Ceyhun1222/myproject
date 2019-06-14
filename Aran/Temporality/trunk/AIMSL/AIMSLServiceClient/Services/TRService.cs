using AIMSLServiceClient.CTSDownload;
using AIMSLServiceClient.CTSPubSub;
using AIMSLServiceClient.NotificationBroker;
using AIMSLServiceClient.Remote;

namespace AIMSLServiceClient.Services
{
    class TRService : AbstracService
    {
        public TRService()
        {
            Init();
        }

        protected override void Init()
        {
            if (ToInit(AimslPubSubExtConsumerClient))
                AimslPubSubExtConsumerClient = new AIMSLPubSubExtConsumerClient("TRPubSubPort");
            if (ToInit(SddUploadClient))
                SddUploadClient = new SDDUploadPortTypeClient("TRUploadPort");
            if (ToInit(SddDownloadClient))
                SddDownloadClient = new SDDDownloandPortTypeClient("TRDownloadPort");
            if (ToInit(CreatePullPointClient))
                CreatePullPointClient = new CreatePullPointClient("TRCreatePullPoint");
            if (ToInit(PullPointClient))
                PullPointClient = new PullPointClient("TRPullPoint");
            if (ToInit(NotificationBrokerClient))
                NotificationBrokerClient = new NotificationBrokerClient("TRNotificationBroker");
        }
    }
}
