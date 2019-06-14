using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Omega.Enums;
using Aran.Omega.Strategy.ObstacleCalculation;
using Aran.Omega.Strategy.UI;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Geometry;
using Polygon = Aran.Geometries.Polygon;

namespace Aran.Omega.Models
{
    public class InnerTransitional : SurfaceBase,IMultiplePlane
    {
        public InnerTransitional()
        {
            Planes = new List<Plane>();
            SurfaceType = Aran.PANDA.Constants.SurfaceType.InnerTransitional;
        }

        public double Slope { get; set; }

        public override IList<Info> PropertyList
        {
            get => new List<Info> { new Info("Slope", Slope.ToString(CultureInfo.InvariantCulture), "%") };
            set
            {
                throw new NotImplementedException();
            }
        }

        public override PointPenetrateModel GetManualReport(Aran.Geometries.Point obstaclePt)
        {
            foreach (Plane plane in Planes)
            {
                if (plane.Geo.IsPointInside(obstaclePt))
                {
                    //Get two point from strip which can create plane
                    //Then calculate  x1 and x2 distance from start point.
                    var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI,
                        obstaclePt);
                    var obstacleElev =obstaclePt.Z;
                    var surfaceElevation = plane.Param.GetZ(localObstaclePt);

                    var result = new PointPenetrateModel();
                    result.Surface = "Inner Transitional";
                    result.Elevation =Common.ConvertHeight(obstaclePt.Z,RoundType.ToNearest);
                    result.Plane = plane.Param.CreateEquationStr(surfaceElevation);
                    result.Penetration = Common.ConvertHeight(obstacleElev - surfaceElevation,RoundType.ToNearest);
                    return result;
                }
            }
            return null;
        }
    }
}
