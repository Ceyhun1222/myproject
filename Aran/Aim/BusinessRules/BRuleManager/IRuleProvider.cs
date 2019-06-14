using SbvrParser.TesterConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRuleManager
{
    public interface IRuleProvider
    {
        List<BRuleInfo> GetRules();
    }
}
