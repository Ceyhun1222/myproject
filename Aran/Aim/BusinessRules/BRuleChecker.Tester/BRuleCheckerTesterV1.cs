using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.BusinessRules;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using SbvrParser.TesterConsole;

namespace BRuleCheckerTester
{
    public class BRuleCheckerTesterV1 : ITester
    {
        private string testPrefix = "Test_";

        public void Test()
        {
            var searcher = new FeatureSearcher();

            var methods = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
            var testParamType = typeof(TestParam);

            var failedMethodList = new List<string>();

            foreach(var method in methods)
            {
                if (method.Name.StartsWith(testPrefix) && method.ReturnType == testParamType)
                {
                    var tp = method.Invoke(null, null) as TestParam;

                    searcher.Open(tp.FeatProvider);

                    var resIdentifiers = searcher.Test(tp.FeatureType, tp.Command, tp.CommandValues);

                    var isEqual = resIdentifiers.Count == tp.Result.Count;

                    if (isEqual)
                    {
                        foreach (var item in tp.Result)
                        {
                            isEqual = resIdentifiers.Contains(item);
                            if (!isEqual)
                                break;
                        }
                    }

                    searcher.Close();

                    if (!isEqual)
                        failedMethodList.Add(method.Name);
                }
            }

            if (failedMethodList.Count > 0)
            {
                Console.WriteLine("Failed methods:");
                foreach (var item in failedMethodList)
                    Console.WriteLine("\t" + item);
            }
        }

        static TestParam Test_IsAssigned()
        {
            var listPro = new FeatureListProvider();
            var list = new List<AirportHeliport>();
            for (var i = 1; i <= 10; i++)
                list.Add(CreateAirportHeliport(i));

            list[1].ServedCity.Add(new City { });
            list[3].ServedCity.Add(new City { Name = "AH-3->City-" + 2 });

            listPro.InsertRange(list);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.AirportHeliport,
                Command = "$AimIsAssigned$('servedCity.City.name')",
                Result = { list[3].Identifier }
            };
        }

