using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AranUpdater
{
    [DataContract]
    public class UserRequest<T>
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public T Value { get; set; }
    }

    [DataContract]
    public class UserRequest<T1, T2>
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public T1 Value1 { get; set; }

        [DataMember]
        public T2 Value2 { get; set; }
    }
}
