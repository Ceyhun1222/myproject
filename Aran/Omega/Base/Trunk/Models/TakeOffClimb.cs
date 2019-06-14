using Aran.Omega.Enums;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using System;
using System.Collections.Generic;
using Point = Aran.Geometries.Point;

namespace Aran.Omega.Models
{
    public class TakeOffClimb:SurfaceBase
    {
        public TakeOffClimb()
        {
            SurfaceType = Aran.PANDA.Constants.SurfaceType.TakeOffClimb;
        }

        public double LengthOfInnerEdge { get; set; }
        public double DistanceFromThreshold { get; set; }
        public double Divergence { get; set; }
        public double FinalWidth { get; set; }
        public double Slope { get; set; }
        public double Length { get; set; }

        private IList<Info> _propList = new List<Info>();

        public override IList<Info> PropertyList
        {
            get
            {
                _propList.Clear();
                _propList.Add(new Info("Length of inner edge", Common.ConvertDistance(LengthOfInnerEdge, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Divergence", Divergence.ToString(), "%"));
                if (SurfaceType == SurfaceType.TakeOffClimb)
                {
                    _propList.Add(new Info("Final width",
                        Common.ConvertDistance(FinalWidth, RoundType.ToNearest).ToString(),InitOmega.DistanceConverter.Unit));
                    _propList.Add(new Info("Distance from runway end", Common.ConvertDistance(DistanceFromThreshold, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                
                }
                _propList.Add(new Info("Slope", Slope.ToString(), "%"));
                _propList.Add(new Info("Length", Common.ConvertDistance(Length, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                return _propList;
            }
            set => _propList = value;
        }

        public override PointPenetrateModel GetManualReport(Point obstaclePt)
        {
            foreach (Plane plane in Planes)
            {
                if (plane.Geo.IsPointInside(obstaclePt))
                {
                    //Get two point from strip which can create plane
                    //Then calculate  x1 and x2 distance from start point.
                    var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI,
                        obstaclePt);
                    var obstacleElev = obstaclePt.Z;

                    var surfaceElevation = plane.Param.GetZ(localObstaclePt);

                    var result = new PointPenetrateModel();
                    result.Surface = "Take of climb";
                    result.Elevation =Common.ConvertHeight(obstaclePt.Z,RoundType.ToNearest);
                    result.Plane = plane.Param.CreateEquationStr(surfaceElevation);
                    result.Penetration =Common.ConvertHeight(obstacleElev - surfaceElevation,RoundType.ToNearest);
                    return result;
                }
            }
            return null;
        }
    }
}
