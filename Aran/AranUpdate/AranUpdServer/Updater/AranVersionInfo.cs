using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AranUpdater
{
    [DataContract]
    public class AranVersionInfo
    {
        [DataMember]
        public double? UpdateIntervalSec { get; set; }

        [DataMember]
        public string CurrVersionName { get; set; }

        [DataMember]
        public int? VersionId { get; set; }

        [DataMember]
        public byte[] Data { get; set; }
    }
}
