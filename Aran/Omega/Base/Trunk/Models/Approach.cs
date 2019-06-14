using System;
using System.Collections.Generic;
using Aran.Omega.Enums;
using Aran.Omega.Strategy.ObstacleCalculation;
using Aran.Omega.Strategy.UI;
using Aran.PANDA.Common;
using Point = Aran.Geometries.Point;

namespace Aran.Omega.Models
{
    public class Approach : SurfaceBase,IMultiplePlane
    {
        private List<Plane> _planeList;
        public Approach()
        {
            SurfaceType = Aran.PANDA.Constants.SurfaceType.Approach;
            _planeList = new List<Plane>();
        }

        public Plane Section1 { get; set; }
        public Plane Section2 { get; set; }
        public Plane Section3 { get; set; }


        public bool SecondPlane { get; set; }

        public double LengthOfInnerEdge { get; set; }
        public double DistanceFromThreshold { get; set; }
        public double Divergence { get; set; }
        public double FirstSectionLength { get; set; }
        public double FirstSectionSlope { get; set; }
        public double SecondSectionLength { get; set; }
        public double SecondSectionSlope { get; set; }
        public double HorizontalSectionLength { get; set; }
        public double HorizontalSectionTotalLength { get; set; }


        private IList<Info> _propList;
        public override IList<Info> PropertyList
        {
            get
            {
                _propList = new List<Info>
                {
                    new Info("Length of inner edge",
                        Common.ConvertDistance(LengthOfInnerEdge, RoundType.ToNearest).ToString(),
                        InitOmega.DistanceConverter.Unit),
                    new Info("Distance from runway end",
                        Common.ConvertDistance(DistanceFromThreshold, RoundType.ToNearest).ToString(),
                        InitOmega.DistanceConverter.Unit),
                    new Info("Divergence", Divergence.ToString(), "%"),
                    new Info("First section length",
                        Common.ConvertDistance(FirstSectionLength, RoundType.ToNearest).ToString(),
                        InitOmega.DistanceConverter.Unit),
                    new Info("First section slope", FirstSectionSlope.ToString(), "%")
                };
                if (SecondPlane)
                {
                    _propList.Add(new Info("Second section length", Common.ConvertDistance(SecondSectionLength, RoundType.ToNearest).ToString(),InitOmega.DistanceConverter.Unit));
                    _propList.Add(new Info("Second section slope", SecondSectionSlope.ToString(),"%"));
                    _propList.Add(new Info("Horizontal section length",Common.ConvertDistance(HorizontalSectionLength,RoundType.ToNearest).ToString(),InitOmega.DistanceConverter.Unit));
                    _propList.Add(new Info("Total length", Common.ConvertDistance(FirstSectionLength+SecondSectionLength+HorizontalSectionLength, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                }
                return _propList;
            }
            set { _propList = value; }
        }

        public override PointPenetrateModel GetManualReport(Point obstaclePt)
        {
            var equation = "";
            double penetrate = 0;
            if (GeoPrj.IsPointInside(obstaclePt))
            {
                var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI, obstaclePt);
                var obstacleElev = obstaclePt.Z;

                if (Section1.Geo.IsPointInside(obstaclePt))
                {
                    var surfaceElevation = Section1.Param.GetZ(localObstaclePt);
                    penetrate = obstacleElev - surfaceElevation;
                    equation = "I Sector:" + Section1.Param.CreateEquationStr(surfaceElevation);
                }

                else if (SecondPlane)
                {
                    if (Section2.Geo.IsPointInside(obstaclePt))
                    {
                        var surfaceElevation = Section2.Param.GetZ(localObstaclePt);
                        penetrate = obstacleElev - surfaceElevation;
                        equation = "II Sector" + Section2.Param.CreateEquationStr(surfaceElevation);
                    }

                    else if (Section3.Geo.IsPointInside(obstaclePt))
                    {
                        penetrate = obstacleElev - Section3.Param.D;
                        equation = "Horizontal Sector: Z =" + Section3.Param.D;

                    }
                }
                var result = new PointPenetrateModel();
                result.Surface = "Approach";
                result.Elevation =Common.ConvertHeight(obstaclePt.Z,RoundType.ToNearest);
                result.Plane = equation;
                result.Penetration = Common.ConvertHeight(penetrate, RoundType.ToNearest);
                return result;
            }
            return null;

        }

        public override List<Plane> Planes
        {
            get
            {
                _planeList.Clear();
                _planeList.Add(Section1);
                if (SecondPlane)
                {
                    _planeList.Add(Section2);
                    _planeList.Add(Section3);
                }
                return _planeList;
            }
            set
            {
                _planeList = value;
            }
        }
    }
}
