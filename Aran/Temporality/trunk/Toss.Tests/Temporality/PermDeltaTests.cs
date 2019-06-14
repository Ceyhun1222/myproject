using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Metadata.ISO;
using Aran.Temporality.Common.Exceptions;
using Xunit;

namespace Toss.Tests.Temporality
{
    [Collection(nameof(Names.ServiceCollection))]
    [TestCaseOrderer("Toss.Tests.PriorityOrderer", "Toss.Tests")]
    public class PermDeltaTests : TemporalityTests
    {
        public PermDeltaTests(DataFixture noAixmFixture) : base(noAixmFixture)
        {
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Comission(Feature feat)
        {
            Commit(feat);
            AssertResult(feat);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Correct_Comissioned(Feature feat)
        {
            Commit(feat);
            EditFeature(feat);
            _dataFixture.Commit(feat, true);
            AssertResult(feat);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Decommission_Corrected_Commissioned(Feature feat)
        {
            Commit(feat);
            EditFeature(feat);
            _dataFixture.Commit(feat, true);
            _dataFixture.Decommit(feat);

            AssertResult(feat);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Cancel_Corrected_Commissioned(Feature feat)
        {
            Commit(feat);
            EditFeature(feat);
            _dataFixture.Commit(feat, true);
            _dataFixture.Cancel(feat);

            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType);
            Assert.Empty(list);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Publish_Canceled_Corrected_Commissioned(Feature feat)
        {
            Commit(feat);
            EditFeature(feat);
            _dataFixture.Commit(feat, true);
            _dataFixture.Cancel(feat);
            _dataFixture.PublishSlot();

            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType);
            Assert.Empty(list);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Publish_Corrected_Commissioned(Feature feat)
        {
            Commit(feat);
            EditFeature(feat);
            _dataFixture.Commit(feat, true);
            _dataFixture.PublishSlot();

            AssertResult(feat, onlyPublishedSlot: true);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Decommission_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.Decommit(feat);

            AssertResult(feat);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Cancel_Decommissioned_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.Decommit(feat);
            _dataFixture.Cancel(feat);

            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType);
            Assert.Empty(list);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Publish_Canceled_Decommissioned_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.Decommit(feat);
            _dataFixture.Cancel(feat);
            _dataFixture.PublishSlot();

            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType);
            Assert.Empty(list);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Publish_Decommissioned_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.Decommit(feat);
            _dataFixture.PublishSlot();

            AssertResult(feat, onlyPublishedSlot: true);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Cancel_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.Cancel(feat);

            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType);
            Assert.Empty(list);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Publish_Canceled_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.Cancel(feat);
            _dataFixture.PublishSlot();

            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType);
            Assert.Empty(list);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Publish_Comissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            AssertResult(feat, onlyPublishedSlot: true);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Correct_Published_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(false);
            EditFeature(feat);
            _dataFixture.Commit(feat, true);

            AssertResult(feat);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Decommission_Corrected_Published_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(false);
            EditFeature(feat);
            _dataFixture.Commit(feat, true);
            _dataFixture.Decommit(feat);

            AssertResult(feat);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Cancel_Corrected_Published_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(false);
            EditFeature(feat);
            _dataFixture.Cancel(feat);

            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType);
            Assert.Empty(list);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Publish_Canceled_Corrected_Published_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(false);
            EditFeature(feat);
            _dataFixture.Cancel(feat);
            _dataFixture.PublishSlot();

            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType);
            Assert.Empty(list);

        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Publish_Corrected_Published_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(false);
            EditFeature(feat);
            _dataFixture.Commit(feat, true);

            AssertResult(feat);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void NewSequence_Published_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(true);
            EditFeature(feat);
            _dataFixture.Commit(feat);

            AssertResult(feat);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Decommission_NewSequence_Published_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(true);
            EditFeature(feat);
            _dataFixture.Commit(feat);
            _dataFixture.Decommit(feat);

            AssertResult(feat);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Cancel_NewSequence_Published_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(true);
            EditFeature(feat);
            _dataFixture.Commit(feat);
            _dataFixture.Cancel(feat);

            feat = LoadState();

            AssertResult(feat);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Publish_Canceled_NewSequence_Published_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(true);

            EditFeature(feat);
            _dataFixture.Commit(feat);
            _dataFixture.Cancel(feat);
            _dataFixture.PublishSlot();

            feat = LoadState();

            AssertResult(feat);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Publish_NewSequence_Published_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(true);
            EditFeature(feat);
            _dataFixture.Commit(feat);
            _dataFixture.PublishSlot();

            AssertResult(feat);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Decommission_Published_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(false);
            _dataFixture.Decommit(feat);

            AssertResult(feat);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Cancel_Published_Commisssioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(false);
            _dataFixture.Cancel(feat);

            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType);
            Assert.Empty(list);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Publish_Canceled_Published_Commissioned(Feature feat)
        {
            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(false);
            _dataFixture.Cancel(feat);
            _dataFixture.PublishSlot();

            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType);
            Assert.Empty(list);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void PermError_Comission_With_Seq_Diff_1(Feature feat)
        {
            ApplyTimeSlice(feat);
            feat.TimeSlice.SequenceNumber++;

            var exception = Assert.Throws<OperationException>(() => _dataFixture.Commit(feat));
            Assert.Equal("Feature should be comissioned", exception.Message);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void MetadataCommision(Feature feat)
        {
            feat.Metadata = new MdMetadata
            {
                IdentificationInfo =
                {
                    new MdAbstractIdentificationObject
                    {
                        Value = new MdDataIdentification
                        {
                            MetadataId = "asdasd",
                            Language = "asd",
                            Status =
                            {
                                new MdProgressCodeObject
                                {
                                    Value = MdProgressCode.Completed
                                },
                                new MdProgressCodeObject
                                {
                                    Value = MdProgressCode.Obsolete
                                }
                            },
                            Abstract = "Abstract",
                            Citation = new CiCitation
                            {
                                Identifier =
                                {
                                    new MdIdentifier
                                    {
                                        Code = "ads",
                                        MetadataId = "asdasd"
                                    }
                                }
                            }
                        }
                    }
                }
            };
            Commit(feat);
            AssertResult(feat);
        }
    }
}