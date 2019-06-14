using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AranUpdater
{
    [DataContract]
    public class ARequest<T1, T2>
    {
        public ARequest()
        {
        }

        public ARequest(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        [DataMember]
        public T1 Value1 { get; set; }

        [DataMember]
        public T2 Value2 { get; set; }
    }

    [DataContract]
    public class ARequest<T1, T2, T3> : ARequest<T1, T2>
    {
        [DataMember]
        public T3 Value3 { get; set; }
    }

    
}
