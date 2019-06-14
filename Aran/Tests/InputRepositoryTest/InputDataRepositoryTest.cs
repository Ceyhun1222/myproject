using Aran.Aim;
using Aran.Aim.Data.InputRepository;
using Aran.Aim.Features;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InputRepositoryTest
{
    
    public class InputDataRepositoryTest
    {
        [Fact]
        public void IsFeaturesAddCorrectly()
        {
            var inputRepository = new InputDataRepository();

            Fixture fixture = new Fixture();
            var count1 = 20;
            var list = fixture.CreateMany<Runway>(count1).ToList<Feature>();
            inputRepository.AddFeatures(list);
            Assert.Equal(inputRepository.FeatureList.Count, list.Count);
        }

        [Theory]
        [InlineData(15)]
        [InlineData(25)]
        [InlineData(34)]
        [InlineData(12)]
        public void IsUniqueFeatureAdded(int count)
        {
            var inputRepository = new InputDataRepository();

            Fixture fixture = new Fixture();
            var count1 = count;
            var list = fixture.CreateMany<Runway>(count1).ToList<Feature>();

            inputRepository.AddFeatures(list);

            var count2 = count;
            list.AddRange(fixture.CreateMany<RunwayCentrelinePoint>(count2).ToList<Feature>());

            inputRepository.AddFeatures(list);
            Assert.Equal(inputRepository.FeatureList.Count, count1 + count2);
        }

        [Fact]
        public void FeaturesAddThrowExceptionIfParametrIsNull()
        {
            var inputRepository = new InputDataRepository();

            Assert.Throws<ArgumentNullException>(()=> inputRepository.AddFeatures(null));
        }


        [Fact]
        public void FeatureAdded()
        {
            var inputRepository = new InputDataRepository();
            var fixture = new Fixture();
            var rwyCntPoint =fixture.Create<RunwayCentrelinePoint>();

            inputRepository.AddFeature(rwyCntPoint);

            Assert.Equal(rwyCntPoint,(RunwayCentrelinePoint)inputRepository.GetFeatures(FeatureType.RunwayCentrelinePoint)[0]);
        }

        [Fact]
        public void FeatureAddThrowExceptionIfParametrIsNull()
        {
            var inputRepository = new InputDataRepository();

            Assert.Throws<ArgumentNullException>(() => inputRepository.AddFeature(null));
        }

        [Fact]
        public void GetFeaturesByTypeIsCorrect()
        {
            var inputRepository = new InputDataRepository();
            var fixture = new Fixture();
            var cntList = fixture.CreateMany<RunwayCentrelinePoint>(500).ToList<Feature>();
            
            inputRepository.AddFeatures(cntList);

            var dsgPtList = fixture.CreateMany<DesignatedPoint>(2999).ToList<DesignatedPoint>().ToList<Feature>();

            inputRepository.AddFeatures(dsgPtList);

            Assert.Equal(inputRepository.GetFeatures(FeatureType.RunwayCentrelinePoint).Count, cntList.Count);
            Assert.Equal(inputRepository.GetFeatures(FeatureType.DesignatedPoint).Count, dsgPtList.Count);
        }
    }
}
