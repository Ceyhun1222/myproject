using System;

namespace AIMSLServiceClient.Services
{
    public class UploadFile
    {
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string CheckSum { get; set; }
        public byte[] Content { get; set; }
        public string MimeType { get; set; } = "text/xml";
    }
}