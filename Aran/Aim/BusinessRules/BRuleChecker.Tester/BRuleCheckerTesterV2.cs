using Aran.Aim.BusinessRules;
using Aran.Aim.Features;
using SbvrParser.TesterConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRuleCheckerTester
{
    public class BRuleCheckerTesterV2 : ITester
    {
        public void Test()
        {
            var searcher = new FeatureSearcher();

            var tp = Test7();

            searcher.Open(tp.FeatProvider);

            var res = searcher.Check(tp.Rule);

            searcher.Close();

            foreach(var item in res)
            {
                var feat = tp.FeatProvider.GetFeature(item.Item1, item.Item2);
            }
        }

        static TestParamV2 Test7()
        {
            var oaList = new List<ObstacleArea>();
            for (var i = 0; i < 3; i++)
                oaList.Add(BRuleCheckerTesterV1.CreateObstacleArea(i + 1));

            var vs = BRuleCheckerTesterV1.CreateVerticalStructure(0);

            oaList[0].Obstacle.Add(new Aran.Aim.Objects.FeatureRefObject(vs.Identifier));
            oaList[2].Obstacle.Add(new Aran.Aim.Objects.FeatureRefObject(vs.Identifier));

            var listPro = new FeatureListProvider();
            listPro.InsertRange(oaList);
            listPro.Insert(vs);

            //var vsList = new List<VerticalStructure>();
            //for (var i = 0; i < 10; i++)
            //    vsList.Add(BRuleCheckerTester.CreateVerticalStructure(i));

            //for (var i = 0; i < 10; i++)
            //{
            //    var oa = oaList[i];
            //    var vs = vsList[i];

            //    oa.Obstacle.Add(new Aran.Aim.Objects.FeatureRefObject(vs.Identifier));
            //}

            //oaList[2].Obstacle.Clear();
            //oaList[4].Obstacle.Clear();
            //oaList[6].Obstacle.Add(new Aran.Aim.Objects.FeatureRefObject(vsList[2].Identifier));

            //var listPro = new FeatureListProvider();
            //listPro.InsertRange(oaList);
            //listPro.InsertRange(vsList);

            return new TestParamV2
            {
                FeatProvider = listPro,
                Rule = RulesTester.Get_1A8548()
            };
        }
    }

    public class TestParamV2
    {
        public IFeatureProvider FeatProvider { get; set; }

        public BRule Rule { get; set; }
    }
}
