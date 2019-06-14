using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules
{
    [DataContract]
    public class BRule
    {
        [DataMember]
        public RuleType Type { get; set; }

        [DataMember]
        public Noun Noun { get; set; }

        [DataMember]
        public AbstractOperation Operation { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as BRule;
            if (other == null)
                return false;

            return Global.IsObjectsEquals(
                Type, other.Type,
                Noun, other.Noun,
                Operation, other.Operation);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public enum RuleType
    {
        Each,
        Prohibited,
        Obligatory
    }
}
