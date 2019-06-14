using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMSLServiceClient.Services.Model
{
    public class NotificationMessage
    {
        public string SubscritptionReference { get; set; }
        public string Topic { get; set; }
        public Message Message { get; set; }
    }

    //public class ProcessingMessage
    //{
    //    public string Type { get; set; }
    //    public string Classification { get; set; }
    //    public string Code { get; set; }
    //    public string Description { get; set; }
    //}

    public class Message
    {
        public string JobId { get; set; }
        public string Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastChangeTime { get; set; }
        public List<string> ProcessingMessages { get; set; }
}
}
