using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules
{
    [DataContract]
    public class OperAssociation
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Noun Noun { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as OperAssociation;
            if (other == null)
                return false;

            return Global.IsObjectsEquals(
                Name, other.Name,
                Noun, other.Noun);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
