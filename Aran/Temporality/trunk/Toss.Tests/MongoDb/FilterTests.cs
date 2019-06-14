using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Data.Filters;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using ESRI.ArcGIS.esriSystem;
using Toss.Tests.Temporality;
using Xunit;

namespace Toss.Tests.MongoDb
{
    [Collection(nameof(Names.ServiceCollection))]
    [TestCaseOrderer("Toss.Tests.PriorityOrderer", "Toss.Tests")]
    public class FilterTests : TemporalityTests
    {
        public FilterTests(DataFixture noAixmFixture) : base(noAixmFixture)
        {
            var value = ConfigUtil.RepositoryType;

            if (value != RepositoryType.MongoRepository && value != RepositoryType.MongoWithBackupRepository)
            {
                throw new Exception("Need to select Mongo repository");
            }
        }

        static FilterTests()
        {
            LicenseInitializer.Instance.InitializeApplication(new[] { esriLicenseProductCode.esriLicenseProductCodeStandard }, new esriLicenseExtensionCode[] { });
        }

        #region Tools

        private ObstacleArea CommitObstacleArea(MultiPolygon multiPolygon)
        {
            ObstacleArea feat = (ObstacleArea)CreateFeature(FeatureType.ObstacleArea);

            feat.SurfaceExtent = new Surface();
            feat.SurfaceExtent.Geo.Add(multiPolygon);

            Commit(feat);

            return feat;
        }

        private HoldingPattern CommitHoldingPattern(double? inboundCourse = 1, double? outbandCourse = null,
            double lowerLimitVal = 10000, UomDistanceVertical lowerLimitUom = UomDistanceVertical.SM,
            string instruction = "Instruction", Guid navaidSystem = default(Guid), Point point = null,
            CodeCourse? codeCourse = CodeCourse.HDG)
        {
            HoldingPattern feat = (HoldingPattern)CreateFeature(FeatureType.HoldingPattern);

            feat.InboundCourse = inboundCourse;
            feat.Instruction = instruction;
            feat.LowerLimit = new ValDistanceVertical(lowerLimitVal, lowerLimitUom);
            feat.OutboundCourse = outbandCourse;
            feat.OutboundCourseType = codeCourse;

            feat.HoldingPoint = new AerialRefuellingPoint();
            feat.HoldingPoint.FacilityMakeup.Add(
                new PointReference
                {
                    PostFixTolerance = new ValDistanceSigned(1, UomDistance.CM)
                }
            );

            feat.OutboundLegSpan = new HoldingPatternLength();
            feat.OutboundLegSpan.EndPoint = new TerminalSegmentPoint();
            feat.OutboundLegSpan.EndPoint.PointChoice = new SignificantPoint();
            if (point == null)
            {
                feat.OutboundLegSpan.EndPoint.PointChoice.NavaidSystem = new FeatureRef
                {
                    FeatureType = FeatureType.Navaid,
                    Id = 1,
                    Identifier = navaidSystem
                };
            }
            else
            {
                feat.OutboundLegSpan.EndPoint.PointChoice.Position = new AixmPoint
                {
                    HorizontalAccuracy = new ValDistance(1, UomDistance.CM),
                    Id = 1
                };
                feat.OutboundLegSpan.EndPoint.PointChoice.Position.Geo.X = point.X;
                feat.OutboundLegSpan.EndPoint.PointChoice.Position.Geo.Y = point.Y;
                feat.OutboundLegSpan.EndPoint.PointChoice.Position.Geo.Z = point.Z;
            }

            Commit(feat);

            return feat;
        }

        private List<HoldingPattern> GetHoldingPatterns(Filter filter, bool notEmpty = true)
        {
            var res = _dataFixture.GetActualDataByDate(new FeatureId
            {
                FeatureTypeId = (int)FeatureType.HoldingPattern,
                WorkPackage = _dataFixture.LastSlotId
            }, true, DateTime.Now.AddYears(1), Interpretation.BaseLine, filter: filter);

            Assert.NotNull(res);
            if (notEmpty)
                Assert.NotEmpty(res);
            return res.Select(x => x.Data.Feature).Cast<HoldingPattern>().ToList();
        }

