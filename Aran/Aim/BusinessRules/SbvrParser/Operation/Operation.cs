using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BRules.SbvrParser
{
    [DataContract]
    public class Operation : AbstractOperation
    {
        public Operation()
        {
            OperType = AbstractOperationType.Operation;
        }

        [DataMember]
        public bool IsNot { get; set; }

        [DataMember]
        public OperationType Type { get; set; }

        [DataMember]
        public string Noun { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public OperAssociation Association { get; set; }

        public void Parse(ITaggedReader reader)
        {
            IsNot = reader.LastRead.IsEqual(TaggedKey.Keyword, "not");
            if (IsNot)
                reader.Next();

            bool isAssingedExists = reader.LastRead.IsEqual(TaggedKey.Keyword, "assigned");
            if (isAssingedExists)
            {
                reader.Next();
            }
            else
            {
                for(var i = 0; i < quantificationKeywordPairs.Length; i += 1)
                {
                    var kw = quantificationKeywordPairs[i].ToString();

                    if (reader.LastRead.Text.StartsWithIC(kw))
                    {
                        Type = (OperationType)quantificationKeywordPairs[i + 1];
                        Value = reader.LastRead.Text.Substring(kw.Length + 1).Trim();
                        reader.Next();

                        if (reader.LastRead.Key != TaggedKey.Noun)
                            throw new InvalidTaggedException("After Quantification keywords must follow Noun");

                        Noun = reader.LastRead.Text;
                        return;
                    }
                }
            }






            //logicType = null;

            //var tagItem = reader.Read();

            //if (tagItem.Key == TaggedKey.Keyword)
            //{
            //    if (tagItem.IsText("with assigned"))
            //    {
            //        reader.Next();
            //        logicType = LogicType.And;
            //        Type = OperationType.Assigned;

            //        tagItem = reader.ReadAndNext();
            //        if (tagItem.Key != TaggedKey.Noun)
            //            throw new InvalidTaggedException();

            //        Noun = tagItem.Text;

            //        tagItem = reader.ReadAndNext();
            //        if (!tagItem.IsEqual(TaggedKey.Keyword, "value"))
            //            throw new InvalidTaggedException();

            //        tagItem = reader.Read();

            //        if (tagItem.IsEqual(TaggedKey.Verb, "has"))
            //            logicType = LogicType.And;
            //    }
            //}
            //else if (tagItem.Key == TaggedKey.Verb)
            //{
            //    if (tagItem.IsText("has"))
            //    {
            //        reader.Next();

            //        tagItem = reader.ReadAndNext();

            //        if (tagItem.Key != TaggedKey.Noun)
            //            throw new InvalidTaggedException();

            //        Noun = tagItem.Text;

            //        tagItem = reader.ReadAndNext();
            //        if (tagItem.Key != TaggedKey.Keyword)
            //            throw new InvalidTaggedException();

            //        if (tagItem.Text.StartsWithIC("value "))
            //        {
            //            var s = tagItem.Text.Substring(6).Trim();
            //            if (s.StartsWithIC("resolved-into "))
            //            {
            //                Type = OperationType.ResolvedInto;
            //                Value = s.Substring(14).Trim();

            //                Association = new OperAssociation();
            //                Association.Parse(reader);
            //            }
            //        }
            //        else
            //        {

            //        }
            //    }
            //}

            //if (tagItem.IsEqual(TaggedKey.Verb, "and"))
            //{
            //    reader.Next();
            //    logicType = LogicType.And;
            //}
            //else if (tagItem.IsEqual(TaggedKey.Verb, "or"))
            //{
            //    reader.Next();
            //    logicType = LogicType.Or;
            //}
        }


        public void Parse_OLD(ITaggedReader reader, out LogicType? logicType)
        {
            logicType = null;

            var tagItem = reader.Read();

            if (tagItem.Key == TaggedKey.Keyword)
            {
                if (tagItem.IsText("with assigned"))
                {
                    reader.Next();
                    logicType = LogicType.And;
                    Type = OperationType.Assigned;

                    tagItem = reader.ReadAndNext();
                    if (tagItem.Key != TaggedKey.Noun)
                        throw new InvalidTaggedException();
                    
                    Noun = tagItem.Text;
                    
                    tagItem = reader.ReadAndNext();
                    if (!tagItem.IsEqual(TaggedKey.Keyword, "value"))
                        throw new InvalidTaggedException();

                    tagItem = reader.Read();

                    if (tagItem.IsEqual(TaggedKey.Verb, "has"))
                        logicType = LogicType.And;
                }
            }
            else if (tagItem.Key == TaggedKey.Verb)
            {
                if (tagItem.IsText("has"))
                {
                    reader.Next();

                    tagItem = reader.ReadAndNext(); 

                    if (tagItem.Key != TaggedKey.Noun)
                        throw new InvalidTaggedException();

                    Noun = tagItem.Text;

                    tagItem = reader.ReadAndNext();
                    if (tagItem.Key != TaggedKey.Keyword)
                        throw new InvalidTaggedException();

                    if (tagItem.Text.StartsWithIC("value "))
                    {
                        var s = tagItem.Text.Substring(6).Trim();
                        if (s.StartsWithIC("resolved-into "))
                        {
                            Type = OperationType.ResolvedInto;
                            Value = s.Substring(14).Trim();

                            Association = new OperAssociation();
                            Association.Parse(reader);
                        }
                    }
                    else
                    {

                    }
                }
            }

            if (tagItem.IsEqual(TaggedKey.Verb, "and"))
            {
                reader.Next();
                logicType = LogicType.And;
            }
            else if (tagItem.IsEqual(TaggedKey.Verb, "or"))
            {
                reader.Next();
                logicType = LogicType.Or;
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Operation;
            if (other == null)
                return false;

            return Global.IsObjectsEquals(
                IsNot, other.IsNot,
                Type, other.Type,
                Noun, other.Noun,
                Value, other.Value,
                Association, other.Association);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private static object[] quantificationKeywordPairs =
        {
            "at least", OperationType.AtLeast,
            "at most", OperationType.AtMost,
            "exactly one", OperationType.ExactlyOne,
            "more than one", OperationType.MoreThanOne
        };
    }
}
