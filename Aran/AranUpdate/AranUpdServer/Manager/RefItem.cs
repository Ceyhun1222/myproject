using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AranUpdManager
{
    [DataContract]
    public class RefItem
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        public bool IsNew { get { return Id < 1; } }

        public void Clear()
        {
            Id = 0;
            Text = string.Empty;
        }

        public void Assign(DbItem item)
        {
            Id = item.Id;
            Text = item.Description;
        }

        public override string ToString()
        {
            if (Text == null)
                return string.Empty;
            return Text;
        }
    }
}
