using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AranUpdManager
{
    [DataContract]
    public class User : DbItem
    {
        public User()
        {
            Group = new RefItem();
            HasLog = true;
        }
        
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string LastDownloadedVersion { get; set; }

        [DataMember]
        public string LastUpdatedVersion { get; set; }

        [DataMember]
        public string Note { get; set; }

        [DataMember]
        public RefItem Group { get; private set; }

        [DataMember]
        public bool HasLog { get; set; }

        public override string Description
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FullName))
                    return UserName;
                return FullName;
            }
        }
    }
}