        private List<ObstacleArea> GetObstacleArea(Filter filter, bool notEmpty = true)
        {
            var res = _dataFixture.GetActualDataByDate(new FeatureId
            {
                FeatureTypeId = (int)FeatureType.ObstacleArea,
                WorkPackage = _dataFixture.LastSlotId
            }, true, DateTime.Now.AddYears(1), Interpretation.BaseLine, filter: filter);

            Assert.NotNull(res);
            if (notEmpty)
                Assert.NotEmpty(res);
            return res.Select(x => x.Data.Feature).Cast<ObstacleArea>().ToList();
        }

        #endregion

        #region Comparison Filters

        [Fact]
        public void ComparisonEqualTo()
        {
            CommitHoldingPattern(1);
            CommitHoldingPattern(2);

            var comparisonOps = new ComparisonOps(ComparisonOpType.EqualTo, "InboundCourse", 1);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.True(res.All(x => x.InboundCourse == 1));
        }

        [Fact]
        public void ComparisonEqualToDouble()
        {
            CommitHoldingPattern(1.00000000000001, 1);
            CommitHoldingPattern(0.99999999999999, 1);
            CommitHoldingPattern(2, 2);

            var comparisonOps = new ComparisonOps(ComparisonOpType.EqualTo, "InboundCourse", 1.0);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.Equal(2, res.Count);
            Assert.True(res.All(x => x.InboundCourse > 1 - 0.0000001 && x.InboundCourse < 1 + 0.0000001));
        }

        [Fact]
        public void ComparisonEqualToGuid()
        {
            var guid = Guid.NewGuid();
            var f1 = CommitHoldingPattern(navaidSystem: guid);
            var f2 = CommitHoldingPattern(navaidSystem: Guid.NewGuid());
            var f3 = CommitHoldingPattern(navaidSystem: guid);
            f3.OutboundLegSpan.EndPoint.PointChoice.NavaidSystem.Identifier = guid;

            var comparisonOps = new ComparisonOps(ComparisonOpType.EqualTo, "OutboundLegSpan.EndPoint.PointChoice.NavaidSystem", guid);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.Equal(2, res.Count);
            Assert.True(res.All(x => x.OutboundLegSpan.EndPoint.PointChoice.NavaidSystem.Identifier.Equals(guid)));
        }


        [Fact]
        public void ComparisonEqualToSi()
        {
            CommitHoldingPattern(lowerLimitVal: 1, lowerLimitUom: UomDistanceVertical.M);
            CommitHoldingPattern(lowerLimitVal: 0.1, lowerLimitUom: UomDistanceVertical.SM);
            CommitHoldingPattern(lowerLimitVal: 1, lowerLimitUom: UomDistanceVertical.FT);

            var comparisonOps = new ComparisonOps(ComparisonOpType.EqualTo, "LowerLimit", 1);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.Equal(2, res.Count);
            Assert.True(res.All(x => ConverterToSI.Convert(x.LowerLimit, 0) == 1));
        }

        [Fact]
        public void ComparisonIn()
        {
            CommitHoldingPattern(0);
            CommitHoldingPattern(1);
            CommitHoldingPattern(2);

            var list = new List<double?> { 1, 2 };

            var comparisonOps = new ComparisonOps(ComparisonOpType.In, "InboundCourse", list);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.True(res.All(x => list.Contains(x.InboundCourse)));
        }

        [Fact]
        public void ComparisonInEnum()
        {
            CommitHoldingPattern(codeCourse: CodeCourse.HDG);
            CommitHoldingPattern(codeCourse: CodeCourse.MAG_BRG);
            CommitHoldingPattern(codeCourse: CodeCourse.MAG_TRACK);

            var list = new List<CodeCourse?> {CodeCourse.HDG, CodeCourse.MAG_BRG };

            var comparisonOps = new ComparisonOps(ComparisonOpType.In, "OutboundCourseType", list);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.True(res.All(x => list.Contains(x.OutboundCourseType)));
        }

        [Fact]
        public void ComparisonLike()
        {
            CommitHoldingPattern(instruction: "123asd123");
            CommitHoldingPattern(instruction: "123test123");
            CommitHoldingPattern(instruction: "123qwe123");

            var comparisonOps = new ComparisonOps(ComparisonOpType.Like, "Instruction", "test");
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.True(res.All(x => x.Instruction.Contains("test")));
        }

        [Fact]
        public void ComparisonNotLike()
        {
            CommitHoldingPattern(instruction: "123asd123");
            CommitHoldingPattern(instruction: "123test123");
            CommitHoldingPattern(instruction: "123qwe123");

            var comparisonOps = new ComparisonOps(ComparisonOpType.NotLike, "Instruction", "test");
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.True(res.All(x => !x.Instruction.Contains("test")));
        }

