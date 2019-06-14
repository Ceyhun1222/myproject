using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BRules.SbvrParser
{
    [DataContract]
    public class OperAssociation
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Noun Noun { get; set; }

        public void Parse(ITaggedReader reader)
        {
            var ti = reader.Read();
            if (ti.Key != TaggedKey.Verb)
                throw new InvalidTaggedException();
            Name = ti.Text;

            reader.Next();

            Noun = new Noun();
            Noun.Parse(reader);
        }

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
