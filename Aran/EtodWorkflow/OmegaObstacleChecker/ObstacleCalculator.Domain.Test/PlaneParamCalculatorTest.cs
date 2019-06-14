using System;
using Xunit;
using GeoAPI.Geometries;
using ObstacleCalculator.Domain.Utils;

namespace ObstacleCalculator.Domain.Test
{
    public class PlaneParamCalculatorTest
    {
        [Fact]
        public void Test_CalcPlaneParam_ShouldSendException_WhenAnyParametrsNull()
        {
            var ptA = new Coordinate();
            var ptC = new Coordinate();

            Xunit.Assert.Throws<ArgumentNullException>(() =>
                PlaneParamCalculator.CalcPlaneParam(ptA, null, ptC));
        }

        [Fact]
        public void Test_CalcPlaneParam_ShouldMatch_WhenParametrsIsNotNull()
        {
            var ptA = new Coordinate(1, 2, -2);
            var ptB = new Coordinate(3, -2, 1);
            var ptC = new Coordinate(5, 1, -4);
            var planeParam = PlaneParamCalculator.CalcPlaneParam(ptA, ptB,ptC);

            Console.WriteLine(planeParam.B);
            Assert.True(Math.Abs(planeParam.A - 11) < 0.1);
            Assert.True(Math.Abs(planeParam.B - 16) < 0.1);
            Assert.True(Math.Abs(planeParam.C - 14) < 0.1);
            Assert.True(Math.Abs(planeParam.D + 15) < 0.1);
        }

        //[Theory]
        //[InlineData(5,10)]
        //[InlineData(500, 10000)]
        //[InlineData(1, 10090)]
        //[InlineData(-3434, 10)]
        //[InlineData(10, 10)]
        //public void Test_CalPlaneParam_ShouldMathZ_WhenPointAreSame(int x,int y)
        //{
        //    var ptA = new Coordinate(1, 2, -2);
        //    var ptB = new Coordinate(3, -2, 1);
        //    var ptC = new Coordinate(5, 1, -4);

        //    var planeParam1 = PlaneParamCalculator.CalcPlaneParam(ptA, ptB, ptC);

        //    var planeParam2 = PlaneParamCalculator.CalcPlaneParamOld(ptA, ptB, ptC);

        //    var pt = new Coordinate(x,y);
        //    Assert.True(
        //        Math.Abs(planeParam1.GetZ(pt)-planeParam2.GetZ2(pt))<0.1);
        //}
    }
}