        [Fact]
        public void ComparisonNotEqualTo()
        {
            CommitHoldingPattern(1);
            CommitHoldingPattern(2);

            var comparisonOps = new ComparisonOps(ComparisonOpType.NotEqualTo, "InboundCourse", 1);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.True(res.All(x => x.InboundCourse != 1));
        }

        [Fact]
        public void ComparisonGreaterThan()
        {
            CommitHoldingPattern(1);
            CommitHoldingPattern(2);

            var comparisonOps = new ComparisonOps(ComparisonOpType.GreaterThan, "InboundCourse", 1);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.True(res.All(x => x.InboundCourse > 1));
        }

        [Fact]
        public void ComparisonGreaterThanOrEqualTo()
        {
            CommitHoldingPattern(0);
            CommitHoldingPattern(1);
            CommitHoldingPattern(2);

            var comparisonOps = new ComparisonOps(ComparisonOpType.GreaterThanOrEqualTo, "InboundCourse", 1);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.True(res.All(x => x.InboundCourse >= 1));
        }

        [Fact]
        public void ComparisonLessThan()
        {
            CommitHoldingPattern(1);
            CommitHoldingPattern(2);
            CommitHoldingPattern(0);

            var comparisonOps = new ComparisonOps(ComparisonOpType.LessThan, "InboundCourse", 1);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.True(res.All(x => x.InboundCourse < 1));
        }

        [Fact]
        public void ComparisonLessThanOrEqualTo()
        {
            CommitHoldingPattern(1);
            CommitHoldingPattern(2);
            CommitHoldingPattern(0);

            var comparisonOps = new ComparisonOps(ComparisonOpType.LessThanOrEqualTo, "InboundCourse", 1);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.True(res.All(x => x.InboundCourse <= 1));
        }

        [Fact]
        public void ComparisonNotNull()
        {
            CommitHoldingPattern(null);
            CommitHoldingPattern(2);
            CommitHoldingPattern(0);

            var comparisonOps = new ComparisonOps(ComparisonOpType.NotNull, "InboundCourse", 1);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.True(res.All(x => x.InboundCourse != null));
        }

        [Fact]
        public void ComparisonNull()
        {
            CommitHoldingPattern(null);
            CommitHoldingPattern(2);
            CommitHoldingPattern(0);

            var comparisonOps = new ComparisonOps(ComparisonOpType.Null, "InboundCourse", 1);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter);

            Assert.True(res.All(x => x.InboundCourse == null));
        }

        [Fact]
        public void ComparisonEmptyResult()
        {
            CommitHoldingPattern(1);
            CommitHoldingPattern(2);

            var comparisonOps = new ComparisonOps(ComparisonOpType.EqualTo, "InboundCourse", 3);
            var filter = new Filter(new OperationChoice(comparisonOps));

            var res = GetHoldingPatterns(filter, false);

            Assert.Empty(res);
        }

        #endregion

        #region Logic Filters

        [Fact]
        public void LogicAnd()
        {
            CommitHoldingPattern(1, 1);
            CommitHoldingPattern(1, 2);
            CommitHoldingPattern(2, 1);
            CommitHoldingPattern(2, 2);

            var logicOps = new BinaryLogicOp { Type = BinaryLogicOpType.And };

            logicOps.OperationList.Add(new OperationChoice(new ComparisonOps(ComparisonOpType.GreaterThan, "InboundCourse", 1)));
            logicOps.OperationList.Add(new OperationChoice(new ComparisonOps(ComparisonOpType.GreaterThanOrEqualTo, "OutboundCourse", 1)));
            var filter = new Filter(new OperationChoice(logicOps));

            var res = GetHoldingPatterns(filter);

            Assert.Equal(2, res.Count);
            Assert.True(res.All(x => x.InboundCourse > 1 && x.OutboundCourse >= 1));
        }

        [Fact]
        public void LogicOr()
        {
            CommitHoldingPattern(1, 1);
            CommitHoldingPattern(1, 2);
            CommitHoldingPattern(2, 1);
            CommitHoldingPattern(2, 2);

            var logicOps = new BinaryLogicOp { Type = BinaryLogicOpType.Or };
            logicOps.OperationList.Add(new OperationChoice(new ComparisonOps(ComparisonOpType.EqualTo, "InboundCourse", 1)));
            logicOps.OperationList.Add(new OperationChoice(new ComparisonOps(ComparisonOpType.EqualTo, "OutboundCourse", 1)));
            var filter = new Filter(new OperationChoice(logicOps));

            var res = GetHoldingPatterns(filter);

            Assert.Equal(3, res.Count);
            Assert.True(res.All(x => x.InboundCourse == 1 || x.OutboundCourse == 1));
        }

