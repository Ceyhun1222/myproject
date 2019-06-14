using Aran.Aim;
using Aran.Aim.AixmMessage;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AranGeo = Aran.Geometries;

namespace BRuleCheckerTester
{
    class AimPropertyGetterTester : ITester
    {
        public void Test()
        {
            Func<Tuple<AimObject, object[]>>[] funcs =
            {
                GetAirportHeliport,
                GetFinalLeg,
                GetObstacleArea
            };

            var strList = new StringBuilder();

            foreach (var getFunc in funcs)
            {
                var tuple = getFunc();
                var cInfo = AimMetadata.GetClassInfoByIndex(tuple.Item1);

                var propName = Check(tuple.Item1, tuple.Item2);

                if (!string.IsNullOrEmpty(propName))
                    strList.AppendLine($"\t{cInfo.AixmName}.{propName}");
            }

            if (strList.Length > 0)
            {
                Console.WriteLine("Failed:");
                Console.WriteLine(strList.ToString());
            }
        }

        private string Check(AimObject aimObj, object[] param)
        {
            for (var i = 0; i < param.Length - 1; i += 2)
            {
                var propPath = param[i].ToString();
                var propVal = param[i + 1] as IEnumerable<object>;
                if (propVal == null)
                    propVal = new object[] { param[i + 1] };

                var vals = Aran.Aim.BusinessRules.AimPropertyGetter.GetPropValue(aimObj, propPath);
                if (!vals.SequenceEqual(propVal as IEnumerable<object>, new CheckerObjectComparer()))
                    return propPath;
            }

            return string.Empty;
        }

        private Tuple<AimObject, object[]> GetAirportHeliport()
        {
            var orgIdentifier = Guid.NewGuid();

            return new Tuple<AimObject, object[]>(

                new AirportHeliport
                {
                    Name = "AH - 1",

                    FieldElevation = new ValDistanceVertical
                    {
                        Value = 15.1,
                        Uom = UomDistanceVertical.M
                    },

                    FieldElevationAccuracy = new ValDistanceVertical
                    {
                        Value = 0.1,
                        Uom = UomDistanceVertical.SM
                    },

                    Type = CodeAirportHeliport.AH,

                    CertifiedICAO = true,

                    MagneticVariation = 20,

                    ServedCity =
                    {
                        new City { Name = "City - 1" },
                        new City { Name = "City - 2" },
                    },

                    ResponsibleOrganisation = new AirportHeliportResponsibilityOrganisation
                    {
                        Role = CodeAuthorityRole.OWN,
                        TheOrganisationAuthority = new FeatureRef(orgIdentifier)
                    },

                    ARP = new ElevatedPoint
                    {
                        Elevation = new ValDistanceVertical { Value = 17.2, Uom = UomDistanceVertical.M },
                        GeoidUndulation = new ValDistanceSigned { Value = 22.2, Uom = UomDistance.M }
                    }
                },
                
                new object[]
                {
                    "Name", "AH - 1",
                    "fieldElevation.uom", "M",
                    "fieldElevation.Value", 15.1,
                    "certifiedICAO", true,
                    "fieldElevationAccuracy.uom", "sm",
                    "type", "ah",
                    "magneticVariation", 20,
                    "ARP.ElevatedPoint.Elevation.Value", 17.2,
                    "servedCity.City.name", new object[] { "City - 1", "City - 2" }
                });
        }

        private Tuple<AimObject, object[]> GetFinalLeg()
        {
            var fixDesignatedPointIdentifier = Guid.NewGuid();

            var leg = new FinalLeg
            {
                LegPath = CodeTrajectory.PT,
                FinalPathAlignmentPoint = new SignificantPoint
                {
                    //FixDesignatedPoint = new FeatureRef(fixDesignatedPointIdentifier)
                    Position = new AixmPoint
                    {
                        HorizontalAccuracy = new ValDistance(1.2, UomDistance.MI)
                    }
                }
            };

            leg.FinalPathAlignmentPoint.Position.Geo.SetCoords(10.1, 20.2);

            return new Tuple<AimObject, object[]>(leg,
                new object[]
                {
                    "legPath", "PT",
                    //"finalPathAlignmentPoint.SignificantPoint.fixDesignatedPoint", fixDesignatedPointIdentifier
                    "finalPathAlignmentPoint.SignificantPoint.position.Point.horizontalAccuracy.uom", "mi"
                });
        }

        private Tuple<AimObject, object[]> GetObstacleArea()
        {
            var vsIden1 = Guid.NewGuid();
            var vsIden2 = Guid.NewGuid();

            return new Tuple<AimObject, object[]>(

                new ObstacleArea
                {
                    Obstacle =
                    {
                        new FeatureRefObject(vsIden1),
                        new FeatureRefObject(vsIden2)
                    }
                },

                new object[]
                {
                    "obstacle", new object[] { vsIden1, vsIden2 }
                });
        }

    }

    internal class CheckerObjectComparer : IEqualityComparer<object>
    {
        public new bool Equals(object x, object y)
        {
            switch (Type.GetTypeCode(x.GetType()))
            {
                case TypeCode.String:
                    return string.Equals(x as string, y as string, StringComparison.InvariantCultureIgnoreCase);
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                    if (x == null || y == null)
                        return false;
                    return Convert.ToDouble(x) == Convert.ToDouble(y);
                default:
                    return object.Equals(x, y);
            }
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}
