using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AranUpdManager
{
    [DataContract]
    public class DbItem
    {
        [DataMember]
        public long Id { get; set; }

        public virtual string Description
        {
            get { return String.Empty; }
        }

        public bool IsNew
        {
            get { return Id < 1; }
        }
    }
}
