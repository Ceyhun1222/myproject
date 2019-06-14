using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using AIMSLServiceClient.Config;
using AIMSLServiceClient.CTSPubSub;
using AIMSLServiceClient.Remote;
using AIMSLServiceClient.CTSDownload;
using AIMSLServiceClient.NotificationBroker;
using AIMSLServiceClient.Services.Model;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.Extensions;

namespace AIMSLServiceClient.Services
{
    public abstract class AbstracService : IService
    {
        protected AIMSLPubSubExtConsumerClient AimslPubSubExtConsumerClient;
        protected SDDUploadPortTypeClient SddUploadClient;
        protected SDDDownloandPortTypeClient SddDownloadClient;
        protected CreatePullPointClient CreatePullPointClient;
        protected PullPointClient PullPointClient;
        protected NotificationBrokerClient NotificationBrokerClient;

        protected abstract void Init();

        protected bool ToInit<T>(ClientBase<T> clientBase) where T: class
        {
            if (clientBase == null)
                return true;
            if (clientBase.InnerChannel.State == CommunicationState.Faulted)
                return true;
            return false;
        }

        public List<string> GetAllSubscriptions()
        {
            Init();
            var result = AimslPubSubExtConsumerClient.GetAllSubscriptions(new[] { "*" });
            return result?.Select(t => t.MatchingTopics).ToList();
        }

        public int GetAllTopics(string filter)
        {
            Init();
            var result = AimslPubSubExtConsumerClient.GetAllTopics(new[] { filter });
            return result?.Any?.Length ?? 0;
        }

        public string Download(string fileName)
        {
            Init();
            var result = SddDownloadClient.downloadAixmMessageFile(new GetDownload() { downloadFileId = "" });
            return result?.status.ToString();
        }

        public UploadResult Upload(string fileName)
        {
            Init();
            var uploadFile = CreateUploadFile(fileName);
            return UploadFile(uploadFile);
        }

       

        public UploadResult UploadZippedFile(byte[] zippedFile, string fileName)
        {
            Init();
            var uploadFile = CreateUploadFile(zippedFile, fileName);
            return UploadFile(uploadFile);
        }

        public string CreatePullPoint()
        {
            Init();
            var result = CreatePullPointClient.CreatePullPoint(new CreatePullPoint());
            return result?.PullPoint?.Address?.Value;
        }

        public void DestroyPullPoint(string id)
        {
            Init();
            using (var scope = new OperationContextScope(PullPointClient.InnerChannel))
            {
                MessageHeader idHeader = MessageHeader.CreateHeader("id",
                    "http://www.teamead.com/ws/aimsl", id);
                OperationContext.Current.OutgoingMessageHeaders.Add(idHeader);
                PullPointClient.DestroyPullPoint(new DestroyPullPoint());

            }
            
        }

        public string Subscribe(string adress, string uuid)
        {
            Init();
            XmlDocument doc = new XmlDocument();
            XmlElement xmlElement = doc.CreateElement("TopicExpression", "http://docs.oasis-open.org/wsn/b-2");
            var text = doc.CreateTextNode($"sdd_upload:{uuid}");
            xmlElement.Attributes.Append(CreateAttribute(doc, "Dialect", "http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete"));
            xmlElement.Attributes.Append(CreateAttribute(doc, "xmlns:sdd_upload", "http://www.teamead.com/ws/sdd/upload"));
            xmlElement.AppendChild(text);


            return NotificationBrokerClient.Subscribe(new Subscribe
            {
                ConsumerReference = new NotificationBroker.EndpointReferenceType
                {
                    Address = new NotificationBroker.AttributedURIType { Value = adress }
                },
                Filter = new NotificationBroker.FilterType
                {
                    Any = new XmlElement[] { xmlElement }
                },
                InitialTerminationTime = DateTime.UtcNow.AddMinutes(AimslSettings.Settings.SubscriptionTimeout).ToString("s")
            }).SubscriptionReference?.Address?.Value;
        }

        public IList<NotificationMessage> GetMessages(string id)
        {
            Init();
            using (var scope = new OperationContextScope(PullPointClient.InnerChannel))
            {
                MessageHeader idHeader = MessageHeader.CreateHeader("id",
                    "http://www.teamead.com/ws/aimsl", id);
                OperationContext.Current.OutgoingMessageHeaders.Add(idHeader);
                var result = PullPointClient.GetMessages(new GetMessages());
                return Convert(result);

            }
        }

