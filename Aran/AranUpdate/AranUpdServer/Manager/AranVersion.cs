using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AranUpdManager
{
    public class AranVersion : DbItem
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public DateTime ReleasedDate { get; set; }

        [DataMember]
        public bool IsDefault { get; set; }

        [DataMember]
        public string ChangesRtf { get; set; }

        public override string Description
        {
            get { return Key; }
        }
    }
}
