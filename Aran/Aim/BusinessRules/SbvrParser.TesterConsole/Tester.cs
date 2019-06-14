using Aran.Aim;
using Aran.Aim.BusinessRules;
using Aran.Aim.BusinessRules.SbvrParser;
using Aran.Aim.Utilities;
using BusinessRules.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SbvrParser.TesterConsole
{
    public class Tester
    {
        public void Test()
        {
            //ParseTest();
            //TaggedDocumentTest();
            //CommandInfoJsonTest();
            OtherTest();

            Console.Read();
        }

        public void ParseTest()
        {
            var specialCase = "1A5285";

            var incorrectRules = new List<string>();

            using (var ruleDb = new RulesDb())
            {
                ruleDb.Open();

                foreach (var pair in RulesTester.GetRules())
                {
                    var taggedText = ruleDb.GetTaggedText(pair.Key);

                    var ttReader = new TaggedDocument();
                    ttReader.Init(taggedText);
                    ttReader.Next();

                    BRule parsedRule;

                    try
                    {
                        if (pair.Key != specialCase) continue;

                        parsedRule = new BRule();
                        parsedRule.Parse(ttReader, out var hasUnknown);

                        if (hasUnknown)
                        {
                            Console.WriteLine($"Rule has unknown keyword, id: {pair.Key}");
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Rule parse error, id: {pair.Key}. Details: {ex.Message}");
                        continue;
                    }

                    if (!parsedRule.Equals(pair.Value))
                        incorrectRules.Add(pair.Key);
                }

                ruleDb.Close();
            }

            Console.WriteLine("Done!");
            if (incorrectRules.Count > 0)
            {
                Console.WriteLine($"Failed rules. (Count{incorrectRules.Count}):");
                incorrectRules.ForEach((item) => { Console.WriteLine($"\tRule: {item}"); });
            }
        }

        public void TaggedDocumentTest()
        {
            var list = new List<string>();

            using (var ruleDb = new RulesDb())
            {
                ruleDb.Open();

                var taggedText = ruleDb.GetTaggedText("33070");

                var tgDoc = new TaggedDocument();
                tgDoc.Init(taggedText);

                ruleDb.Close();
            }
        }

        public void CommandInfoJsonTest()
        {
            var json = new CommandInfoList
            {
                Items =
                {
                    new CommandInfo
                    {
                        Command = "abc",
                        FeatureType = FeatureType.Airspace,
                        CommandValues =
                        {
                            7,
                            new object[] { "BASE" }
                        }
                    }
                }
            }.ToJson();

        }

        public void OtherTest()
        {
            var featTypePrefixList = AimMetadataUtility.GetUsedByFeatureTypeList(
                ObjectType.AirspaceLayer, ".", true, true);
        }
    }
}
