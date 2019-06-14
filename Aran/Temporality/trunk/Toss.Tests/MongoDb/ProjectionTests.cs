using System;
using System.Configuration;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Toss.Tests.Temporality;
using Xunit;

namespace Toss.Tests.MongoDb
{
    [Collection(nameof(Names.ServiceCollection))]
    [TestCaseOrderer("Toss.Tests.PriorityOrderer", "Toss.Tests")]
    public class ProjectionTests : TemporalityTests
    {
        public ProjectionTests(DataFixture noAixmFixture) : base(noAixmFixture)
        {
            var value = ConfigUtil.RepositoryType;

            if (value != RepositoryType.MongoRepository && value != RepositoryType.MongoWithBackupRepository)
            {
                throw new Exception("Need to select Mongo repository");
            }
        }

        private HoldingPattern CommitHoldingPattern()
        {
            HoldingPattern feat = (HoldingPattern)CreateFeature(FeatureType.HoldingPattern);

            feat.InboundCourse = 1;
            feat.Instruction = "Instruction";

            feat.HoldingPoint = new AerialRefuellingPoint();
            feat.HoldingPoint.FacilityMakeup.Add(
                new PointReference
                {
                    PostFixTolerance = new ValDistanceSigned(1, UomDistance.CM)
                }
            );

            feat.OutboundLegSpan = new HoldingPatternLength();
            feat.OutboundLegSpan.EndPoint = new TerminalSegmentPoint();
            feat.OutboundLegSpan.EndPoint.PointChoice = new SignificantPoint
            {
                Position = new AixmPoint
                {
                    HorizontalAccuracy = new ValDistance(1, UomDistance.CM),
                    Id = 1
                }
            };
            feat.OutboundLegSpan.EndPoint.PointChoice.Position.Geo.X = 1;
            feat.OutboundLegSpan.EndPoint.PointChoice.Position.Geo.Y = 1;
            feat.OutboundLegSpan.EndPoint.PointChoice.Position.Geo.Z = 1;

            feat.OutboundLegSpan.EndPoint.PointChoice = new SignificantPoint
            {
                NavaidSystem = new FeatureRef
                {
                    FeatureType = FeatureType.Navaid,
                    Id = 1,
                    Identifier = Guid.NewGuid()
                }
            };

            Assert.Equal(1, feat.InboundCourse);
            Assert.Equal("Instruction", feat.Instruction);
            Assert.NotEmpty(feat.HoldingPoint.FacilityMakeup);
            Assert.Equal(FeatureType.Navaid, feat.OutboundLegSpan?.EndPoint?.PointChoice?.NavaidSystem?.FeatureType);

            Commit(feat);

            Assert.NotNull(feat.TimeSlice?.ValidTime?.BeginPosition);

            return feat;
        }

        private Navaid CommitNavaid()
        {
            Navaid feat = (Navaid)CreateFeature(FeatureType.Navaid);

            var navaidComponent = new NavaidComponent();
            navaidComponent.TheNavaidEquipment = new AbstractNavaidEquipmentRef
            {
                Id = 1,
                Identifier = Guid.NewGuid(),
                Type = NavaidEquipmentType.DME
            };
            feat.NavaidEquipment.Add(navaidComponent);

            Commit(feat);

            Assert.NotNull(feat.TimeSlice?.ValidTime?.BeginPosition);

            return feat;
        }

        private HoldingPattern GetHoldingPattern(Guid identifier, Projection projection)
        {
            var res = _dataFixture.GetActualDataByDate(new FeatureId
            {
                FeatureTypeId = (int)FeatureType.HoldingPattern,
                Guid = identifier,
                WorkPackage = _dataFixture.LastSlotId
            }, true, DateTime.Now.AddYears(1), Interpretation.BaseLine, null, null, projection);

            Assert.NotNull(res);
            Assert.NotEmpty(res);
            return (HoldingPattern)res.First().Data.Feature;
        }

        private Navaid GetNavaid(Guid identifier, Projection projection)
        {
            var res = _dataFixture.GetActualDataByDate(new FeatureId
            {
                FeatureTypeId = (int)FeatureType.Navaid,
                Guid = identifier,
                WorkPackage = _dataFixture.LastSlotId
            }, true, DateTime.Now.AddYears(1), Interpretation.BaseLine, null, null, projection);

            Assert.NotNull(res);
            Assert.NotEmpty(res);
            return (Navaid)res.First().Data.Feature;
        }

        [Fact]
        public void FeatureRefReadWrite()
        {
            var feat = CommitNavaid();
            Assert.NotNull(feat.NavaidEquipment?.First()?.TheNavaidEquipment?.Type);
            var res = GetNavaid(feat.Identifier, null);
            Assert.Equal(feat.NavaidEquipment?.First()?.TheNavaidEquipment?.Type, res?.NavaidEquipment?.First()?.TheNavaidEquipment?.Type);

            var feat2 = CommitHoldingPattern();
            Assert.NotNull(feat2.OutboundLegSpan?.EndPoint?.PointChoice?.NavaidSystem?.FeatureType);
            var res2 = GetHoldingPattern(feat2.Identifier, null);
            Assert.Equal(feat2.OutboundLegSpan?.EndPoint?.PointChoice?.NavaidSystem?.FeatureType, res2?.OutboundLegSpan?.EndPoint?.PointChoice?.NavaidSystem?.FeatureType);
        }

        [Fact]
        public void ChoiceInclude()
        {
            var feat = CommitHoldingPattern();

            var projection = Projection.Include("OutboundLegSpan.EndPoint.PointChoice.NavaidSystem");
            var res = GetHoldingPattern(feat.Identifier, projection);

            Assert.Null(res.InboundCourse);
            Assert.Null(res.Instruction);
            Assert.Null(res.HoldingPoint);
            Assert.NotNull(res.OutboundLegSpan.EndPoint.PointChoice.NavaidSystem);
        }

