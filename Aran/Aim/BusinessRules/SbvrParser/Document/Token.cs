using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.Parser
{
    public class Tokens
    {
        private List<string> _tokens;
        private int[] _intervals;

        public Tokens()
        {
            _tokens = new List<string>();
            _intervals = new int[2];

            Init();
        }

        public TokenType GetTokenType(string token)
        {
            var index = _tokens.IndexOf(token.ToLower());
            if (index == -1)
                return TokenType.None;

            if (index < (int)TokenType.Keyword)
                return TokenType.Keyword;

            if (index < (int)TokenType.Verb)
                return TokenType.Verb;

            return TokenType.None;
        }

        private void Init()
        {
            _tokens.AddRange(new string[]
            {
                "Each",
                "It is obligatory that",
                "It is prohibited that",
                "a",
                "an",
                "and",
                "assigned",
                "at least * ",
                "at most *",
                "each",
                "equal-to",
                "exactly *",
                "has",
                "higher-than",
                "lower-or-equal-to",
                "lower-than",
                "more than *",
                "not",
                "or",
                "other",
                "other-than",
                "resolved-into",
                "shall",
                "than",
                "value",
                "with"
            });

            _intervals[(int)TokenType.Keyword] = _tokens.Count;


            _tokens.AddRange(new string[] {
                "specialisation",
                "has",
                "have" });

            _intervals[(int)TokenType.Verb] = _tokens.Count;

            for (var i = 0; i < _tokens.Count; i++)
                _tokens[i] = _tokens[i].ToLower();
        }
    }

    public enum TokenType { Keyword, Verb, None}

    public enum KeywordTypes
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
}