        [Fact]
        public void LogicEmptyResult()
        {
            CommitHoldingPattern(1, 1);
            CommitHoldingPattern(1, 2);
            CommitHoldingPattern(2, 1);
            CommitHoldingPattern(2, 2);

            var logicOps = new BinaryLogicOp { Type = BinaryLogicOpType.And };
            logicOps.OperationList.Add(new OperationChoice(new ComparisonOps(ComparisonOpType.EqualTo, "InboundCourse", 0)));
            logicOps.OperationList.Add(new OperationChoice(new ComparisonOps(ComparisonOpType.EqualTo, "OutboundCourse", 1)));
            var filter = new Filter(new OperationChoice(logicOps));

            var res = GetHoldingPatterns(filter, false);

            Assert.Empty(res);
        }

        #endregion

        #region Spatial filters

        #region For Point

        [Fact]
        public void SpatialPointInPolygon()
        {
            CommitHoldingPattern(point: new Point(1, 1));
            CommitHoldingPattern(point: new Point(1, 0));
            CommitHoldingPattern(point: new Point(10, 10));

            var polygon = new Polygon
            {
                ExteriorRing = new Ring
                {
                    new Point(0, 0), new Point(5, 0) , new Point(5, 5) , new Point(0, 5), new Point(0, 0)
                }
            };

            var spatialOps = new Within
            {
                PropertyName = "OutboundLegSpan.EndPoint.PointChoice.Position.Geo",
                Geometry = polygon
            };

            var filter = new Filter(new OperationChoice(spatialOps));

            var res = GetHoldingPatterns(filter);

            Assert.Equal(2, res.Count);
            Assert.True(res.All(x => x.OutboundLegSpan.EndPoint.PointChoice.Position.Geo.X == 1));
        }

        [Fact]
        public void SpatialPointInPolygonWithInteriorRing()
        {
            CommitHoldingPattern(point: new Point(1, 1));
            CommitHoldingPattern(point: new Point(0, 1));
            CommitHoldingPattern(point: new Point(3, 2));
            CommitHoldingPattern(point: new Point(2, 2));
            CommitHoldingPattern(point: new Point(2, 10));

            var polygon = new Polygon
            {
                ExteriorRing = new Ring { new Point(0, 0), new Point(5, 0), new Point(5, 5), new Point(0, 5), new Point(0, 0) }
            };
            polygon.InteriorRingList.Add(new Ring { new Point(1, 1), new Point(3, 1), new Point(3, 3), new Point(1, 3), new Point(1, 1) });

            var spatialOps = new Within
            {
                PropertyName = "OutboundLegSpan.EndPoint.PointChoice.Position.Geo",
                Geometry = polygon
            };

            var filter = new Filter(new OperationChoice(spatialOps));

            var res = GetHoldingPatterns(filter);

            Assert.Equal(3, res.Count);
            Assert.True(res.All(x => x.OutboundLegSpan.EndPoint.PointChoice.Position.Geo.X != 2));
        }

        [Fact]
        public void SpatialPointInMultiPolygon()
        {
            CommitHoldingPattern(point: new Point(1, 1));
            CommitHoldingPattern(point: new Point(1, 0));
            CommitHoldingPattern(point: new Point(-1, -1));
            CommitHoldingPattern(point: new Point(-1, 0));
            CommitHoldingPattern(point: new Point(10, 10));
            CommitHoldingPattern(point: new Point(-10, -10));

            var multiPolygon = new MultiPolygon
            {
                new Polygon
                {
                    ExteriorRing = new Ring
                    {
                        new Point(0, 0), new Point(5, 0) , new Point(5, 5) , new Point(0, 5), new Point(0, 0)
                    }
                },

                new Polygon
                {
                    ExteriorRing = new Ring
                    {
                        new Point(0, 0), new Point(-5, 0) , new Point(-5, -5) , new Point(0, -5), new Point(0, 0)
                    }
                }
            };

            var spatialOps = new Within
            {
                PropertyName = "OutboundLegSpan.EndPoint.PointChoice.Position.Geo",
                Geometry = multiPolygon
            };

            var filter = new Filter(new OperationChoice(spatialOps));

            var res = GetHoldingPatterns(filter);

            Assert.Equal(4, res.Count);
            Assert.True(res.All(x => Math.Abs(x.OutboundLegSpan.EndPoint.PointChoice.Position.Geo.X) < 5));
        }

