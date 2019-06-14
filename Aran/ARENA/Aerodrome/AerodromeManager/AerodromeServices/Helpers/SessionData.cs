using System.Runtime.Serialization;

namespace AerodromeServices.Helpers
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
