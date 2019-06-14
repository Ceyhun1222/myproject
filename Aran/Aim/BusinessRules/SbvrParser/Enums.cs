using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.SbvrParser
{
    public enum TaggedKey
    {
        [Description("~")]
        None,

        [Description("keyword")]
        Keyword,

        [Description("NounConcept")]
        Noun,

        [Description("Verb-concept")]
        Verb,

        [Description("Name")]
        Name,

        [Description("font")]
        Font,

        [Description("")]
        Unknown
    }
}
