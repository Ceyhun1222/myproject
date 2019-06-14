using System.Runtime.Serialization;

namespace ChartServices.Helpers
{
    [DataContract]
    public class SessionData
    {
        [DataMember]
        public string SessionId { get; set; }

        [DataMember]
        public string UserName { get; set; }
    }
}