        private IList<NotificationMessage> Convert(GetMessagesResponse response)
        {

            LogManager.GetLogger(typeof(AbstracService)).Trace(response.Dump());
            if (response?.NotificationMessage == null)
                return null;
            if (response.NotificationMessage.Length == 0)
                return null;
            List<NotificationMessage> result = new List<NotificationMessage>();

            foreach (var notification in response.NotificationMessage)
            {
                NotificationMessage message = new NotificationMessage
                {
                    SubscritptionReference = notification.SubscriptionReference?.Address?.Value
                };
                if (notification.Topic?.Any != null && notification.Topic?.Any.Length > 0)
                {
                    message.Topic = DeleteNs(notification.Topic.Any[0].InnerText);
                }
                if (notification.Message != null)
                {
                    var xmlMessage = notification.Message;
                    message.Message = new Model.Message
                    {
                        JobId = GetValue(xmlMessage, "uploadJobId"),
                        Status = GetValue(xmlMessage, "status"),
                        CreateTime = GetDateTime(xmlMessage, "createdOn"),
                        LastChangeTime = GetDateTime(xmlMessage, "lastTouchOn")
                    };

                    var messageNodes = GetNodes(xmlMessage, "message");
                    if (messageNodes.Count > 0)
                    {
                        message.Message.ProcessingMessages = messageNodes.Select(t => t.InnerXml).ToList();
                    }
                    
                }
                result.Add(message);
            }

            return result;
        }

        private List<XmlNode> GetNodes(XmlNode element, string name)
        {
            List<XmlNode> nodes = new List<XmlNode>();
            if (element.HasChildNodes)
                for (var i = 0; i < element.ChildNodes.Count; i++)
                {
                    if (element.ChildNodes[i].Name.Contains(name))
                    {
                        nodes.Add(element.ChildNodes[i]);
                    }
                    nodes.AddRange(GetNodes(element.ChildNodes[i], name));
                }

            return nodes;
        }

        private DateTime GetDateTime(XmlNode element, string name)
        {
            if (element.HasChildNodes)
                for (var i = 0; i < element.ChildNodes.Count; i++)
                {
                    if (element.ChildNodes[i].Name.Contains(name))
                    {
                        return DateTime.Parse(element.ChildNodes[i].InnerText);
                    }
                    var val = GetDateTime(element.ChildNodes[i], name);
                    if (val != default(DateTime)) return val;
                }

            return default(DateTime);
        }

        private string GetValue(XmlNode element, string name)
        {
            if (element.HasChildNodes)
                for (var i = 0; i < element.ChildNodes.Count; i++)
                {
                    if (element.ChildNodes[i].Name.Contains(name))
                    {
                        return DeleteNs(element.ChildNodes[i].InnerText);
                    }
                    var val = GetValue(element.ChildNodes[i], name);
                    if (val != null) return val;
                }

            return null;
        }


        private string DeleteNs(string value)
        {
            if (value.Split(':').Length > 1)
                return value.Split(':')[1];
            return value;
        }

        private static XmlAttribute CreateAttribute(XmlDocument doc, string name, string value)
        {
            var atrr = doc.CreateAttribute(name);
            atrr.Value = value;
            return atrr;
        }

        private UploadResult UploadFile(UploadFile uploadFile)
        {
            UploadAixmMessageFile file = new UploadAixmMessageFile
            {
                transaction = new transactionType { uuid = uploadFile.Uuid },
                upload = new Remote.AttachedFileType
                {
                    content = uploadFile.Content,
                    mimetype = uploadFile.MimeType,
                    checksum = uploadFile.CheckSum,
                    name = uploadFile.Name
                }
            };
            using (var scope = new OperationContextScope(SddUploadClient.InnerChannel))
            {
                MessageHeader skipHeader = MessageHeader.CreateHeader("SkipTasklets",
                    "http://www.teamead.com/ws/sdd/header", "UploadJobSdoProvisioningTasklet");
                OperationContext.Current.OutgoingMessageHeaders.Add(skipHeader);
                var result = SddUploadClient.uploadAixmMessageFile(file);
                return new UploadResult { Uuid = result?.uploadJobId, Status = result?.status.ToString() };
            }
        }

        private UploadFile CreateUploadFile(string filePath)
        {
            byte[] content = GetZippedFileContent(filePath);
            return CreateUploadFile(content, Path.GetFileName(filePath));
        }

        private UploadFile CreateUploadFile(byte[] zipped, string fileName)
        {
            string hash = MD5String(MD5(zipped));

            UploadFile file = new UploadFile
            {
                Name = fileName + "zip.b64",
                Content = System.Text.Encoding.UTF8.GetBytes(System.Convert.ToBase64String(zipped)),
                CheckSum = hash
            };
            return file;
        }

        protected string MD5String(byte[] hash)
        {
            return string.Concat(hash.Select(x => x.ToString("X2"))).ToLower();
        }

        protected byte[] MD5(byte[] content)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();

            return md5.ComputeHash(content);
        }

        protected byte[] GetZippedFileContent(string fileName)
        {
            byte[] content;
            using (var ms = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {

                    zipArchive.CreateEntryFromFile(Path.GetFullPath(fileName), Path.GetFileName(fileName),
                        CompressionLevel.Fastest);
                }
                content = ms.ToArray();
            }

            return content;
        }
    }
}