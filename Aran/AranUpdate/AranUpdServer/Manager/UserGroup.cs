using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AranUpdManager
{
    [DataContract]
    public class UserGroup : DbItem
    {
        [DataMember]
        private string _description;

        public UserGroup()
        {
            Version = new RefItem();
        }

        [DataMember]
        public string Name { get; set; }

        public override string Description
        {
            get { return _description; }
        }

        public void SetDescription(string value)
        {
            _description = value;
        }

        [DataMember]
        public string Note { get; set; }

        [DataMember]
        public RefItem Version { get; private set; }
    }
}