        static TestParam Test_RefCount1()
        {
            var listPro = new FeatureListProvider();

            var ahList = new List<AirportHeliport>();
            for (var i = 0; i < 10; i++)
                ahList.Add(CreateAirportHeliport(i));

            var rwyList = new List<Runway>();
            for (var i = 0; i < 10; i++)
            {
                var rwy = CreateRunway(i);
                rwyList.Add(rwy);
            }

            rwyList[1].AssociatedAirportHeliport = new FeatureRef(Guid.NewGuid());
            rwyList[3].AssociatedAirportHeliport = new FeatureRef(ahList[6].Identifier);

            ahList.Add(ahList[6]);

            listPro.InsertRange(ahList);
            listPro.InsertRange(rwyList);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.Runway,
                Command = "$AimIsAssigned$('Designator') AND NOT $AimRefCount$('AssociatedAirportHeliport', :v0, 1)",
                CommandValues = new object[] { FeatureType.AirportHeliport },
                Result = { rwyList[1].Identifier }
            };
        }

        static TestParam Test_RefCount2()
        {
            var listPro = new FeatureListProvider();

            var oaList = new List<ObstacleArea>();
            for (var i = 0; i < 10; i++)
                oaList.Add(CreateObstacleArea(i));

            var ahList = new List<AirportHeliport>();
            for (var i = 0; i < 10; i++)
                ahList.Add(CreateAirportHeliport(i));

            oaList[1].Reference = new ObstacleAreaOrigin { OwnerAirport = new FeatureRef(ahList[0].Identifier) };
            oaList[2].Reference = new ObstacleAreaOrigin { OwnerAirport = new FeatureRef(ahList[1].Identifier) };

            listPro.InsertRange(oaList);
            listPro.InsertRange(ahList);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.ObstacleArea,
                Command = 
                    "$AimIsAssigned$('reference.ownerAirport') AND " +
                    "$AimRefCount$('reference.ownerAirport', :v0, 1)",
                CommandValues = new object[] { FeatureType.AirportHeliport },
                Result = { oaList[1].Identifier, oaList[2].Identifier }
            };
        }

        static TestParam Test_AimEqualTo()
        {
            var listPro = new FeatureListProvider();

            var ahList = new List<AirportHeliport>();
            for (var i = 0; i < 10; i++)
            {
                var ah = CreateAirportHeliport(i);
                ahList.Add(ah);
            }

            ahList[2].FieldElevation.Uom = UomDistanceVertical.SM;
            ahList[4].FieldElevation.Uom = UomDistanceVertical.FL;

            listPro.InsertRange(ahList);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.AirportHeliport,
                Command = "$AimEqualTo$('fieldElevation.uom', :v0)",
                CommandValues = new object[] { new object[] { "FL", "SM" } },
                Result = { ahList[2].Identifier, ahList[4].Identifier }
            };
        }

        static TestParam Test_AimEqualToProp()
        {
            var listPro = new FeatureListProvider();

            var ahList = new List<AirportHeliport>();
            for (var i = 0; i < 10; i++)
                ahList.Add(CreateAirportHeliport(i));

            var tah = ahList[0];
            tah.FieldElevationAccuracy = new ValDistanceVertical(10.1, UomDistanceVertical.M);
            tah.ARP = new ElevatedPoint { GeoidUndulation = new ValDistanceSigned(1.1, UomDistance.M) };

            tah = ahList[4];
            tah.FieldElevationAccuracy = new ValDistanceVertical(10.2, UomDistanceVertical.FT);
            tah.ARP = new ElevatedPoint { GeoidUndulation = new ValDistanceSigned(1.2, UomDistance.M) };

            listPro.InsertRange(ahList);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.AirportHeliport,
                Command =
                    "$AimEqualToProp$('fieldElevationAccuracy.uom', 'ARP.ElevatedPoint.geoidUndulation.uom') AND " +
                    "$AimIsAssigned$('fieldElevationAccuracy') AND " +
                    "$AimIsAssigned$('ARP.ElevatedPoint.geoidUndulation')",
                Result = { ahList[0].Identifier }
            };
        }

        static TestParam Test_AimHigherLowerEqual()
        {
            var listPro = new FeatureListProvider();

            var ahList = new List<AirportHeliport>();
            for (var i = 0; i < 10; i++)
                ahList.Add(CreateAirportHeliport(i));

            var tah = ahList[0];
            tah.FieldElevation = new ValDistanceVertical(30.1, UomDistanceVertical.M);

            tah = ahList[4];
            tah.FieldElevation = new ValDistanceVertical(40.1, UomDistanceVertical.M);

            listPro.InsertRange(ahList);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.AirportHeliport,
                Command =
                    "$AimIsAssigned$('fieldElevation') AND " +
                    "$AimHigherLowerEqual$('fieldElevation.value', '>', :v0)  ",
                CommandValues = new object[] { new object[] { 31 } },
                Result = { ahList[4].Identifier }
            };
        }

        static TestParam Test_1A8548()
        {
            var listPro = new FeatureListProvider();

            var vsList = new List<VerticalStructure>();
            vsList.Add(CreateVerticalStructure(0));
            vsList.Add(CreateVerticalStructure(1));
            vsList.Add(CreateVerticalStructure(2));

            var oaList = new List<ObstacleArea>();

            var oa = BRuleCheckerTesterV1.CreateObstacleArea(0);
            oa.Obstacle.Add(new Aran.Aim.Objects.FeatureRefObject(vsList[0].Identifier));
            oa.Obstacle.Add(new Aran.Aim.Objects.FeatureRefObject(vsList[1].Identifier));
            oaList.Add(oa);

            oa = BRuleCheckerTesterV1.CreateObstacleArea(1);
            oa.Obstacle.Add(new Aran.Aim.Objects.FeatureRefObject(vsList[2].Identifier));
            oa.Obstacle.Add(new Aran.Aim.Objects.FeatureRefObject(Guid.NewGuid()));
            oaList.Add(oa);

            oa = BRuleCheckerTesterV1.CreateObstacleArea(2);
            oaList.Add(oa);

            listPro.InsertRange(oaList);
            listPro.InsertRange(vsList);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.ObstacleArea,
                Command = 
                    "$AimIsAssigned$('obstacle') AND " +
                    "NOT ($AimRefCount$('obstacle', :v0, 1))",
                CommandValues = new object[] { FeatureType.VerticalStructure },
                Result = { oaList[1].Identifier }
            };
        }

        static TestParam Test_26D18()
        {
            var listPro = new FeatureListProvider();

            var list = new List<Airspace>();
            for(var i = 0; i < 3; i++)
                list.Add(CreateAirspace(i));

            list[0].GeometryComponent.Add(new AirspaceGeometryComponent { Operation = CodeAirspaceAggregation.BASE });
            list[0].GeometryComponent.Add(new AirspaceGeometryComponent { Operation = CodeAirspaceAggregation.BASE });

            list[1].GeometryComponent.Add(new AirspaceGeometryComponent { Operation = CodeAirspaceAggregation.BASE });


            listPro.InsertRange(list);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.Airspace,
                Command = "($AimPropCount$('geometryComponent', null, null) > 1) AND " +
                    "NOT ($AimPropCount$('geometryComponent.AirspaceGeometryComponent.operation', :v0, :v1) = 1) ",
                CommandValues = new object[] {
                    PropConditionType.Equal,
                    new object[] { "BASE" } },
                Result = { list[0].Identifier }
            };
        }

        static TestParam Test_19F870()
        {
            var listPro = new FeatureListProvider();

            var list = new List<Airspace>();
            for (var i = 0; i < 3; i++)
                list.Add(CreateAirspace(i));

            list[0].GeometryComponent.Add(new AirspaceGeometryComponent { Operation = CodeAirspaceAggregation.BASE });
            list[0].GeometryComponent.Add(new AirspaceGeometryComponent { Operation = CodeAirspaceAggregation.BASE });

            list[1].GeometryComponent.Add(new AirspaceGeometryComponent { OperationSequence = 1 });
            list[1].GeometryComponent.Add(new AirspaceGeometryComponent { OperationSequence = 2 });

            listPro.InsertRange(list);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.Airspace,
                Command =
                    "$AimPropCount$('geometryComponent', null, null) > 1 AND " +
                    "NOT ($AimIsAssigned$('geometryComponent.AirspaceGeometryComponent.operation')) ",
                Result = { list[1].Identifier }
            };
        }

        static TestParam Test_19F488()
        {
            var listPro = new FeatureListProvider();

            var list = new List<Airspace>();
            for (var i = 0; i < 3; i++)
                list.Add(CreateAirspace(i));

            list[0].GeometryComponent.Add(new AirspaceGeometryComponent { Operation = CodeAirspaceAggregation.BASE });
            list[0].GeometryComponent.Add(new AirspaceGeometryComponent { Operation = CodeAirspaceAggregation.BASE });

            list[1].GeometryComponent.Add(new AirspaceGeometryComponent { OperationSequence = 1 });
            list[1].GeometryComponent.Add(new AirspaceGeometryComponent { OperationSequence = 2 });

            listPro.InsertRange(list);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.Airspace,
                Command =
                    "$AimPropCount$('geometryComponent', null, null) > 1 AND " +
                    "NOT ($AimIsAssigned$('geometryComponent.AirspaceGeometryComponent.operationSequence')) ",
                Result = { list[0].Identifier }
            };
        }

        static TestParam Test_249F1()
        {
            var listPro = new FeatureListProvider();
            var list = new List<StandardLevelTable>();

            list.Add(new StandardLevelTable { Identifier = Guid.NewGuid() });
            list.Add(new StandardLevelTable { Identifier = Guid.NewGuid() });

            listPro.InsertRange(list);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.StandardLevelTable,
                Command = "",
                Result = { list[0].Identifier, list[1].Identifier }
            };
        }
        
        static TestParam Test_249F2()
        {
            var listPro = new FeatureListProvider();
            var list = new List<StandardLevelColumn>();

            list.Add(new StandardLevelColumn { Identifier = Guid.NewGuid() });
            list.Add(new StandardLevelColumn { Identifier = Guid.NewGuid() });

            listPro.InsertRange(list);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.StandardLevelColumn,
                Command = "",
                Result = { list[0].Identifier, list[1].Identifier }
            };
        }

        static TestParam Test_D4A72()
        {
            var listPro = new FeatureListProvider();
            var list = new List<RunwayDirection>();

            for (var i = 0; i < 10; i++)
            {
                var rd = BRuleCheckerTesterV1.CreateRunwayDirection(i);
                rd.ElevationTDZ = new ValDistanceVertical(10, UomDistanceVertical.M);
                rd.ElevationTDZAccuracy = new ValDistance(1, UomDistance.M);
                list.Add(rd);
            }

            list[2].ElevationTDZAccuracy.Uom = UomDistance.FT;

            listPro.InsertRange(list);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.RunwayDirection,
                Command =
                    "$AimIsAssigned$('elevationTDZ') AND " +
                    "$AimIsAssigned$('elevationTDZAccuracy') AND " +
                    "NOT ($AimEqualToProp$('elevationTDZ.uom', 'elevationTDZAccuracy.uom'))",
                Result = { list[2].Identifier }
            };
        }

        static TestParam Test_12B510()
        {
            var listPro = new FeatureListProvider();
            var list = new List<Airspace>();

            for (var i = 0; i < 10; i++)
            {
                var item = BRuleCheckerTesterV1.CreateAirspace(i);
                item.GeometryComponent.Add(new AirspaceGeometryComponent
                {
                    TheAirspaceVolume = new AirspaceVolume
                    {
                        HorizontalProjection = new Surface { },
                        Centreline = new Curve { }
                    }
                });
                list.Add(item);
            }

            list[2].GeometryComponent[0].TheAirspaceVolume.HorizontalProjection = null;
            list[2].GeometryComponent[0].TheAirspaceVolume.Centreline = null;

            listPro.InsertRange(list);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.Airspace,
                Command =
                    "NOT ($AimIsAssigned$('geometryComponent.AirspaceGeometryComponent.TheAirspaceVolume.AirspaceVolume.contributorAirspace')) AND " +
                    "NOT ($AimIsAssigned$('geometryComponent.AirspaceGeometryComponent.TheAirspaceVolume.AirspaceVolume.horizontalProjection')) AND " +
                    "NOT ($AimIsAssigned$('geometryComponent.AirspaceGeometryComponent.TheAirspaceVolume.AirspaceVolume.centreline')) ",
                Result = { list[2].Identifier }
            };
        }

        static TestParam Test_135151()
        {
            var listPro = new FeatureListProvider();
            var list = new List<OrganisationAuthority>();

            for (var i = 0; i < 10; i++)
            {
                var item = BRuleCheckerTesterV1.CreateOrganisationAuthority(i);
                item.Type = CodeOrganisation.ATS;
                list.Add(item);
            }

            list[1].Type = CodeOrganisation.STATE_GROUP;
            list[3].Type = CodeOrganisation.STATE;
            list[5].Type = CodeOrganisation.INTL_ORG;

            listPro.InsertRange(list);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.OrganisationAuthority,
                Command = "$AimEqualTo$('type', :v0)",
                CommandValues = new object[] { new object[] { "STATE_GROUP", "STATE", "INTL_ORG" } },
                Result = { list[1].Identifier, list[3].Identifier, list[5].Identifier }
            };
        }

        static TestParam Test_1A33EF()
        {
            var listPro = new FeatureListProvider();
            var list = new List<RouteSegment>();

            for (var i = 0; i < 10; i++)
            {
                var item = new RouteSegment
                {
                    Identifier = Guid.NewGuid(),
                    RouteFormed = new FeatureRef(Guid.NewGuid())
                };
                list.Add(item);
            }

            list[1].RouteFormed = null;
            list[3].RouteFormed = null;
            list[5].RouteFormed = null;

            listPro.InsertRange(list);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.RouteSegment,
                Command = "NOT $AimIsAssigned$('routeFormed')",
                Result = { list[1].Identifier, list[3].Identifier, list[5].Identifier }
            };
        }

        static TestParam Test_1A851E()
        {
            var listPro = new FeatureListProvider();

            var navList = new List<Navaid>();
            for (var i = 0; i < 10; i++)
                navList.Add(new Navaid { Identifier = Guid.NewGuid() });


            var list = new List<AngleIndication>();

            for (var i = 0; i < 10; i++)
            {
                var item = new AngleIndication
                {
                    Identifier = Guid.NewGuid(),
                    PointChoice = new SignificantPoint
                    {
                        NavaidSystem = new FeatureRef(navList[i].Identifier)
                    }
                };
                list.Add(item);
            }

            list[1].PointChoice.NavaidSystem.Identifier = Guid.NewGuid();
            list[3].PointChoice.NavaidSystem.Identifier = Guid.NewGuid();
            list[5].PointChoice = null;
            list[7].PointChoice.FixDesignatedPoint = new FeatureRef(Guid.NewGuid());

            listPro.InsertRange(list);
            listPro.InsertRange(navList);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.AngleIndication,
                Command =
                    "$AimIsAssigned$('pointChoice.SignificantPoint.navaidSystem') AND " +
                    "NOT $AimRefCount$('pointChoice.SignificantPoint.navaidSystem', :v0, 1)",
                CommandValues = new object[] { FeatureType.Navaid },
                Result = { list[1].Identifier, list[3].Identifier }
            };
        }

        static TestParam Test_1A8530()
        {
            var listPro = new FeatureListProvider();

            var navList = new List<Navaid>();
            for (var i = 0; i < 10; i++)
                navList.Add(new Navaid { Identifier = Guid.NewGuid() });


            var list = new List<DistanceIndication>();

            for (var i = 0; i < 10; i++)
            {
                var item = new DistanceIndication
                {
                    Identifier = Guid.NewGuid(),
                    PointChoice = new SignificantPoint
                    {
                        NavaidSystem = new FeatureRef(navList[i].Identifier)
                    }
                };
                list.Add(item);
            }

            list[1].PointChoice.NavaidSystem.Identifier = Guid.NewGuid();
            list[3].PointChoice.NavaidSystem.Identifier = Guid.NewGuid();
            list[5].PointChoice = null;

            listPro.InsertRange(list);
            listPro.InsertRange(navList);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.DistanceIndication,
                Command =
                    "$AimIsAssigned$('pointChoice.SignificantPoint.navaidSystem') AND " +
                    "NOT $AimRefCount$('pointChoice.SignificantPoint.navaidSystem', :v0, 1)",
                CommandValues = new object[] { FeatureType.Navaid },
                Result = { list[1].Identifier, list[3].Identifier }
            };
        }

        static TestParam Test_1B2152()
        {
            var listPro = new FeatureListProvider();

            var dme = new DME { Identifier = Guid.NewGuid() };
            listPro.Insert(dme);

            var vor = new VOR { Identifier = Guid.NewGuid() };
            listPro.Insert(vor);

            var nav = new Navaid
            {
                Identifier = Guid.NewGuid(),
                Type = CodeNavaidService.DME,
                NavaidEquipment =
                {
                    new NavaidComponent
                    {
                        TheNavaidEquipment = new AbstractNavaidEquipmentRef
                        {
                            Type = NavaidEquipmentType.DME,
                            Identifier = dme.Identifier
                        }
                    }
                }
            };
            listPro.Insert(nav);

            nav = nav.Clone() as Navaid;
            nav.Identifier = Guid.NewGuid();
            nav.Type = CodeNavaidService.VOR;
            nav.NavaidEquipment[0].TheNavaidEquipment = new AbstractNavaidEquipmentRef { Type = NavaidEquipmentType.VOR, Identifier = vor.Identifier };
            listPro.Insert(nav);

            nav = nav.Clone() as Navaid;
            nav.Identifier = Guid.NewGuid();
            nav.Type = CodeNavaidService.VOR;
            nav.NavaidEquipment[0].TheNavaidEquipment = new AbstractNavaidEquipmentRef { Type = NavaidEquipmentType.DME, Identifier = dme.Identifier };
            listPro.Insert(nav);

            return new TestParam
            {
                FeatProvider = listPro,
                FeatureType = FeatureType.Navaid,
                Command =
                    "$AimEqualTo$('type', :v0) AND " +
                    "$AimRefCount$('navaidEquipment.NavaidComponent.theNavaidEquipment', :v1, 1)",
                CommandValues = new object[] { new object[] { "VOR" }, FeatureType.DME },
                Result = { nav.Identifier }
            };
        }



        public static AirportHeliport CreateAirportHeliport(int n)
        {
            var ah = new AirportHeliport();
            ah.Identifier = Guid.NewGuid();
            ah.Type = CodeAirportHeliport.AH;
            ah.Designator = "AH Designator - " + n;
            ah.Name = "AH Name - " + n;
            ah.FieldElevation = new ValDistanceVertical(10.1 + n, UomDistanceVertical.M);
            return ah;
        }

        public static Runway CreateRunway(int n)
        {
            var rwy = new Runway();
            rwy.Identifier = Guid.NewGuid();
            rwy.Type = CodeRunway.RWY;
            rwy.WidthStrip = new ValDistance(20.2 + n, UomDistance.MI);
            rwy.Designator = "RWY-Dsg-" + n;
            return rwy;
        }

        public static ObstacleArea CreateObstacleArea(int n)
        {
            var oa = new ObstacleArea();
            oa.Identifier = Guid.NewGuid();
            if (!Enum.TryParse<CodeObstacleArea>("AREA" + n, out CodeObstacleArea type))
                type = CodeObstacleArea.MANAGED;
            oa.Type = type;
            return oa;
        }

        public static VerticalStructure CreateVerticalStructure(int n)
        {
            var vs = new VerticalStructure();
            vs.Identifier = Guid.NewGuid();
            vs.Name = "VS - " + n;
            vs.Type = (n > (int)CodeVerticalStructure.WINDMILL_FARMS ? CodeVerticalStructure.AG_EQUIP : (CodeVerticalStructure)n);
            return vs;
        }

        public static Airspace CreateAirspace(int n)
        {
            var airsp = new Airspace();
            airsp.Identifier = Guid.NewGuid();
            airsp.Designator = "Airspace-" + n;
            
            return airsp;
        }

        public static RunwayDirection CreateRunwayDirection(int n)
        {
            var rd = new RunwayDirection();
            rd.Identifier = Guid.NewGuid();
            rd.Designator = "Runway Direction - " + n;
            return rd;
        }

        public static OrganisationAuthority CreateOrganisationAuthority(int n)
        {
            var item = new OrganisationAuthority();
            item.Identifier = Guid.NewGuid();
            item.Designator = "OrganisationAuthority - " + n;
            return item;
        }
    }

    public class TestParam
    {
        public TestParam()
        {
            Result = new List<Guid>();
        }

        public IFeatureProvider FeatProvider { get; set; }
        public FeatureType FeatureType { get; set; }
        public string Command { get; set; }
        public object[] CommandValues { get; set; }
        public List<Guid> Result { get; private set; }
    }
}
