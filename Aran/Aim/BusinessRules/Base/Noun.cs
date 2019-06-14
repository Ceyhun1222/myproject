using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules
{
    [DataContract]
    public class Noun
    {
        public Noun()
        {
            Childs = new List<Noun>();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<Noun> Childs { get; private set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Noun other))
                return false;

            return Global.IsObjectsEquals(
                Name, other.Name,
                Childs, other.Childs);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
