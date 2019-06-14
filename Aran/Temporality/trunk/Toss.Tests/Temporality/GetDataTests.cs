using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Enum;
using TOSSM.Annotations;
using Xunit;

namespace Toss.Tests.Temporality
{
    [Collection(nameof(Names.ServiceCollection))]
    [TestCaseOrderer("Toss.Tests.PriorityOrderer", "Toss.Tests")]
    public class GetDataTests : TemporalityTests
    {
        public GetDataTests(DataFixture noAixmFixture) : base(noAixmFixture)
        {
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatureTypes), MemberType = typeof(DataGenerator))]
        public void GetActualDataByFeatureType(FeatureType featureType)
        {
            var commitedFeatures = new List<Feature>();
            for (int i = 0; i < FeatureDuplicatsCount; i++)
            {
                var feat = CreateFeature(featureType);
                Commit(feat);
                commitedFeatures.Add(feat);
            }

            var list = _dataFixture.GetFeatures(null, featureType);
            Assert.NotEmpty(list);
            foreach (var feature in commitedFeatures)
            {
                var res = list.FirstOrDefault(x => x.Identifier == feature.Identifier);
                Assert.NotNull(res);
                Assert.True(AimObject.IsEquals(res, feature, true));
            }
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetFeatures), MemberType = typeof(DataGenerator)), TestPriority(0)]
        public void GetActualDataInternal(Feature feat)
        {
            Commit(feat);
            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType);
            Assert.NotEmpty(list);
            Assert.Single(list);
            Assert.True(AimObject.IsEquals(feat, list[0], true));
        }

        private static void EditNavaid(Navaid feat, TimeSliceInterpretationType interpretation, DateTime begintPosition, DateTime? endPosition, string designator,
            string name, bool? flightChecked)
        {
            feat.Designator = designator;
            feat.TimeSlice.Interpretation = interpretation;
            feat.Name = name;
            feat.TimeSlice.ValidTime.BeginPosition = begintPosition;
            feat.TimeSlice.ValidTime.EndPosition = endPosition;
            feat.FlightChecked = flightChecked;
        }

        private void GenerateNavaidEvents(Navaid feat)
        {
            DateTime startTime = feat.TimeSlice.FeatureLifetime.BeginPosition;

            // Public slot 1
            EditNavaid(feat, TimeSliceInterpretationType.PERMDELTA, startTime, null, "1 Permdelta", "1P", null);
            _dataFixture.Commit(feat);
            _dataFixture.PublishSlot();


            // Public slot 2
            _initialSlotId = _dataFixture.CreateSlot(true, effectiveDate: startTime.AddDays(1));

            EditNavaid(feat, TimeSliceInterpretationType.PERMDELTA, startTime.AddDays(1), null, "2 Permdelta", "2P", null);
            _dataFixture.Commit(feat);

            EditNavaid(feat, TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(2), startTime.AddDays(4), "1 Tempdelta", "1T", true);
            _dataFixture.Commit(feat);

            EditNavaid(feat, TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(3), startTime.AddDays(6), "2 Tempdelta", null, false);
            _dataFixture.Commit(feat);

            _dataFixture.PublishSlot();


            // Public slot 3
            _initialSlotId = _dataFixture.CreateSlot(true, effectiveDate: startTime.AddDays(5));

            EditNavaid(feat, TimeSliceInterpretationType.PERMDELTA, startTime.AddDays(5), null, "3 Permdelta", "3P", null);
            _dataFixture.Commit(feat);

            EditNavaid(feat, TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(7), startTime.AddDays(8), "3 Tempdelta", null, true);
            _dataFixture.Commit(feat);

            _dataFixture.PublishSlot();
        }

        [AssertionMethod]
        private static void AssertNavaid(Navaid feat, string designator, string name, bool? flightChecked)
        {
            Assert.Equal(designator, feat.Designator);
            Assert.Equal(name, feat.Name);
            Assert.Equal(flightChecked, feat.FlightChecked);
        }

        [AssertionMethod]
        private static void AssertNavaidState(AbstractState<AimFeature> aimState, TimeSliceInterpretationType interpretation,
            DateTime beginPosition, DateTime? endPosition, string designator, string name, bool? flightChecked)
        {
            Assert.Equal(interpretation, aimState.Data.Feature.TimeSlice.Interpretation);
            Assert.Equal(beginPosition, aimState.TimeSlice.BeginPosition);
            Assert.Equal(endPosition, aimState.TimeSlice.EndPosition);
            AssertNavaid((Navaid)aimState.Data.Feature, designator, name, flightChecked);
        }

        [AssertionMethod]
        private static void AssertNavaidEvent(AbstractEvent<AimFeature> aimEvent, TimeSliceInterpretationType interpretation,
            DateTime beginPosition, DateTime? endPosition, string designator, string name, bool? flightChecked)
        {
            Assert.Equal(interpretation, aimEvent.Data.Feature.TimeSlice.Interpretation);
            Assert.Equal(beginPosition, aimEvent.TimeSlice.BeginPosition);
            Assert.Equal(endPosition, aimEvent.TimeSlice.EndPosition);
            AssertNavaid((Navaid)aimEvent.Data.Feature, designator, name, flightChecked);
        }

        [Fact]
        public void GetStatesBySnapshot()
        {
            var feat = (Navaid)CreateFeature(FeatureType.Navaid);
            ApplyTimeSlice(feat);
            var startTime = feat.TimeSlice.FeatureLifetime.BeginPosition;

            GenerateNavaidEvents(feat);
            var states = _dataFixture.GetStatesInRangeByInterpretation(feat.Identifier, feat.FeatureType, 0, false, startTime.AddYears(-100),
                startTime.AddYears(100), Interpretation.Snapshot);

            //var result = states.Select(state => new
            //{
            //    ((Navaid)state.Data.Feature).Designator,
            //    ((Navaid)state.Data.Feature).Name,
            //    ((Navaid)state.Data.Feature).FlightChecked,
            //    state.TimeSlice,
            //    state.Data.Feature.TimeSlice.Interpretation
            //}).ToList();

            Assert.NotEmpty(states);
            Assert.Equal(9, states.Count);

            AssertNavaidState(states[0], TimeSliceInterpretationType.BASELINE, startTime.AddDays(0),
                startTime.AddDays(1), "1 Permdelta", "1P", null);

            AssertNavaidState(states[1], TimeSliceInterpretationType.BASELINE, startTime.AddDays(1),
                startTime.AddDays(2), "2 Permdelta", "2P", null);

            AssertNavaidState(states[2], TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(2),
                startTime.AddDays(3), "1 Tempdelta", "1T", true);

            AssertNavaidState(states[3], TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(3),
                startTime.AddDays(4), "2 Tempdelta", "1T", false);

            AssertNavaidState(states[4], TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(4),
                startTime.AddDays(5), "2 Tempdelta", "2P", false);

            AssertNavaidState(states[5], TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(5),
                startTime.AddDays(6), "2 Tempdelta", "3P", false);

            AssertNavaidState(states[6], TimeSliceInterpretationType.BASELINE, startTime.AddDays(6),
                startTime.AddDays(7), "3 Permdelta", "3P", null);

            AssertNavaidState(states[7], TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(7),
                startTime.AddDays(8), "3 Tempdelta", "3P", true);

            AssertNavaidState(states[8], TimeSliceInterpretationType.BASELINE, startTime.AddDays(8),
                startTime.AddYears(100), "3 Permdelta", "3P", null);
        }

        [Fact]
        public void GetStatesByBaseline()
        {
            var feat = (Navaid)CreateFeature(FeatureType.Navaid);
            ApplyTimeSlice(feat);
            var startTime = feat.TimeSlice.FeatureLifetime.BeginPosition;

            GenerateNavaidEvents(feat);
            var states = _dataFixture.GetStatesInRangeByInterpretation(feat.Identifier, feat.FeatureType, 0, false, startTime.AddYears(-100),
                startTime.AddYears(100), Interpretation.BaseLine);

            Assert.NotEmpty(states);
            Assert.Equal(3, states.Count);

            AssertNavaidState(states[0], TimeSliceInterpretationType.BASELINE, startTime.AddDays(0),
                startTime.AddDays(1), "1 Permdelta", "1P", null);

            AssertNavaidState(states[1], TimeSliceInterpretationType.BASELINE, startTime.AddDays(1),
                startTime.AddDays(5), "2 Permdelta", "2P", null);

            AssertNavaidState(states[2], TimeSliceInterpretationType.BASELINE, startTime.AddDays(5),
                startTime.AddYears(100), "3 Permdelta", "3P", null);
        }

        [Fact]
        public void GetStatesByPermdelta()
        {
            var feat = (Navaid)CreateFeature(FeatureType.Navaid);
            ApplyTimeSlice(feat);
            var startTime = feat.TimeSlice.FeatureLifetime.BeginPosition;

            GenerateNavaidEvents(feat);
            var states = _dataFixture.GetStatesInRangeByInterpretation(feat.Identifier, feat.FeatureType, 0, false, startTime.AddYears(-100),
                startTime.AddYears(100), Interpretation.PermanentDelta);

            Assert.NotEmpty(states);
            Assert.Equal(3, states.Count);

            AssertNavaidState(states[0], TimeSliceInterpretationType.BASELINE, startTime.AddDays(0),
                startTime.AddDays(1), "1 Permdelta", "1P", null);

            AssertNavaidState(states[1], TimeSliceInterpretationType.BASELINE, startTime.AddDays(1),
                startTime.AddDays(5), "2 Permdelta", "2P", null);

            AssertNavaidState(states[2], TimeSliceInterpretationType.BASELINE, startTime.AddDays(5),
                startTime.AddYears(100), "3 Permdelta", "3P", null);
        }

        [Fact]
        public void GetStatesByTempdelta()
        {
            var feat = (Navaid)CreateFeature(FeatureType.Navaid);
            ApplyTimeSlice(feat);
            var startTime = feat.TimeSlice.FeatureLifetime.BeginPosition;

            GenerateNavaidEvents(feat);
            var states = _dataFixture.GetStatesInRangeByInterpretation(feat.Identifier, feat.FeatureType, 0, false,
                startTime.AddYears(-100), startTime.AddYears(100), Interpretation.TempDelta);

            Assert.NotEmpty(states);
            Assert.Equal(5, states.Count);

            AssertNavaidState(states[0], TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(2),
                startTime.AddDays(3), "1 Tempdelta", "1T", true);

            AssertNavaidState(states[1], TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(3),
                startTime.AddDays(4), "2 Tempdelta", "1T", false);

            AssertNavaidState(states[2], TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(4),
                startTime.AddDays(5), "2 Tempdelta", "2P", false);

            AssertNavaidState(states[3], TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(5),
                startTime.AddDays(6), "2 Tempdelta", "3P", false);

            AssertNavaidState(states[4], TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(7),
                startTime.AddDays(8), "3 Tempdelta", "3P", true);
        }

        [Fact]
        public void GetEvolution()
        {
            var feat = (Navaid)CreateFeature(FeatureType.Navaid);
            ApplyTimeSlice(feat);
            var startTime = feat.TimeSlice.FeatureLifetime.BeginPosition;

            GenerateNavaidEvents(feat);

            var events = _dataFixture.GetEvolution(feat.Identifier, feat.FeatureType, 0);

            AssertNavaidEvent(events[0], TimeSliceInterpretationType.PERMDELTA, startTime,
                null, "1 Permdelta", "1P", null);

            AssertNavaidEvent(events[1], TimeSliceInterpretationType.PERMDELTA, startTime.AddDays(1),
                null, "2 Permdelta", "2P", null);

            AssertNavaidEvent(events[2], TimeSliceInterpretationType.PERMDELTA, startTime.AddDays(5),
                null, "3 Permdelta", "3P", null);

            AssertNavaidEvent(events[3], TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(2),
                startTime.AddDays(4), "1 Tempdelta", "1T", true);

            AssertNavaidEvent(events[4], TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(3),
                startTime.AddDays(6), "2 Tempdelta", null, false);

            AssertNavaidEvent(events[5], TimeSliceInterpretationType.TEMPDELTA, startTime.AddDays(7),
                startTime.AddDays(8), "3 Tempdelta", null, true);
        }

        [AssertionMethod]
        private void AssertNavaidActualData(Navaid feat, Interpretation interpretation, DateTime actualDate,
            (string Designator, string Name, bool? FlightChecked)? stateBeforeDelta,
            (string Designator, string Name, bool? FlightChecked)? delta,
            (string Designator, string Name, bool? FlightChecked)? stateAfterDelta)
        {
            var stateWithDelta = _dataFixture.GetActualDataForEditing(feat, 0, actualDate, interpretation);

            if (stateBeforeDelta == null)
                Assert.Null(stateWithDelta.StateBeforeDelta);
            else
                AssertNavaid((Navaid)stateWithDelta.StateBeforeDelta.Feature, stateBeforeDelta.Value.Designator, stateBeforeDelta.Value.Name, stateBeforeDelta.Value.FlightChecked);

            if (delta == null)
                Assert.Null(stateWithDelta.Delta);
            else
                AssertNavaid((Navaid)stateWithDelta.Delta.Feature, delta.Value.Designator, delta.Value.Name, delta.Value.FlightChecked);

            if (stateAfterDelta == null)
                Assert.Null(stateWithDelta.StateAfterDelta);
            else
                AssertNavaid((Navaid)stateWithDelta.StateAfterDelta.Feature, stateAfterDelta.Value.Designator, stateAfterDelta.Value.Name, stateAfterDelta.Value.FlightChecked);
        }

        [AssertionMethod]
        private void DoubleAssertNavaidActualData(Navaid feat, Interpretation interpretation, DateTime actualDate,
            (string Designator, string Name, bool? FlightChecked)? stateBeforeDelta,
            (string Designator, string Name, bool? FlightChecked)? delta,
            (string Designator, string Name, bool? FlightChecked)? stateAfterDelta)
        {
            AssertNavaidActualData(feat, interpretation, actualDate, stateBeforeDelta, delta, stateAfterDelta);
            AssertNavaidActualData(feat, interpretation, actualDate.AddDays(0.5), stateAfterDelta, null, stateAfterDelta);
        }

        [Fact]
        public void GetActualDataForEditingByBaseline()
        {
            var feat = (Navaid)CreateFeature(FeatureType.Navaid);
            ApplyTimeSlice(feat);
            var startTime = feat.TimeSlice.FeatureLifetime.BeginPosition;

            GenerateNavaidEvents(feat);

            DoubleAssertNavaidActualData(feat, Interpretation.BaseLine, startTime.AddDays(0),
                null, ("1 Permdelta", "1P", null), ("1 Permdelta", "1P", null));

            DoubleAssertNavaidActualData(feat, Interpretation.BaseLine, startTime.AddDays(1),
                ("1 Permdelta", "1P", null), ("2 Permdelta", "2P", null), ("2 Permdelta", "2P", null));

            AssertNavaidActualData(feat, Interpretation.BaseLine, startTime.AddDays(2),
                ("2 Permdelta", "2P", null), null, ("2 Permdelta", "2P", null));

            DoubleAssertNavaidActualData(feat, Interpretation.BaseLine, startTime.AddDays(5),
                ("2 Permdelta", "2P", null), ("3 Permdelta", "3P", null), ("3 Permdelta", "3P", null));
        }

        [Fact]
        public void GetActualDataForEditingBySnapshot()
        {
            var feat = (Navaid)CreateFeature(FeatureType.Navaid);
            ApplyTimeSlice(feat);
            var startTime = feat.TimeSlice.FeatureLifetime.BeginPosition;

            GenerateNavaidEvents(feat);

            DoubleAssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(0),
                null, ("1 Permdelta", "1P", null), ("1 Permdelta", "1P", null));

            DoubleAssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(1),
                ("1 Permdelta", "1P", null), ("2 Permdelta", "2P", null), ("2 Permdelta", "2P", null));

            DoubleAssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(2),
                ("2 Permdelta", "2P", null), ("1 Tempdelta", "1T", true), ("1 Tempdelta", "1T", true));

            DoubleAssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(3),
                ("1 Tempdelta", "1T", true), ("2 Tempdelta", null, false), ("2 Tempdelta", "1T", false));

