using Aran.Aim;
using Aran.Aim.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.BRules.Data;

namespace BRuleCheckerTester
{
    internal class BRuleCheckerTesterV3 : ITester
    {
        private string testPrefix = "Test_";

        public void Test()
        {
            var methods = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

            foreach (var method in methods)
            {
                if (method.Name.StartsWith(testPrefix) && method.ReturnType == typeof(TestParam))
                {
                    var tp = method.Invoke(null, null) as TestParam;

                    var searcher = new FeatureSearcher();
                    searcher.Open(tp.FeatProvider);

                    var resIdentifiers = searcher.Test(tp.FeatureType, tp.Command, tp.CommandValues);

                    foreach(var item in resIdentifiers)
                        Console.WriteLine($"{tp.FeatureType.ToString()}: {item}");

                    searcher.Close();
                }
            }
        }

        static TestParam _Test_1A8548()
        {
            var xmlFileName = @"C:\Users\AnarNft\Documents\WorkFiles\2018-02\171204_LGS_Test_Vienna\LGS_TransitionTest_20171204-20171206\Testdata\" +
                "LGS_20171205_09.xml";
            var dbPro = new AixmMessageProvider();
            dbPro.Open(xmlFileName);

            return new TestParam
            {
                FeatProvider = dbPro,
                FeatureType = FeatureType.ObstacleArea,
                Command =
                    "$AimIsAssigned$('obstacle') AND " +
                    "NOT $AimRefCount$('obstacle', :v1, 1)  ",
                CommandValues = new object[] { FeatureType.VerticalStructure }
            };
        }

        static TestParam _Test_26D18()
        {
            var xmlFileName = @"C:\Users\AnarNft\Documents\WorkFiles\2018-02\171204_LGS_Test_Vienna\LGS_TransitionTest_20171204-20171206\Testdata\" +
                "LGS_20171205_02.xml";
            var dbPro = new AixmMessageProvider();
            dbPro.Open(xmlFileName);

            return new TestParam
            {
                FeatProvider = dbPro,
                FeatureType = FeatureType.Airspace,
                Command =
                    "$AimPropCount$('geometryComponent', null, null) > 1 AND " +
                    "NOT ($AimPropCount$('geometryComponent.AirspaceGeometryComponent.operation', :v1, :v2) = 1) ",
                CommandValues = new object[] {
                    PropConditionType.Equal,
                    new object[] { "BASE" } }
            };
        }

        static TestParam _Test_19F870()
        {
            var xmlFileName = @"C:\Users\AnarNft\Documents\WorkFiles\2018-02\171204_LGS_Test_Vienna\LGS_TransitionTest_20171204-20171206\Testdata\" +
                "LGS_20171205_02.xml";
            var dbPro = new AixmMessageProvider();
            dbPro.Open(xmlFileName);

            return new TestParam
            {
                FeatProvider = dbPro,
                FeatureType = FeatureType.Airspace,
                Command =
                    "$AimPropCount$('geometryComponent', null, null) > 1 AND " +
                    "NOT ($AimIsAssigned$('geometryComponent.AirspaceGeometryComponent.operation')) "
            };
        }

        static TestParam _Test_19F488()
        {
            var xmlFileName = @"C:\Users\AnarNft\Documents\WorkFiles\2018-02\171204_LGS_Test_Vienna\LGS_TransitionTest_20171204-20171206\Testdata\" +
                "LGS_20171205_02.xml";
            var dbPro = new AixmMessageProvider();
            dbPro.Open(xmlFileName);

            return new TestParam
            {
                FeatProvider = dbPro,
                FeatureType = FeatureType.Airspace,
                Command =
                    "$AimPropCount$('geometryComponent', null, null) > 1 AND " +
                    "NOT ($AimIsAssigned$('geometryComponent.AirspaceGeometryComponent.operationSequence')) "
            };
        }

        static TestParam _Test_249F1()
        {
            var xmlFileName = @"C:\Users\AnarNft\Documents\WorkFiles\2018-02\171204_LGS_Test_Vienna\LGS_TransitionTest_20171204-20171206\Testdata\" +
                "LGS_20171205_02.xml";
            var dbPro = new AixmMessageProvider();
            dbPro.Open(xmlFileName);

            return new TestParam
            {
                FeatProvider = dbPro,
                FeatureType = FeatureType.StandardLevelTable,
                Command = ""
            };
        }

        static TestParam _Test_249F2()
        {
            var xmlFileName = @"C:\Users\AnarNft\Documents\WorkFiles\2018-02\171204_LGS_Test_Vienna\LGS_TransitionTest_20171204-20171206\Testdata\" +
                "LGS_20171205_02.xml";
            var dbPro = new AixmMessageProvider();
            dbPro.Open(xmlFileName);

            return new TestParam
            {
                FeatProvider = dbPro,
                FeatureType = FeatureType.StandardLevelColumn,
                Command = ""
            };
        }

        static TestParam _Test_D4A72()
        {
            var xmlFileName = @"C:\Users\AnarNft\Documents\WorkFiles\2018-02\171204_LGS_Test_Vienna\LGS_TransitionTest_20171204-20171206\Testdata\" +
                "LGS_20171205_02.xml";
            var dbPro = new AixmMessageProvider();
            dbPro.Open(xmlFileName);

            return new TestParam
            {
                FeatProvider = dbPro,
                FeatureType = FeatureType.RunwayDirection,
                Command =
                    "$AimIsAssigned$('elevationTDZ') AND " +
                    "$AimIsAssigned$('elevationTDZAccuracy') AND " +
                    "NOT ($AimEqualToProp$('elevationTDZ.uom', 'elevationTDZAccuracy.uom'))",
            };
        }

        static TestParam _Test_12B510()
        {
            var xmlFileName = @"C:\Users\AnarNft\Documents\WorkFiles\2018-02\171204_LGS_Test_Vienna\LGS_TransitionTest_20171204-20171206\Testdata\" +
                "LGS_20171205_02.xml";
            var dbPro = new AixmMessageProvider();
            dbPro.Open(xmlFileName);

            return new TestParam
            {
                FeatProvider = dbPro,
                FeatureType = FeatureType.Airspace,
                Command =
                    "NOT ($AimIsAssigned$('geometryComponent.AirspaceGeometryComponent.TheAirspaceVolume.AirspaceVolume.contributorAirspace')) AND " +
                    "NOT ($AimIsAssigned$('geometryComponent.AirspaceGeometryComponent.TheAirspaceVolume.AirspaceVolume.horizontalProjection')) AND " +
                    "NOT ($AimIsAssigned$('geometryComponent.AirspaceGeometryComponent.TheAirspaceVolume.AirspaceVolume.centreline')) ",
            };
        }

        static TestParam Test_135151()
        {
            var xmlFileName = @"C:\Users\AnarNft\Documents\WorkFiles\2018-02\171204_LGS_Test_Vienna\LGS_TransitionTest_20171204-20171206\Testdata\" +
                "LGS_20171205_02.xml";
            var dbPro = new AixmMessageProvider();
            dbPro.Open(xmlFileName);

            return new TestParam
            {
                FeatProvider = dbPro,
                FeatureType = FeatureType.OrganisationAuthority,
                Command = "$AimEqualTo$('type', :v1)",
                CommandValues = new object[] { new object[] { "STATE_GROUP", "STATE", "INTL_ORG" } }
            };
        }

        
        

    }
}