        [Fact]
        public void SpatialWithinException()
        {
            CommitHoldingPattern(point: new Point(1, 1));

            var spatialOps = new Within
            {
                PropertyName = "OutboundLegSpan.EndPoint.PointChoice.Position.Geo",
                Geometry = new Point(1, 1)
            };

            var filter = new Filter(new OperationChoice(spatialOps));

            Assert.Throws<ArgumentException>(() => GetHoldingPatterns(filter));
        }

        [Fact]
        public void SpatialPointInExtend()
        {
            CommitHoldingPattern(point: new Point(1, 1));
            CommitHoldingPattern(point: new Point(1, 0));
            CommitHoldingPattern(point: new Point(10, 10));

            var spatialOps = new InExtend
            {
                PropertyName = "OutboundLegSpan.EndPoint.PointChoice.Position.Geo",
                MinX = 0,
                MaxX = 5,
                MinY = 0,
                MaxY = 5
            };

            var filter = new Filter(new OperationChoice(spatialOps));

            var res = GetHoldingPatterns(filter);

            Assert.Equal(2, res.Count);
            Assert.True(res.All(x => x.OutboundLegSpan.EndPoint.PointChoice.Position.Geo.X == 1));
        }

        [Fact]
        public void SpatialPointDWithin()
        {
            CommitHoldingPattern(point: new Point(1, 1));
            CommitHoldingPattern(point: new Point(1, 0));
            CommitHoldingPattern(point: new Point(2, 2));

            var spatialOps = new DWithin
            {
                PropertyName = "OutboundLegSpan.EndPoint.PointChoice.Position.Geo",
                Distance = new ValDistance(200, UomDistance.KM),
                Point = new Point(0, 0)
            };

            var filter = new Filter(new OperationChoice(spatialOps));

            var res = GetHoldingPatterns(filter);

            Assert.Equal(2, res.Count);
            Assert.True(res.All(x => x.OutboundLegSpan.EndPoint.PointChoice.Position.Geo.X == 1));
        }

        #endregion

        #region For MultiPolygon

        public MultiPolygon GetMultiPolygon(double x, double y, double eps = 0.0001)
        {
            var p = new Point(x, y);
            var multiPolygon = new MultiPolygon
            {
                new Polygon
                {
                    ExteriorRing = new Ring
                    {
                        new Point(p.X - eps, p.Y - eps), new Point(p.X + eps, p.Y - eps),
                        new Point(p.X + eps, p.Y + eps), new Point(p.X - eps, p.Y + eps), new Point(p.X - eps, p.Y - eps)
                    }
                }
            };

            return multiPolygon;
        }

        [Fact]
        public void SpatialMultiPolygonInExtend()
        {
            CommitObstacleArea(GetMultiPolygon(1, 1));
            CommitObstacleArea(GetMultiPolygon(1, 0.1));
            CommitObstacleArea(GetMultiPolygon(10, 10));

            var spatialOps = new InExtend
            {
                PropertyName = "SurfaceExtent.Geo",
                MinX = 0,
                MaxX = 5,
                MinY = 0,
                MaxY = 5
            };

            var filter = new Filter(new OperationChoice(spatialOps));

            var res = GetObstacleArea(filter);

            Assert.Equal(2, res.Count);
            Assert.True(res.All(x => Math.Abs(x.SurfaceExtent.Geo[0].ExteriorRing[0].X - 1) <= 0.0001));
        }

        [Fact]
        public void SpatialPolygontDWithin()
        {
            CommitObstacleArea(GetMultiPolygon(1, 1));
            CommitObstacleArea(GetMultiPolygon(1, 0.1));
            CommitObstacleArea(GetMultiPolygon(2, 2));

            var spatialOps = new DWithin
            {
                PropertyName = "SurfaceExtent.Geo",
                Distance = new ValDistance(200, UomDistance.KM),
                Point = new Point(0, 0)
            };

            var filter = new Filter(new OperationChoice(spatialOps));

            var res = GetObstacleArea(filter);

            Assert.Equal(2, res.Count);
            Assert.True(res.All(x => Math.Abs(x.SurfaceExtent.Geo[0].ExteriorRing[0].X - 1) <= 0.0001));
        }

        #endregion

        #endregion
    }
}
