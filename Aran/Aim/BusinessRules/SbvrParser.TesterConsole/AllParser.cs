using Aran.Aim.BusinessRules;
using Aran.Aim.BusinessRules.SbvrParser;
using BusinessRules.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SbvrParser.TesterConsole
{
    internal class AllParser
    {
        public void Parse()
        {
            var ruleSuccesCount = 0;
            var ruleFailedCount = 0;
            var sqlSuccesCount = 0;
            var sqlFailedCount = 0;

            using (var ruleDb = new RulesDb())
            {
                ruleDb.Open();

                var ruleInfos = ruleDb.GetAllTaggedDescriptions(RuleFilterProfileType.Both);

                foreach (var ruleInfo in ruleInfos)
                {
                    ////***DEBUG
                    //if (ruleInfo.Uid != "AIXM-5.1_RULE-9F6CB")
                    //    continue;

                    var td = new TaggedDocument();
                    td.Init(ruleInfo.TaggedDescription);
                    td.Next();

                    try
                    {
                        var brule = new BRule();
                        brule.Parse(td, out var isIncomplate);

                        if (isIncomplate)
                        {
                            ruleFailedCount++;
                            (ruleDb as IRuleDbSetter).SetSqlText(ruleInfo.Uid, null);
                            continue;
                        }

                        ruleSuccesCount++;

                        try
                        {
                            var cmdInfoList = BRuleSqlConverter.ToSqlCommand(brule);

                            if (cmdInfoList.Count > 0)
                            {
                                var json = new CommandInfoList(cmdInfoList).ToJson();
                                (ruleDb as IRuleDbSetter).SetSqlText(ruleInfo.Uid, json);
                            }

                            sqlSuccesCount++;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Error: " + ex.Message);
                            sqlFailedCount++;
                        }
                    }
                    catch
                    {
                        ruleFailedCount++;
                    }
                }

                Console.WriteLine(
                    "Parse result:\n" +
                        "\tSucces: {0}\n" +
                        "\tFailed: {1}\n" +
                    "Command result:\n" +
                        "\tSucces: {2}\n" +
                        "\tFailed: {3}", 
                    ruleSuccesCount, 
                    ruleFailedCount, 
                    sqlSuccesCount, 
                    sqlFailedCount);
            }
        }
    }
}
