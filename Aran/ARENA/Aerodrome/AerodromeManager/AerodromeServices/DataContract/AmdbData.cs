using System.Runtime.Serialization;

namespace AerodromeServices.DataContract
{
    [DataContract]
    public class AmdbData:AmdbMetadata
    {        
        [DataMember]
        public virtual byte[] Preview { get; set; }

        [DataMember]
        public virtual byte[] Source { get; set; }
    }
}
