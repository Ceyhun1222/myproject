using System;
using System.Configuration;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.PropertyEnum;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Toss.Tests.Temporality;
using Xunit;

namespace Toss.Tests.MongoDb
{
    [Collection(nameof(Names.ServiceCollection))]
    [TestCaseOrderer("Toss.Tests.PriorityOrderer", "Toss.Tests")]
    public class NilReasonTests : TemporalityTests
    {
        public NilReasonTests(DataFixture noAixmFixture) : base(noAixmFixture)
        {
            var value = ConfigUtil.RepositoryType;

            if (value != RepositoryType.MongoRepository && value != RepositoryType.MongoWithBackupRepository)
            {
                throw new Exception("Need to select Mongo repository");
            }
        }

        [Fact]
        public void EmptyNilReasons()
        {
            var feature = CreateFeature(FeatureType.Navaid);
            Commit(feature);
            var res = AssertResult(feature);
            var classInfo = AimMetadata.GetClassInfoByIndex(feature);
            foreach (var propInfo in classInfo.Properties)
            {
                Assert.Null(res.GetNilReason(propInfo.Index));
            }
        }

        [Fact]
        public void NotEmptyNilReasons()
        {
            var feature = (Navaid) CreateFeature(FeatureType.Navaid);
            feature.Name = null;
            feature.IntegrityLevel = null;
            feature.CourseQuality = null;
            feature.Designator = null;
            feature.FlightChecked = null;
            feature.SetNilReason(PropertyNavaid.Name, NilReason.inapplicable);
            feature.SetNilReason(PropertyNavaid.IntegrityLevel, NilReason.missing);
            feature.SetNilReason(PropertyNavaid.CourseQuality, NilReason.template);
            feature.SetNilReason(PropertyNavaid.Designator, NilReason.unknown);
            feature.SetNilReason(PropertyNavaid.FlightChecked, NilReason.withheld);
            Commit(feature);
            var res = AssertResult(feature);
            Assert.Equal(NilReason.inapplicable, res.GetNilReason(PropertyNavaid.Name));
            Assert.Equal(NilReason.missing, res.GetNilReason(PropertyNavaid.IntegrityLevel));
            Assert.Equal(NilReason.template, res.GetNilReason(PropertyNavaid.CourseQuality));
            Assert.Equal(NilReason.unknown, res.GetNilReason(PropertyNavaid.Designator));
            Assert.Equal(NilReason.withheld, res.GetNilReason(PropertyNavaid.FlightChecked));
        }
    }
}
