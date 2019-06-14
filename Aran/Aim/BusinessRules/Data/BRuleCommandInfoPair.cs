using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRules.Data
{
    public class BRuleCommandInfoPair
    {
        public BRuleInfo Rule { get; set; }
        public string CommandsJson { get; set; }
    }
}
