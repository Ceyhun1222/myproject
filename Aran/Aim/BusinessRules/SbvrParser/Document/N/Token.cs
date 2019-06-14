using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.Parser.Document
{
    public class Token
    {
        public string Text { get; set; }

        public TokenId TokenId { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public override string ToString()
        {
            return string.Format("\"{0}\": {1} @ [{2}]", Text, TokenId, Line);
        }
    }

    public enum TokenId
    {
        //*** Keywords
        [Description("a")]
        A,

        [Description("an")]
        An,

        [Description("and")]
        And,

        [Description("assigned")]
        Assigned,

        #region "at least" AND "at most"
        [Description("at")]
        At,

        [Description("least")]
        Least,
        #endregion

        [Description("most")]
        Most,

        [Description("each")]
        Each,

        [Description("equal-to")]
        EqualTo,

        [Description("exactly")]
        Exactly,

        [Description("has")]
        Has,

        [Description("higher-than")]
        HigherThan,

        #region "it is obligatory that" AND "it is prohibited that"
        [Description("it")]
        It,
        [Description("is")]
        Is,
        [Description("obligatory")]
        Obligatory,
        [Description("prohibited")]
        Prohibited,
        [Description("that")]
        That,
        #endregion
        
        [Description("lower-or-equal-to")]
        LowerOrEqualTo,
        
        [Description("lower-than")]
        LowerThan,

        #region "more than"
        [Description("more")]
        More,
        [Description("than")]
        Than,
        #endregion

        [Description("not")]
        Not,

        [Description("or")]
        Or,

        [Description("other")]
        Other,

        [Description("other-than")]
        OtherThan,

        [Description("resolved-into")]
        ResolvedInto,

        [Description("shall")]
        Shall,

        [Description("value")]
        Value,

        [Description("with")]
        With,

        Eof,
        CharLit,
        StringLit,
        NumberLit,
        Ident,
        Undefined
    }
}