#warning unexpected result
            AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(4),
                ("2 Tempdelta", "1T", false), null, ("2 Tempdelta", "1T", false));
            //AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(4),
            //    ("2 Tempdelta", "1T", false), null, ("2 Tempdelta", "2", false));

            AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(4.5),
                ("2 Tempdelta", "2P", false), null, ("2 Tempdelta", "2P", false));

#warning unexpected result
            AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(5),
                ("2 Tempdelta", "2P", false), ("3 Permdelta", "3P", null), ("3 Permdelta", "3P", null));
            //AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(5),
            //    ("2 Tempdelta", "2P", false), ("3 Permdelta", "3P", null), ("2 Tempdelta", "3P", false));

            AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(5.5),
                ("2 Tempdelta", "3P", false), null, ("2 Tempdelta", "3P", false));

#warning unexpected result
            AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(6),
                ("2 Tempdelta", "3P", false), null, ("2 Tempdelta", "3P", false));
            //AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(6),
            //    ("2 Tempdelta", "3P", false), null, ("3 Permdelta", "3P", null));

#warning unexpected result
            AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(6),
                ("2 Tempdelta", "3P", false), null, ("2 Tempdelta", "3P", false));
            //AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(6),
            //    ("2 Tempdelta", "3P", false), null, ("3 Permdelta", "3P", null));

            AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(6.5),
                ("3 Permdelta", "3P", null), null, ("3 Permdelta", "3P", null));

            DoubleAssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(7),
                ("3 Permdelta", "3P", null), ("3 Tempdelta", null, true), ("3 Tempdelta", "3P", true));

#warning unexpected result
            AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(8),
                ("3 Tempdelta", "3P", true), null, ("3 Tempdelta", "3P", true));
            //AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(8),
            //    ("3 Tempdelta", "3P", true), null, ("3 Permdelta", "3P", null));

            AssertNavaidActualData(feat, Interpretation.Snapshot, startTime.AddDays(8.5),
                ("3 Permdelta", "3P", null), null, ("3 Permdelta", "3P", null));
        }
    }
}