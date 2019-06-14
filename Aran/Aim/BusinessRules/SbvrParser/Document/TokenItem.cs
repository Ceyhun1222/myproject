using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.Parser.Document
{
    public class TokenItem
    {
        public TokenType Type { get; set; }

        public KeywordType Keyword { get; set; }

        public VerbType Verb { get; set; }

        public string Value { get; set; }
    }

    public enum TokenType { None, Keyword, Verb, Name }

    public enum KeywordType
    {
        Each,
        ItIsObligatoryThat,
        ItIsProhibitedThat,
        A,
        An,
        And,
        Assigned,
        AtLeast_,
        AtMost_,
        EqualTo,
        Exactly_,
        Has,
        HigherThan,
        LowerOrEqualTo,
        LowerThan,
        MoreThan_,
        Not,
        Or,
        Other,
        OtherThan,
        ResolvedInto,
        Shall,
        Than,
        Value,
        With
    }

    public enum VerbType
    {
        Specialisation,
        Has,
        Have
    }
}
