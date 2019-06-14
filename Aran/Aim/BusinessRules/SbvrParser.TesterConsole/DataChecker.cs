using Aran.Aim;
using Aran.Aim.BRules.Data;
using Aran.Aim.BusinessRules;
using BusinessRules.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SbvrParser.TesterConsole
{
    public class DataChecker
    {
        public BRuleReportList CheckAixmMessage(string xmlFileName)
        {
            if (!System.IO.File.Exists(xmlFileName))
            {
                Console.WriteLine("File is not exists: " + xmlFileName);
                return new BRuleReportList();
            }

            Console.WriteLine("Loading AIXM Message file...");

            var dbPro = new AixmMessageProvider();
            dbPro.Open(xmlFileName);

            Console.WriteLine("Checking Business rules...");

            return CheckProvider(dbPro);
        }

        public BRuleReportList CheckProvider(IFeatureProvider featProvider)
        {
            var result = new List<Tuple<BRuleInfo, List<Tuple<FeatureType, Guid>>>>();

            using (var searcher = new FeatureSearcher())
            {
                searcher.Open(featProvider);

                using (var ruleDb = new RulesDb())
                {
                    ruleDb.Open();

                    var commandInfos = ruleDb.GetRuleAndCommandInfos(new RuleFilterValues {
                        { RuleFilterType.IsActive, true }
                    });

                    foreach (var ruleCmdInfoPair in commandInfos)
                    {
                        var cil = CommandInfoList.FromJson(ruleCmdInfoPair.CommandsJson);
                        var resIdentifiers = searcher.Check(cil.Items);

                        if (resIdentifiers != null && resIdentifiers.Count > 0)
                            result.Add(new Tuple<BRuleInfo, List<Tuple<FeatureType, Guid>>>(ruleCmdInfoPair.Rule, resIdentifiers));
                    }
                }
            }

            return WriteToTextFile(result);
        }

        static BRuleReportList WriteToTextFile(List<Tuple<BRuleInfo, List<Tuple<FeatureType, Guid>>>> report)
        {
            var ruleReportlist = new BRuleReportList();

            foreach (var item in report)
            {
                var ruleInfo = item.Item1;
                var features = item.Item2;

                var ruleReport = new BRuleReport
                {
                    Uid = ruleInfo.Uid,
                    Name = ruleInfo.Name,
                    Profile = ruleInfo.Profile,
                    Description = ruleInfo.Comment
                };

                var dict = new Dictionary<FeatureType, List<Guid>>();

                foreach (var featTypeGuid in features)
                {
                    if (!dict.TryGetValue(featTypeGuid.Item1, out var guidList))
                    {
                        guidList = new List<Guid>();
                        dict.Add(featTypeGuid.Item1, guidList);
                    }
                    guidList.Add(featTypeGuid.Item2);
                }

                foreach (var featType in dict.Keys)
                {
                    var rfr = new BRuleFeatureReport { FeatureType = featType.ToString() };
                    rfr.Identifiers.AddRange(dict[featType]);
                    ruleReport.Features.Add(rfr);
                }

                ruleReportlist.Items.Add(ruleReport);
            }

            return ruleReportlist;
        }
    }
}
