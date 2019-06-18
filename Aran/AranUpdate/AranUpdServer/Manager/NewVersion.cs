﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AranUpdManager
{
    [DataContract]
    public class NewVersion
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public DateTime ReleasedDate { get; set; }

        [DataMember]
        public byte[] Data { get; set; }

        [DataMember]
        public string ChangesRtf { get; set; }
    }
}