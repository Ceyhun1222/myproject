using System;
using System.Collections.Generic;
using System.Configuration;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Common.Enum;
using Ploeh.AutoFixture;
using Toss.Tests.AutoFixture;
using Xunit;

namespace Toss.Tests.Temporality
{
    public class TemporalityTests : IDisposable
    {
        protected readonly DataFixture _dataFixture;
        protected Feature _initState;
        protected int _initialSlotId;


        public TemporalityTests(DataFixture noAixmFixture)
        {
#if TestByTossm
            throw new Exception("TestByTosm parameter included");
#endif
            _dataFixture = noAixmFixture;
            _initialSlotId = _dataFixture.CreateSlot(true);
        }

        public static int FeatureTypesCount = 5;
        public static int FeatureDuplicatsCount = 200;

        static TemporalityTests()
        {
            try
            {
                var conf = ConfigurationManager.OpenExeConfiguration("Toss.Tests.dll");

                if (int.TryParse(conf.AppSettings?.Settings["FeatureTypesCount"]?.Value, out var intResult))
                    FeatureTypesCount = intResult;

                if (int.TryParse(conf.AppSettings?.Settings["FeatureDuplicatsCount"]?.Value, out intResult))
                    FeatureDuplicatsCount = intResult;
            }
            catch
            {
                // ignored
            }
        }

        public void Dispose()
        {
            _dataFixture.DeleteSlots();
        }

        protected void SaveState(Feature feat)
        {
            _initState = (Feature)feat.Clone();
        }

        protected Feature LoadState()
        {
            return (Feature)_initState.Clone();
        }

        protected Feature AssertResult(Feature feat, Interpretation interpretation = Interpretation.BaseLine,
            DateTime? dateTime = null, bool onlyPublishedSlot = false)
        {
            var list = _dataFixture.GetFeatures(feat.Identifier, feat.FeatureType, interpretation, dateTime,
                onlyPublishedSlot);
            Assert.NotEmpty(list);
            Assert.Single(list);
            Assert.True(AimObject.IsEquals(feat, list[0], true));
            return list[0];
        }

        protected void Commit(Feature feat)
        {
            ApplyTimeSlice(feat);
            _dataFixture.Commit((Feature)feat.Clone());
            SaveState(feat);
        }

        protected void ApplyTimeSlice(Feature feat)
        {
            feat.TimeSlice = new TimeSlice()
            {
                FeatureLifetime = new TimePeriod(_dataFixture.LastEffectiveDate),
                ValidTime = new TimePeriod(_dataFixture.LastEffectiveDate),
                SequenceNumber = 1,
                CorrectionNumber = 0
            };
        }

        protected void EditFeature(Feature feat)
        {
            Fixture fixture = new Fixture();
            fixture.Customizations.Add(new RandomEnumSequenceGenerator());
            fixture.Customizations.Add(new IgnoreProperties());
            var id = feat.Identifier;
            var timeSlice = feat.TimeSlice.Clone();
            feat.Assign((Feature)fixture.Create(feat.GetType()));
            feat.Identifier = id;
            feat.TimeSlice = (TimeSlice)timeSlice;
        }

        protected static Feature CreateFeature(FeatureType featureType)
        {
            Fixture fixture = new Fixture();
            fixture.Customizations.Add(new CustomDateTimeSequenceGenerator(new RandomDateTimeSequenceGenerator()));
            fixture.Customizations.Add(new RandomEnumSequenceGenerator());
            fixture.Customizations.Add(new IgnoreProperties());
            var feat = (Feature)fixture.Create(AimObjectFactory.CreateFeature(featureType).GetType());
            return feat;
        }

        protected void CreateTempDelta(Feature feat)
        {
            if (feat.TimeSlice == null)
                ApplyTimeSlice(feat);
            feat.TimeSlice.ValidTime.BeginPosition = feat.TimeSlice.ValidTime.BeginPosition.AddDays(1);
            feat.TimeSlice.ValidTime.EndPosition = feat.TimeSlice.ValidTime.BeginPosition.AddDays(1);
            feat.TimeSlice.Interpretation = TimeSliceInterpretationType.TEMPDELTA;
            feat.TimeSlice.SequenceNumber = 1;
            feat.TimeSlice.CorrectionNumber = 0;
        }

        protected List<Feature> GetFeature(Guid identifier, FeatureType featureType,
            Interpretation interpretation = Interpretation.BaseLine, DateTime? dateTime = null,
            bool onlyPublishedSlot = false, int workPackage = int.MinValue)
        {
            return _dataFixture.GetFeatures(identifier, featureType);
        }

        protected void AssertResult(Feature feat, List<Feature> list, int workPackage = int.MinValue)
        {
            Assert.NotEmpty(list);
            Assert.Single(list);
            Assert.StrictEqual(feat, list[0]);
        }
    }
}