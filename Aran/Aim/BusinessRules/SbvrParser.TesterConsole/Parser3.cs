using Aran.Aim.BusinessRules;
using Aran.Aim.BusinessRules.SbvrParser;
using BusinessRules.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SbvrParser.TesterConsole
{
    public class Parser3
    {
        public Parser3()
        {
        }

        public void SbvrToBRuleData(string bruleDataDir)
        {
            var ruleSuccesCount = 0;
            var ruleFailedCount = 0;

            using (var ruleDb = new RulesDb())
            {
                ruleDb.Open();

                var ruleInfos = ruleDb.GetAllTaggedDescriptions(RuleFilterProfileType.Both);

                foreach (var ruleInfo in ruleInfos)
                {
                    ////***DEBUG
                    //if (ruleInfo.Item1 != "AIXM-5.1_RULE-1A3321")
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
                            continue;
                        }

                        var filePath = Path.Combine(bruleDataDir, ruleInfo.Uid + ".json");

                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            var ser = new DataContractJsonSerializer(typeof(BRule));
                            ser.WriteObject(stream, brule);
                        }

                        ruleSuccesCount++;
                    }
                    catch
                    {
                        ruleFailedCount++;
                    }
                }

                Console.WriteLine(
                    "Parse result:\n" +
                        "\tSucces: {0}\n" +
                        "\tFailed: {1}",
                    ruleSuccesCount,
                    ruleFailedCount);
            }

        }

        public void Test(string fileName)
        {
            using (var stream = File.OpenRead(fileName))
            {
                var ser = new DataContractJsonSerializer(typeof(BRule));
                var brule = ser.ReadObject(stream) as BRule;

                var cmdInfoList = BRuleSqlConverter.ToSqlCommand(brule);

                var json = new CommandInfoList(cmdInfoList).ToJson();
            }
        }
    }
}
