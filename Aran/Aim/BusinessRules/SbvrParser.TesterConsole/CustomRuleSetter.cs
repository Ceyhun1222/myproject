using Aran.Aim.BusinessRules;
using BusinessRules.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SbvrParser.TesterConsole
{
    class CustomRuleSetter
    {
        public void Set()
        {
            using (var ruleDb = new RulesDb())
            {
                ruleDb.Open();

                foreach(var ruleInfo in GetCustomRules())
                {
                    var cmdInfoList = BRuleSqlConverter.ToSqlCommand(ruleInfo.Item2);

                    if (cmdInfoList.Count > 0)
                    {
                        var json = new CommandInfoList(cmdInfoList).ToJson();

                        (ruleDb as IRuleDbSetter).SetSqlText("AIXM-5.1_RULE-" + ruleInfo.Item1, json, true);
                    }
                }
            }
        }

        private IEnumerable<Tuple<string, BRule>> GetCustomRules()
        {
            yield return new Tuple<string, BRule>("12B510", RulesTester.Custom_12B510());
        }
    }
}
