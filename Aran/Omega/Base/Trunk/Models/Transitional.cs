using Aran.Omega.Enums;
using Aran.Omega.Strategy.ObstacleCalculation;
using Aran.Omega.Strategy.UI;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using System;
using System.Collections.Generic;

namespace Aran.Omega.Models
{
    public class Transitional:SurfaceBase,IMultiplePlane
    {
        public Transitional()
        {
            SurfaceType = SurfaceType.Transitional;
        }

        public double Slope { get; set; }

        public List<int> SelectedPlanesHandle { get; private set; }

        public override IList<Info> PropertyList
        {
            get
            {
                return new List<Info> { new Info("Slope", Slope.ToString(), "%") };
            }
            set{throw new NotImplementedException();}
        }

        public override PointPenetrateModel GetManualReport(Aran.Geometries.Point obstaclePt)
        {
            foreach (var plane in Planes)
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
                    result.Surface = "Transitional";
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
