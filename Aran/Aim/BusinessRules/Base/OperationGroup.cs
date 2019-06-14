using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules
{
    [DataContract]
    public class OperationGroup : AbstractOperation
    {
        public OperationGroup()
        {
            Items = new List<AbstractOperation>();
            OperType = AbstractOperationType.Group;
        }

        [DataMember]
        public LogicType Type { get; set; }

        [DataMember]
        public List<AbstractOperation> Items { get; private set; }

        public override bool Equals(object obj)
        {
            var other = obj as OperationGroup;
            if (other == null)
                return false;

            return Global.IsObjectsEquals(
                Type, other.Type,
                Items, other.Items);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
