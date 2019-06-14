using Aran.Aim.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.SbvrParser
{
    public static class SbvrParserExtensions
    {
        public static bool StartsWithIC(this string text, string value)
        {
            return text.StartsWith(value, StringComparison.CurrentCultureIgnoreCase);
        }

        public static int IndexOfIC(this string text, string value, int startIndex = 0)
        {
            return text.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
        }

        public static string GetNoun(this TaggedItem ti)
        {
            if (ti.Key != TaggedKey.Noun)
                throw new InvalidTaggedException("Noun expected");
            return ti.Text;
        }

        public static void Parse(this BRule rule, TaggedDocument reader, out bool isIncomplate)
        {
            if (reader.Current.Key != TaggedKey.Keyword)
                throw new InvalidTaggedException();

            if (reader.Current.Text.StartsWithIC("It is obligatory"))
                rule.Type = RuleType.Obligatory;
            else if (reader.Current.Text.StartsWithIC("It is prohibited that"))
                rule.Type = RuleType.Prohibited;
            else if (reader.Current.Text.StartsWithIC("Each"))
                rule.Type = RuleType.Each;

            reader.Next();

            if (rule.Type == RuleType.Prohibited)
            {
                if (reader.Current.Key == TaggedKey.Keyword)
                {
                    if (reader.Current.IsText("a") || reader.Current.IsText("an"))
                    {
                        reader.Next();
                    }
                    else if (reader.Current.IsText("at least one"))
                    {
                        reader.Next();

                        rule.Noun = new Noun();
                        rule.Noun.Parse(reader);

                        if (reader.IsLast)
                        {
                            isIncomplate = false;
                            return;
                        }
                    }
                }
            }
            else if (rule.Type == RuleType.Obligatory)
            {
                if (reader.Current.Key == TaggedKey.Keyword)
                {
                    if (reader.Current.IsText("each"))
                        reader.Next();
                }
            }

            rule.Noun = new Noun();
            rule.Noun.Parse(reader);

            if (rule.Type == RuleType.Each)
            {
                if (reader.Current.IsEqual(TaggedKey.Keyword, "shall"))
                    reader.Next();
            }

            var operGroup = new OperationGroup();
            operGroup.Parse(reader, out isIncomplate);

            if (operGroup.Items.Count > 1)
                rule.Operation = operGroup;
            else if (operGroup.Items.Count == 1)
                rule.Operation = operGroup.Items.First();
        }

        public static void Parse(this Noun noun, TaggedDocument reader)
        {
            if (reader.Current.Key != TaggedKey.Noun)
                throw new InvalidTaggedException();

            noun.Name = reader.Current.Text;

            reader.Next();

            if (reader.Current.IsEqual(TaggedKey.Verb, "specialisation"))
            {
                reader.Next();

                var child = new Noun();
                child.Parse(reader);

                noun.Childs.Add(child);

                while (reader.Current.IsEqual(TaggedKey.Keyword, "or"))
                {
                    reader.Next();

                    child = new Noun();
                    child.Parse(reader);

                    noun.Childs.Add(child);
                }
            }
        }

        public static void Parse(this OperationGroup operGr, TaggedDocument reader, out bool isIncomplate)
        {
            bool isWith;
            bool isTypeAssigned = false;
            operGr.Type = LogicType.And;
            isIncomplate = false;

            bool isNot;

            while (true)
            {
                isWith = false;
                isNot = false;

                if (reader.Current.IsEqual(TaggedKey.Keyword, "not"))
                {
                    isNot = true;
                    reader.Next();
                }

                if (reader.Current.IsEqual(TaggedKey.Verb, "have") ||
                    reader.Current.IsEqual(TaggedKey.Verb, "has"))
                {
                    reader.Next();

                    var oper = new Operation();
                    oper.IsNot = isNot;
                    oper.Parse(reader);
                    operGr.Items.Add(oper);
                }
                else if (reader.Current.IsEqual(TaggedKey.Keyword, "with"))
                {
                    isWith = true;

                    reader.Next();

                    var oper = new Operation();
                    oper.IsWith = true;
                    oper.IsNot = isNot;
                    oper.Parse(reader);
                    operGr.Items.Add(oper);
                }
                else
                {
                    if (!reader.IsLast)
                        isIncomplate = true;
                    break;
                }


                if (reader.Current.IsEqual(TaggedKey.Keyword, "and"))
                {
                    if (!isTypeAssigned)
                        operGr.Type = LogicType.And;
                    else if (operGr.Type != LogicType.And)
                        throw new InvalidTaggedException("Different logic types is not allowed.");

                    while (reader.Current.IsEqual(TaggedKey.Keyword, "and"))
                    {
                        reader.Next();
                        try
                        {
                            var oper = new Operation { IsWith = isWith };
                            oper.Parse(reader);
                            operGr.Items.Add(oper);
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
                else if (reader.Current.IsEqual(TaggedKey.Keyword, "or"))
                {
                    if (!isTypeAssigned)
                        operGr.Type = LogicType.Or;
                    else if (operGr.Type != LogicType.Or)
                        throw new InvalidTaggedException("Different logic types is not allowed.");

                    while (reader.Current.IsEqual(TaggedKey.Keyword, "or"))
                    {
                        reader.Next();
                        try
                        {
                            var oper = new Operation { IsWith = isWith };
                            oper.Parse(reader);
                            operGr.Items.Add(oper);
                        }
                        catch
                        {
                            break;
                        }
                    }
                }


                if (reader.Current.IsEqual(TaggedKey.Keyword, "shall"))
                {
                    if (!isTypeAssigned)
                        operGr.Type = LogicType.And;
                    else if (operGr.Type != LogicType.And)
                        throw new InvalidTaggedException("Different logic types is not allowed.");

                    reader.Next();
                }
                else if (reader.Current.Key == TaggedKey.Unknown)
                {
                    isIncomplate = true;
                    break;
                }
                else if (!isWith)
                {
                    if (!reader.IsLast)
                        isIncomplate = true;
                    break;
                }

                isTypeAssigned = true;
            }
        }

        public static void Parse(this Operation oper, TaggedDocument reader)
        {
            var isNot = reader.Current.IsEqual(TaggedKey.Keyword, "not");
            if (isNot)
            {
                reader.Next();
                oper.IsNot = !oper.IsNot;
            }

            bool isAssingedExists = reader.Current.IsEqual(TaggedKey.Keyword, "assigned");

            if (isAssingedExists)
            {
                reader.Next();
            }
            else
            {
                for (var i = 0; i < quantificationKeywordPairs.Length; i += 2)
                {
                    var kw = quantificationKeywordPairs[i].ToString();

                    if (reader.Current.Text.StartsWithIC(kw))
                    {
                        oper.Type = (OperationType)quantificationKeywordPairs[i + 1];
                        if (reader.Current.Text.Length > kw.Length)
                            oper.Value = reader.Current.Text.Substring(kw.Length + 1).Trim();

                        reader.Next();

                        if (reader.Current.Key != TaggedKey.Noun)
                            throw new InvalidTaggedException("After Quantification keywords must follow Noun");

                        oper.Noun = reader.Current.Text;
                        reader.Next();

                        if (reader.Current.Key == TaggedKey.Keyword)
                        {
                            isNot = reader.Current.IsText("not");
                            var currIndex = isNot ? reader.CurrentIndex + 1 : reader.CurrentIndex;
                            
                            var tgItem1 = reader[currIndex];
                            var tgItem2 = reader[currIndex + 1];

                            if (tgItem1 != null && tgItem2 != null && 
                                tgItem1.Key == TaggedKey.Keyword && 
                                tgItem2.Key == TaggedKey.Name)
                            {
                                for(var j = 0; j < innerOperationKeywordPairs.Length; j += 2)
                                {
                                    var ikw = innerOperationKeywordPairs[j].ToString();
                                    if (tgItem1.Text.StartsWithIC(ikw))
                                    {
                                        oper.InnerOper = new InnerOperation
                                        {
                                            IsNot = isNot,
                                            Type = (InnerOperationType)innerOperationKeywordPairs[j + 1],
                                            Value = tgItem2.Text
                                        };

                                        reader.Next();
                                        reader.Next();
                                    }
                                }
                            }
                        }

                        return;
                    }
                }
            }

            oper.Noun = reader.Current.GetNoun();

            reader.Next();

            if (reader.Current.IsText("value"))
                reader.Next();

            if (reader.Current.IsEqual(TaggedKey.Keyword, "not"))
            {
                oper.IsNot = !oper.IsNot;
                reader.Next();
            }

            for (var i = 0; i < operationKeywordPairs.Length; i += 2)
            {
                var kw = operationKeywordPairs[i].ToString();
                if (reader.Current.IsText(kw))
                {
                    oper.Type = (OperationType)operationKeywordPairs[i + 1];
                    reader.Next();

                    if (string.IsNullOrEmpty(reader.Current.Text))
                        throw new InvalidTaggedException("After operational keyword expected text");

                    if (reader.Current.Key == TaggedKey.Noun)
                        oper.Value = "$prop$" + reader.Current.Text;
                    else
                        oper.Value = reader.Current.Text;

                    reader.Next();

                    if (oper.Type == OperationType.ResolvedInto)
                    {
                        oper.Association = new OperAssociation();
                        oper.Association.Parse(reader);
                    }

                    return;
                }
            }

            if (reader.Current.Key == TaggedKey.Unknown)
            {
                if (ParseUnknownType(reader.Current.Text, 
                    out ExpressedType expType, out int expValue, out string expValueType))
                {
                    oper.Type = OperationType.Expressed;
                    oper.ExpressedType = expType;
                    oper.Value = string.Format($"{expValue};{expValueType}");
                    reader.Next();
                }
                else
                {
                    oper.Type = OperationType.Undefined;
                    oper.Value = reader.Current.Text;
                }

                return;
            }

            if (isAssingedExists)
                oper.Type = OperationType.Assigned;
        }

        public static void Parse(this OperAssociation operAssoc, TaggedDocument reader)
        {
            if (reader.Current.Key != TaggedKey.Verb)
                throw new InvalidTaggedException();
            operAssoc.Name = reader.Current.Text;

            reader.Next();

            operAssoc.Noun = new Noun();
            operAssoc.Noun.Parse(reader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="reader"></param>
        /// <param name="value"></param> /// <param name="value"></param>
        /// <param name="valueType">{decimals, digits, letters}</param>
        /// <returns></returns>
        private static bool ParseUnknownType(string text, 
            out ExpressedType expType, 
            out int value, 
            out string valueType)
        {
            expType = ExpressedType.None;
            value = 0;
            valueType = string.Empty;

            if (text.StartsWith("expressed", StringComparison.InvariantCultureIgnoreCase))
            {
                var tokens = text.Split(" \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if(IsArrayItemEquals(tokens, 1, "with"))
                {
                    int parseValue = -1;

                    if (IsArrayItemEquals(tokens, 2, "more", "than"))
                    {
                        parseValue = 4;
                        expType = ExpressedType.More;
                    }
                    else if (IsArrayItemEquals(tokens, 2, "less", "than"))
                    {
                        parseValue = 4;
                        expType = ExpressedType.Less;
                    }
                    else if (IsArrayItemEquals(tokens, 2, "exactly"))
                    {
                        parseValue = 3;
                        expType = ExpressedType.Exactly;
                    }

                    if (parseValue >= 0)
                    {
                        if (int.TryParse(tokens[parseValue], out int temp))
                        {
                            value = temp;

                            if (IsArrayItemEquals(tokens, parseValue + 1, "decimals"))
                                //valueType = "decimals";
                                valueType = string.Empty;  ///*** to sql function not implemented. Must check perented time.
                            else if (IsArrayItemEquals(tokens, parseValue + 1, "digits"))
                                valueType = "digits";
                            else if (IsArrayItemEquals(tokens, parseValue + 1, "letters"))
                                valueType = "letters";
                        }
                    }
                }
            }

            return (expType != ExpressedType.None &&
                valueType != string.Empty);
        }

        private static bool IsArrayItemEquals(string[] strArr, int index, params string[] texts)
        {
            if (strArr == null || texts == null || texts.Length == 0)
                return false;

            for(var i = 0; i < texts.Length; i++)
            {
                if (!(strArr.Length > index &&
                    string.Equals(strArr[index + i], texts[i], StringComparison.InvariantCultureIgnoreCase)))
                {
                    return false;
                }
            }
            return true;
        }


        private static object[] quantificationKeywordPairs =
        {
            "at least", OperationType.AtLeast,
            "at most", OperationType.AtMost,
            "exactly one", OperationType.ExactlyOne,
            "more than one", OperationType.MoreThanOne
        };

        private static object[] operationKeywordPairs =
        {
            "equal-to", OperationType.Equal,
            "higher-than", OperationType.Higher,
            "higher-or-equal-to", OperationType.HigherEqual,
            "lower-than", OperationType.Lower,
            "lower-or-equal-to", OperationType.LowerEqual,
            "resolved-into", OperationType.ResolvedInto,
            "other-than", OperationType.OtherThan
        };

        private static object[] innerOperationKeywordPairs =
        {
            "equal-to", InnerOperationType.Equal
        };
    }

    internal static class UnknownKeyParser
    {

    }

    internal class UnknownKey
    {
        public UnknownKeyType Type { get; set; }
    }

    internal enum UnknownKeyType
    {
        None,
        Number,
        UomValue,
        Oper
    }
}
