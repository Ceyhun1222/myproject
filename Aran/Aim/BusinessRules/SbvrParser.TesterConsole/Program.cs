using BRuleCheckerTester;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SbvrParser.TesterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //CheckCustomData();
            //return;

            //new Parser3().SbvrToBRuleData(@"C:\Users\AnarNft\Documents\BusinessRules\Data\BRuleData");

            //new Parser3().Test(@"C:\Users\AnarNft\Documents\BusinessRules\Data\BRuleData\AIXM-5.1_RULE-156C62.json");
            //return;

            //var sbvrSourceFile = @"C:/Users/AnarNft/Documents/BusinessRules/Test/1A5250.sbvr";
            //using (var fs = new FileStream(sbvrSourceFile, FileMode.Open))
            //{
            //    var lexer = new Aran.Aim.BusinessRules.Parser.Document.Lexer();
            //    lexer.Run(fs);
            //}
            //return;

            //Parser2();

            //new Tester().Test();

            new AllParser().Parse();
            return;

            //new CustomRuleSetter().Set();

            if (!ParseArgument(args, out var xmlFileName, out var reportFileName))
                return;

            CheckAixmMessageData(args, xmlFileName, reportFileName);

            //CheckCustomData();
        }

        static void CheckAixmMessageData(string[] args, string xmlFileName, string reportFileName)
        {
            var dataChecker = new DataChecker();
            var report = dataChecker.CheckAixmMessage(xmlFileName);

            using (var stream = new FileStream(reportFileName, FileMode.Create, FileAccess.Write))
            {
                var ser = new DataContractJsonSerializer(typeof(BRuleReportList));
                
                ser.WriteObject(stream, report);
                stream.Close();
            }

            Console.WriteLine("Report saved. Rules count: " + report.Items.Count);
        }

        static void CheckCustomData()
        {
            var dataChecker = new DataChecker();
            var cdc = new CustomDataCreator();
            var resultGuidList = new List<Guid>();
            var featList = cdc.Create(Aran.Aim.FeatureType.TouchDownLiftOff, resultGuidList);

            var featProvider = new FeatureListProvider();
            featProvider.InsertRange(featList);

            var reportList = dataChecker.CheckProvider(featProvider);

            var isEqual = (reportList.Items.Count == resultGuidList.Count);
            if (isEqual)
            {
                foreach (var ruleReportItem in reportList.Items)
                {
                    foreach (var reportFeature in ruleReportItem.Features)
                    {
                        foreach (var reportIdentifier in reportFeature.Identifiers)
                        {
                            isEqual = resultGuidList.Contains(reportIdentifier);
                            if (!isEqual)
                                break;
                        }
                    }
                }
            }

            Console.WriteLine("IsEqual: " + isEqual);
        }

        static bool ParseArgument(string[] args, out string xmlFileName, out string reportFileName)
        {
            xmlFileName = string.Empty;
            reportFileName = string.Empty;

            for (var i = 0; i < args.Length; i++)
            {
                var key = args[i];

                if (key == "-xml")
                {
                    if (i == args.Length - 1)
                    {
                        Console.WriteLine("Ivalid argument -xml. (usage: -xml \"C:\aixm_message.xml\"");
                        return false;
                    }
                    xmlFileName = args[i + 1];
                }
                else if (key == "-report")
                {
                    if (i == args.Length - 1)
                    {
                        Console.WriteLine("Ivalid argument -report. (usage: -report \"C:\rule_report.json\"");
                        return false;
                    }
                    reportFileName = args[i + 1];
                }
            }

            return true;
        }

        static void Parser2()
        {
            var parser = new Parser2();
            parser.Parse();
        }
    }
}