        [Fact]
        public void ChoiceExclude()
        {
            var feat = CommitHoldingPattern();

            var projection = Projection.Exclude("OutboundLegSpan.EndPoint.PointChoice.NavaidSystem");
            var res = GetHoldingPattern(feat.Identifier, projection);

            Assert.NotNull(res.InboundCourse);
            Assert.NotNull(res.Instruction);
            Assert.NotNull(res.HoldingPoint);
            Assert.Null(res.OutboundLegSpan.EndPoint.PointChoice.NavaidSystem);
        }

        [Fact]
        public void HasMainPropertiesWhenInclude()
        {
            var feat = CommitHoldingPattern();

            var projection = Projection.Include("InboundCourse");
            var res = GetHoldingPattern(feat.Identifier, projection);

            Assert.Equal(feat.TimeSlice.ValidTime.BeginPosition, res.TimeSlice?.ValidTime?.BeginPosition);
        }

        [Fact]
        public void SimpleInclude()
        {
            var feat = CommitHoldingPattern();

            var projection = Projection.Include("InboundCourse");
            var res = GetHoldingPattern(feat.Identifier, projection);

            Assert.NotNull(res.InboundCourse);
            Assert.Null(res.Instruction);
        }

        [Fact]
        public void SimpleExclude()
        {
            var feat = CommitHoldingPattern();

            var projection = Projection.Exclude("InboundCourse");
            var res = GetHoldingPattern(feat.Identifier, projection);

            Assert.Null(res.InboundCourse);
            Assert.NotNull(res.Instruction);
        }

        [Fact]
        public void MultipleInclude()
        {
            var feat = CommitHoldingPattern();

            var projection = Projection.Include("InboundCourse", "Instruction");
            var res = GetHoldingPattern(feat.Identifier, projection);

            Assert.NotNull(res.InboundCourse);
            Assert.NotNull(res.Instruction);
            Assert.Null(res.HoldingPoint);
        }

        [Fact]
        public void MultipleExclude()
        {
            var feat = CommitHoldingPattern();

            var projection = Projection.Exclude("InboundCourse", "Instruction");
            var res = GetHoldingPattern(feat.Identifier, projection);

            Assert.Null(res.InboundCourse);
            Assert.Null(res.Instruction);
            Assert.NotNull(res.HoldingPoint);
        }

        [Fact]
        public void IncorrectExclude()
        {
            var feat = CommitHoldingPattern();

            var projection = Projection.Exclude("123");
            Assert.Throws<ArgumentException>(() => GetHoldingPattern(feat.Identifier, projection));

            projection = Projection.Exclude("HoldingPoint.asd");
            Assert.Throws<ArgumentException>(() => GetHoldingPattern(feat.Identifier, projection));
        }

        [Fact]
        public void IncorrectInclude()
        {
            var feat = CommitHoldingPattern();

            var projection = Projection.Include("123");
            Assert.Throws<ArgumentException>(() => GetHoldingPattern(feat.Identifier, projection));

            projection = Projection.Include("HoldingPoint.asd");
            Assert.Throws<ArgumentException>(() => GetHoldingPattern(feat.Identifier, projection));
        }

        [Fact]
        public void AbstractInclude()
        {
            var feat = CommitHoldingPattern();

            var projection = Projection.Include("HoldingPoint.FacilityMakeup");
            var res = GetHoldingPattern(feat.Identifier, projection);

            Assert.Null(res.InboundCourse);
            Assert.Null(res.Instruction);
            Assert.NotEmpty(res.HoldingPoint.FacilityMakeup);
        }

        [Fact]
        public void AbstractExclude()
        {
            var feat = CommitHoldingPattern();

            var projection = Projection.Exclude("HoldingPoint.FacilityMakeup");
            var res = GetHoldingPattern(feat.Identifier, projection);

            Assert.NotNull(res.InboundCourse);
            Assert.NotNull(res.Instruction);
            Assert.NotNull(res.HoldingPoint);
            Assert.Empty(res.HoldingPoint.FacilityMakeup);
        }

        [Fact]
        public void MetaInfoExclude()
        {
            var feat = CommitHoldingPattern();
            var projection = Projection.Exclude("Identifier", "timeslice");
            var res = GetHoldingPattern(feat.Identifier, projection);

            Assert.NotEqual(default(Guid), res.Identifier);
            Assert.NotNull(res.TimeSlice);
        }

        [Fact]
        public void DataTypePropertyInclude()
        {
            var feat = CommitHoldingPattern();

            var projection = Projection.Include("OutboundLegSpan.EndPoint.PointChoice.NavaidSystem.Identifier");
            var res = GetHoldingPattern(feat.Identifier, projection);

            Assert.Null(res.InboundCourse);
            Assert.Null(res.Instruction);
            Assert.Null(res.HoldingPoint);
            Assert.NotNull(res.OutboundLegSpan.EndPoint.PointChoice.NavaidSystem.FeatureType);
        }

        [Fact]
        public void DataTypePropertyExclude()
        {
            var feat = CommitHoldingPattern();

            var projection = Projection.Exclude("OutboundLegSpan.EndPoint.PointChoice.NavaidSystem.Identifier");
            var res = GetHoldingPattern(feat.Identifier, projection);

            Assert.NotNull(res.InboundCourse);
            Assert.NotNull(res.Instruction);
            Assert.NotNull(res.HoldingPoint);
            Assert.Null(res.OutboundLegSpan.EndPoint.PointChoice.NavaidSystem);
        }
    }
}
