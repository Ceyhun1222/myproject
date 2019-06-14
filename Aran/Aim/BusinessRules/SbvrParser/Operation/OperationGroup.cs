using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BRules.SbvrParser
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

        public void Parse(ITaggedReader reader)
        {
            while (true)
            {
                if (reader.LastRead.IsEqual(TaggedKey.Verb, "have") ||
                    reader.LastRead.IsEqual(TaggedKey.Verb, "has"))
                {
                    reader.Next();

                    var oper = new Operation();
                    oper.Parse(reader);
                    Items.Add(oper);

                    if (reader.LastRead.IsEqual(TaggedKey.Keyword, "and"))
                        Type = LogicType.And;
                    else if (reader.LastRead.IsEqual(TaggedKey.Keyword, "or"))
                        Type = LogicType.Or;
                }
                else if (reader.LastRead.IsEqual(TaggedKey.Keyword, "with"))
                {
                    Console.WriteLine("Not implemented yet [OperationGroup.Parse => with");
                    return;
                }
                else
                {
                    break;
                }
            }
        }

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
