using System;
using Aran.Aim.Features;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Exceptions;
using Xunit;

namespace Toss.Tests.Temporality
{
    [Collection(nameof(Names.ServiceCollection))]
    [TestCaseOrderer("Toss.Tests.PriorityOrderer", "Toss.Tests")]
    public class TempDeltaTests : TemporalityTests
    {
        public TempDeltaTests(DataFixture noAixmFixture) : base(noAixmFixture)
        {
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Temp_Commission(Feature feat)
        {
            Commit(feat);
            CreateTempDelta(feat);
            EditFeature(feat);
            _dataFixture.Commit(feat);

            AssertResult(feat, Interpretation.Snapshot, feat.TimeSlice.ValidTime.BeginPosition);

            //UndoTemp(true, false);
            feat = LoadState();
            AssertResult(feat, Interpretation.BaseLine, new DateTime?(), false);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Temp_Correction_Commission_WithSameValidTime(Feature feat)
        {
            Commit(feat);
            CreateTempDelta(feat);
            EditFeature(feat);
            _dataFixture.Commit(feat, false);
            EditFeature(feat);
            _dataFixture.Commit(feat, true);
            AssertResult(feat, Interpretation.Snapshot, feat.TimeSlice.ValidTime.BeginPosition);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Temp_Correction_Commission_WithDifferentValidTime(Feature feat)
        {
            Commit(feat);
            CreateTempDelta(feat);
            EditFeature(feat);
            _dataFixture.Commit(feat, false);
            feat.TimeSlice.ValidTime.BeginPosition = feat.TimeSlice.ValidTime.BeginPosition.AddHours(5);
            EditFeature(feat);
            var exception = Assert.Throws<OperationException>((Action)(() => _dataFixture.Commit(feat, true)));
            Assert.Equal("ValidTime Begin is not correct", exception.Message);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Temp_Decommission_Commissioned(Feature feat)
        {
            Commit(feat);
            CreateTempDelta(feat);
            EditFeature(feat);
            _dataFixture.Commit(feat);
            var tempValidBeginPosition = feat.TimeSlice.ValidTime.BeginPosition;
            feat = LoadState();
            _dataFixture.Decommit(feat);
            SaveState(feat);
            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType, Interpretation.Snapshot,
                tempValidBeginPosition);
            Assert.Empty(list);
            //UndoTemp(false);
            feat = LoadState();
            AssertResult(feat, Interpretation.BaseLine);
        }

        //private void UndoTemp(bool isEndNull = true, bool decrementCorrection = false)
        //{
        //    _arp.TimeSlice.ValidTime.BeginPosition = new DateTime(_arp.TimeSlice.ValidTime.BeginPosition.Year - 1,
        //        _arp.TimeSlice.ValidTime.BeginPosition.Month, _arp.TimeSlice.ValidTime.BeginPosition.Day);

        //    if (isEndNull)
        //        _arp.TimeSlice.ValidTime.EndPosition = null;
        //    if (decrementCorrection)
        //        _arp.TimeSlice.CorrectionNumber--;

        //    _arp.TimeSlice.Interpretation = TimeSliceInterpretationType.BASELINE;
        //    _arp.Designator = null;
        //}

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Temp_Cancel_Commissioned(Feature feat)
        {
            Commit(feat);
            CreateTempDelta(feat);
            EditFeature(feat);
            _dataFixture.Commit(feat);
            _dataFixture.Cancel(feat, Interpretation.TempDelta);
            //UndoTemp();
            feat = LoadState();
            AssertResult(feat, Interpretation.Snapshot, feat.TimeSlice.ValidTime.BeginPosition);
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator))]
        public void Temp_Decommission_Corrected_Commissioned(Feature feat)
        {
            Commit(feat);
            CreateTempDelta(feat);
            EditFeature(feat);
            _dataFixture.Commit(feat);
            var tempValidBeginPosition = feat.TimeSlice.ValidTime.BeginPosition;
            EditFeature(feat);
            _dataFixture.Commit(feat, true);
            feat = LoadState();
            _dataFixture.Decommit(feat);
            SaveState(feat);
            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType, Interpretation.Snapshot,
                tempValidBeginPosition);
            Assert.Empty(list);
            //UndoTemp(false, true);
            feat = LoadState();
            AssertResult(feat, Interpretation.BaseLine);
        }
    }
}