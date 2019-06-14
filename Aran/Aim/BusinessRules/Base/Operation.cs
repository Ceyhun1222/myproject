using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules
{
    [DataContract]
    public class Operation : AbstractOperation
    {
        public Operation()
        {
            OperType = AbstractOperationType.Operation;
            ExpressedType = ExpressedType.None;
        }

        [DataMember]
        public bool IsNot { get; set; }

        [DataMember]
        public bool IsWith { get; set; }

        [DataMember]
        public OperationType Type { get; set; }

        [DataMember]
        public string Noun { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public OperAssociation Association { get; set; }

        [DataMember]
        public InnerOperation InnerOper { get; set; }

        [DataMember]
        public ExpressedType ExpressedType { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Operation;
            if (other == null)
                return false;

            return Global.IsObjectsEquals(
                IsNot, other.IsNot,
                IsWith, other.IsWith,
                Type, other.Type,
                Noun, other.Noun,
                Value, other.Value,
                Association, other.Association,
                InnerOper, other.InnerOper);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [DataContract]
    public class InnerOperation
    {
        [DataMember]
        public bool IsNot { get; set; }

        [DataMember]
        public InnerOperationType Type { get; set; }

        [DataMember]
        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as InnerOperation;
            if (other == null)
                return false;

            return Global.IsObjectsEquals(
                IsNot, other.IsNot,
                Type, other.Type,
                Value, other.Value);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
