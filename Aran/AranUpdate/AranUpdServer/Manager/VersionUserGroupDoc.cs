using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AranUpdManager
{
    [DataContract]
    public class VersionUserGroupDoc : DbItem
    {
        public VersionUserGroupDoc()
        {
            Version = new RefItem();
            UserGroup = new RefItem();
            DateTime = DateTime.Now;
        }

        [DataMember]
        public RefItem Version { get; private set; }

        [DataMember]
        public RefItem UserGroup { get; private set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public string Note { get; set; }
    }
}
