using Aran.Aim;
using Aran.Aim.BusinessRules;
using SbvrParser.TesterConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BRuleCheckerTester
{
    class BRuleSqlConverterTester : ITester
    {
        private string testPrefix = "Test_";

        public void Test()
        {
            var methods = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
            var methodResultType = typeof(BRule);

            var failedMethodList = new List<string>();

            foreach (var method in methods)
            {
                if (method.Name.StartsWith(testPrefix) && method.ReturnType == methodResultType)
                {
                    var testCmdInfoList = new List<CommandInfo>();
                    var rule = method.Invoke(null, new object[] { testCmdInfoList }) as BRule;

                    var resultCmdInfoList = BRuleSqlConverter.ToSqlCommand(rule);

                    foreach (var item in testCmdInfoList)
                    {
                        if (!item.Command.StartsWith("SELECT", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var s = "SELECT identifier_p1, identifier_p2 FROM features WHERE feat_type = :featType";
                            if (item.Command.Length > 0)
                                s += " AND (" + item.Command + ")";
                            item.Command = s;
                        }
                    }

                    var isOk = CmdInfoListComparer.Equals(testCmdInfoList, resultCmdInfoList);

                    if (!isOk)
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

        private void TestCmdInfoEquality()
        {
            var aList = new List<CommandInfo>();
            aList.Add(
                new CommandInfo
                {
                    FeatureType = FeatureType.AirportHeliport,
                    Command = "abc",
                    CommandValues = { 1, 3, FeatureType.AirportHeliport, new object[] { "a", "b" } }
                });

            var bList = new List<CommandInfo>();
            bList.Add(
                new CommandInfo
                {
                    FeatureType = FeatureType.AirportHeliport,
                    Command = "abc",
                    CommandValues = { 1, 1583, 3, new object[] { "b", "a" } }
                });

            
            Console.WriteLine("b: " + CmdInfoListComparer.Equals(aList, bList));
        }

        static BRule Test_1A8548(List<CommandInfo> ciList)
        {
            ciList.Add(new CommandInfo
            {
                FeatureType = FeatureType.ObstacleArea,
                Command =
                    "($AimIsAssigned$('obstacle')) AND " +
                    "NOT ($AimRefCount$('obstacle', :v0, 1))",
                CommandValues = { FeatureType.VerticalStructure }
            });

            return RulesTester.Get_1A8548();
        }

        static BRule Test_1028BA(List<CommandInfo> ciList)
        {
            ciList.Add(new CommandInfo
            {
                FeatureType = FeatureType.AirTrafficControlService,
                Command =
                    "($AimIsAssigned$('serviceProvider')) AND " +
                    "NOT ($AimRefCount$('serviceProvider', :v0, 1))",
                CommandValues = { FeatureType.Unit }
            });

            return RulesTester.Get_1028BA();
        }

        static BRule Test_1028DB(List<CommandInfo> ciList)
        {
            ciList.Add(new CommandInfo
            {
                FeatureType = FeatureType.MissedApproachLeg,
                Command =
                    "($AimIsAssigned$('holding.HoldingUse.theHoldingPattern')) AND " +
                    "NOT ($AimRefCount$('holding.HoldingUse.theHoldingPattern', :v0, 1))",
                CommandValues = { FeatureType.HoldingPattern }
            });

            return RulesTester.Get_1028DB();
        }

        static BRule Test_F07A9(List<CommandInfo> ciList)
        {
            ciList.Add(new CommandInfo
            {
                FeatureType = FeatureType.RouteSegment,
                Command =
                    "($AimEqualTo$('minimumObstacleClearanceAltitude', :v0))",
                CommandValues = { new object[] { "FLOOR", "CEILING" } }
            });

            return RulesTester.Get_F07A9();
        }

        static BRule Test_135151(List<CommandInfo> ciList)
        {
            ciList.Add(new CommandInfo
            {
                FeatureType = FeatureType.OrganisationAuthority,
                Command =
                    "($AimEqualTo$('type', :v0))",
                CommandValues = { new object[] { "STATE", "STATE_GROUP", "INTL_ORG" } }
            });

            return RulesTester.Get_135151();
        }

        static BRule __Test_33070(List<CommandInfo> ciList)
        {
            ciList.Add(new CommandInfo
            {
                FeatureType = FeatureType.Navaid,
                Command =
                    "($AimEqualTo$('type', :v0)) AND " +
                    "($AimIsAssigned$('servedAirport'))",
                CommandValues = { new object[] { "SDF" } }
            });

            return RulesTester.__Get_33070();
        }

        static BRule Test_D4A5E(List<CommandInfo> ciList)
        {
            ciList.Add(new CommandInfo
            {
                FeatureType = FeatureType.AirportHeliport,
                Command =
                    "($AimIsAssigned$('ARP.ElevatedPoint.verticalAccuracy')) AND " +
                    "($AimIsAssigned$('ARP.ElevatedPoint.geoidUndulation')) AND " +
                    "NOT ($AimEqualToProp$('ARP.ElevatedPoint.verticalAccuracy.uom', 'ARP.ElevatedPoint.geoidUndulation.uom'))"
            });

            return RulesTester.Get_D4A5E();
        }

        static BRule Test_26D18(List<CommandInfo> ciList)
        {
            ciList.Add(new CommandInfo
            {
                FeatureType = FeatureType.Airspace,
                Command = "($AimPropCount$('geometryComponent', null, null) > 1) AND " +
                    "NOT ($AimPropCount$('geometryComponent.AirspaceGeometryComponent.operation', :v0, :v1) = 1)",
                CommandValues = {
                    PropConditionType.Equal,
                    new object[] { "BASE" } }
            });

            return RulesTester.Get_26D18();
        }

        static BRule Test_19F870(List<CommandInfo> ciList)
        {
            ciList.Add(new CommandInfo
            {
                FeatureType = FeatureType.Airspace,
                Command = "($AimPropCount$('geometryComponent', null, null) > 1) AND " +
                    "NOT ($AimIsAssigned$('geometryComponent.AirspaceGeometryComponent.operation'))"
            });

            return RulesTester.Get_19F870();
        }

        static BRule Test_1A8530(List<CommandInfo> ciList)
        {
            ciList.Add(new CommandInfo
            {
                FeatureType = FeatureType.DistanceIndication,
                Command =
                    "($AimIsAssigned$('pointChoice.SignificantPoint.navaidSystem')) AND " +
                    "NOT ($AimRefCount$('pointChoice.SignificantPoint.navaidSystem', :v0, 1))",
                CommandValues = { FeatureType.Navaid }
            });

            return RulesTester.Get_1A8530();
        }

        static BRule Test_D4A72(List<CommandInfo> ciList)
        {
            ciList.Add(new CommandInfo
            {
                FeatureType = FeatureType.RunwayDirection,
                Command =
                    "($AimIsAssigned$('elevationTDZ')) AND " +
                    "($AimIsAssigned$('elevationTDZAccuracy')) AND " +
                    "NOT ($AimEqualToProp$('elevationTDZ.uom', 'elevationTDZAccuracy.uom'))"
            });

            return RulesTester.Get_D4A72();
        }

        static BRule Test_1A5285(List<CommandInfo> ciList)
        {
            ciList.Add(new CommandInfo
            {
                FeatureType = FeatureType.AirportHeliport,
                Command =
                    "($AimIsAssigned$('availability.specialDateAuthority')) AND " +
                    "NOT ($AimRefCount$('availability.specialDateAuthority', :v0, 1))",
                CommandValues = { FeatureType.OrganisationAuthority }
            });

            return RulesTester.Get_1A5285();
        }

    }

    static class CmdInfoListComparer
    {
        public static bool Equals(List<CommandInfo> aList, List<CommandInfo> bList)
        {
            var equality = new CmdInfoEquality();
            var l1 = aList.Except(bList, equality);
            var l2 = bList.Except(aList, equality);

            return !l1.Any() && !l2.Any();
        }

        private class CmdInfoEquality : IEqualityComparer<CommandInfo>
        {
            public bool Equals(CommandInfo x, CommandInfo y)
            {
                if (x.FeatureType != y.FeatureType)
                    return false;

                if (!x.Command.Equals(y.Command, StringComparison.InvariantCultureIgnoreCase))
                    return false;

                if (x.CommandValues.Count != y.CommandValues.Count)
                    return false;

                var equality = new CmdValEquality();
                var l1 = x.CommandValues.Except(y.CommandValues, equality);
                var l2 = y.CommandValues.Except(x.CommandValues, equality);

                return !l1.Any() && !l2.Any();
            }

            public int GetHashCode(CommandInfo obj)
            {
                var code = obj.FeatureType.GetHashCode(); 
                if (obj.Command != null)
                    code += obj.Command.GetHashCode();

                foreach (var item in obj.CommandValues)
                    code += CmdObjectHashCodeCalcer.Calc(item);

                return code;
            }
        }

        private class CmdValEquality : IEqualityComparer<object>
        {
            bool IEqualityComparer<object>.Equals(object x, object y)
            {
                try
                {
                    if (x == null || y == null)
                        return false;

                    var t1 = x.GetType();
                    var t2 = y.GetType();

                    if (t1.IsArray ^ t2.IsArray)
                        return false;

                    if (t1.IsArray)
                    {
                        if ((x as System.Collections.IList).Count != (y as System.Collections.IList).Count)
                            return false;

                        var list1 = new List<object>();
                        foreach (var item in (x as System.Collections.IList))
                            list1.Add(item);

                        var list2 = new List<object>();
                        foreach (var item in (y as System.Collections.IList))
                            list2.Add(item);

                        var equality = new CmdValEquality();
                        var l1 = list1.Except(list2, equality);
                        var l2 = list2.Except(list1, equality);

                        return !l1.Any() && !l2.Any();
                    }
                    else
                    {
                        if (t1.IsEnum || t2.IsEnum)
                            return (Convert.ToInt32(x) == Convert.ToInt32(y));
                        return x.Equals(y);
                    }
                }
                catch
                {
                    return false;
                }
            }

            int IEqualityComparer<object>.GetHashCode(object obj)
            {
                return CmdObjectHashCodeCalcer.Calc(obj);
            }
        }

        private class CmdObjectHashCodeCalcer
        {
            public static int Calc(object obj)
            {
                if (obj is object[] objArr)
                {
                    var hc = 0;
                    foreach (var item in objArr)
                        hc += item.GetHashCode();
                    return hc;
                }

                return obj.GetHashCode();
            }
        }
    }
}
