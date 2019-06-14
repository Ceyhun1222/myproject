using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BRules.SbvrParser
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

        public void Parse(ITaggedReader reader)
        {
            var tagItem = reader.Read();

            if (tagItem.Key != TaggedKey.Keyword)
                throw new InvalidTaggedException();

            if (tagItem.Text.StartsWithIC("It is obligatory"))
                Type = RuleType.Obligatory;
            else if (tagItem.Text.StartsWithIC("It is prohibited that"))
                Type = RuleType.Prohibited;
            else if (tagItem.Text.StartsWithIC("Each"))
                Type = RuleType.Each;

            //*** not implemented yet.
            if (Type == RuleType.Each)
                return;

            reader.Next();

            if (Type == RuleType.Prohibited)
            {
                tagItem = reader.Read();
                if (tagItem.Key == TaggedKey.Keyword)
                {
                    if (tagItem.IsText("a") || tagItem.IsText("an"))
                        reader.Next();
                }
            }

            Noun = new Noun();
            Noun.Parse(reader);

            var operGroup = new OperationGroup();
            operGroup.Parse(reader);

            if (operGroup.Items.Count > 1)
                Operation = operGroup;
            else if (operGroup.Items.Count == 1)
                Operation = operGroup.Items.First();
        }

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
