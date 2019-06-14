using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Ploeh.AutoFixture;
using Toss.Tests.AutoFixture;
using Toss.Tests.Temporality;

namespace Toss.Tests
{
    class DataGenerator
    {
        public static IEnumerable<object[]> GetFeatures()
        {
            int count = 0;
            Fixture fixture = new Fixture();
            fixture.Customizations.Add(new CustomDateTimeSequenceGenerator(new RandomDateTimeSequenceGenerator()));
            fixture.Customizations.Add(new RandomEnumSequenceGenerator());
            fixture.Customizations.Add(new IgnoreProperties());
            foreach (FeatureType featType in Enum.GetValues(typeof(FeatureType)))
            {
                count++;
                if (count > TemporalityTests.FeatureTypesCount)
                {
                    yield break;
                }
                var feat = fixture.Create(AimObjectFactory.CreateFeature(featType).GetType());
                yield return new[] { feat };
            }
        }

        public static IEnumerable<object[]> GetTestFeatures()
        {
            Fixture fixture = new Fixture();
            fixture.Customizations.Add(new CustomDateTimeSequenceGenerator(new RandomDateTimeSequenceGenerator()));
            fixture.Customizations.Add(new RandomEnumSequenceGenerator());
            fixture.Customizations.Add(new IgnoreProperties());

            var types = new[] { FeatureType.AirportHeliport, FeatureType.WorkArea };

            foreach (var featureType in types)
            {
                var feat = fixture.Create(AimObjectFactory.CreateFeature(featureType).GetType());
                yield return new[] { feat };
            }
        }

        public static IEnumerable<object[]> GetFeatureTypes()
        {
            int count = 0;
            foreach (var featType in Enum.GetValues(typeof(FeatureType)))
            {
                count++;
                if (count > TemporalityTests.FeatureTypesCount)
                {
                    yield break;
                }
                yield return new[] { featType };
            }
        }
    }
}
