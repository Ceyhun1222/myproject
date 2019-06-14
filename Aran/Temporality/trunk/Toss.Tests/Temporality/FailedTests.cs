using Aran.Aim.Features;
using Aran.Temporality.Common.Exceptions;
using Xunit;

namespace Toss.Tests.Temporality
{
    [Collection(nameof(Names.ServiceCollection))]
    [TestCaseOrderer("Toss.Tests.PriorityOrderer", "Toss.Tests")]
    public class FailedTests : TemporalityTests
    {
        public FailedTests(DataFixture noAixmFixture) : base(noAixmFixture)
        {
        }

        #region PermDelta

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void PermError_Correction_With_Diff_ValidTime(Feature feat)
        {
            return;

            Commit(feat);
            EditFeature(feat);
            feat.TimeSlice.ValidTime.BeginPosition = feat.TimeSlice.ValidTime.BeginPosition.AddDays(1);

            var exception = Assert.Throws<OperationException>(() => _dataFixture.Commit(feat, true));
            Assert.Equal("ValidTime Begin is not correct", exception.Message);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void PermError_Correction_With_Diff_Seq(Feature feat)
        {
            return;

            Commit(feat);
            _dataFixture.PublishSlot();
            _dataFixture.CreateSlot(true);
            EditFeature(feat);
            _dataFixture.Commit(feat);
            _dataFixture.PublishSlot();

            _dataFixture.CreateSlot(false, true);
            EditFeature(feat);
            feat.TimeSlice.SequenceNumber = 1;
            feat.TimeSlice.CorrectionNumber = 0;

            var exception = Assert.Throws<OperationException>(() => _dataFixture.Commit(feat, true));
            Assert.Equal("Versions that are later than its validTime should be notified", exception.Message);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void PermError_Correction_With_Equal_Seq_Diff_Corr(Feature feat)
        {
            return;

            Commit(feat);
            EditFeature(feat);
            _dataFixture.Commit(feat, true);
            _dataFixture.PublishSlot();

            var slot1 = _dataFixture.CreateSlot(false);
            EditFeature(feat);
            var feat1 = (Feature)feat.Clone();

            var slot2 = _dataFixture.CreateSlot(false);
            EditFeature(feat);
            var feat2 = (Feature)feat.Clone();

            _dataFixture.Commit(feat1, true, slot1);

            var exception = Assert.Throws<OperationException>(() => _dataFixture.Commit(feat2, true, slot2));
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void PermError_NewSequence_With_Diff_ValidTime(Feature feat)
        {
            return;

            Commit(feat);
            _dataFixture.PublishSlot();

            var slot1 = _dataFixture.CreateSlot(true);
            EditFeature(feat);

            var exception = Assert.Throws<OperationException>(() => _dataFixture.Commit(feat, workpackageId: _initialSlotId, onlySetEffectiveDate: true));
            Assert.Equal("Begin of valid is not equal to slot effective date", exception.Message);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void PermError_NewSequence_With_Diff_Seq(Feature feat)
        {
            return;

            Commit(feat);
            _dataFixture.PublishSlot();

            var slot1 = _dataFixture.CreateSlot(true);
            EditFeature(feat);
            var feat1 = (Feature)feat.Clone();

            var slot2 = _dataFixture.CreateSlot(false);
            EditFeature(feat);
            var feat2 = (Feature)feat;

            _dataFixture.Commit(feat1, workpackageId: slot1);
            _dataFixture.PublishSlot(slot1);

            _dataFixture.Commit(feat2, workpackageId: slot2);

            Assert.Throws<OperationException>(() => _dataFixture.PublishSlot(slot2));
        }

        #endregion

        #region TempDelta

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Temp_Commission_Without_PermDelta(Feature feat)
        {
            return;

            CreateTempDelta(feat);

            var exception = Assert.Throws<OperationException>(() => _dataFixture.Commit(feat));
            Assert.Equal("Feature should be comissioned", exception.Message);
        }

        #endregion
    }
}
