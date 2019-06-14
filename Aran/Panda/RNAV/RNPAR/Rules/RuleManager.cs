using Aran.Panda.RNAV.RNPAR.Rules.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Panda.RNAV.RNPAR.Rules
{
    class RuleManager
    {
        private static Dictionary<string, IRule> Rules = new Dictionary<string, IRule>();

        public static IRule GetRule(string ruleName)
        {
            IRule rule = null;
            Rules.TryGetValue(ruleName,out rule);
            return rule;
        }


    }
}
