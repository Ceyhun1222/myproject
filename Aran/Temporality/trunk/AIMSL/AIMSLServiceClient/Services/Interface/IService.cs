using System.Collections.Generic;
using AIMSLServiceClient.Services.Model;

namespace AIMSLServiceClient.Services
{
    public interface IService
    {
        List<string> GetAllSubscriptions();
        int GetAllTopics(string filter);
        string Download(string fileName);
        UploadResult Upload(string fileName);
        UploadResult UploadZippedFile(byte[] zippedFile, string fileName);
        string CreatePullPoint();
        void DestroyPullPoint(string id);
        string Subscribe(string adress, string uuid);
        IList<NotificationMessage> GetMessages(string id);
    }
}