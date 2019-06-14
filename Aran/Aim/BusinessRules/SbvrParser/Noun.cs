using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BRules.SbvrParser
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

        public void Parse(ITaggedReader reader)
        {
            reader.ReadAndNext();

            if (reader.LastRead.Key != TaggedKey.Noun)
                throw new InvalidTaggedException();

            Name = reader.LastRead.Text;

            reader.Read();

            if (reader.LastRead.IsEqual(TaggedKey.Verb, "specialisation"))
            {
                reader.Next();

                var child = new Noun();
                child.Parse(reader);

                Childs.Add(child);

                reader.Read();

                while (reader.LastRead.IsEqual(TaggedKey.Keyword, "or"))
                {
                    reader.Next();

                    child = new Noun();
                    child.Parse(reader);

                    Childs.Add(child);
                }
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Noun;
            if (other == null)
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
