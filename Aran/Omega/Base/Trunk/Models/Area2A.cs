using Aran.Omega.Enums;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Aran.Omega.Models
{
    public class Area2A:SurfaceBase,IMultiplePlane
    {
        public const double VerticalBufferWidth = 3;
        public const double HorizontalBufferWidth = 5;
        private const double Area2AHeight = 3;

        public Area2A()
        {
            Planes = new List<Plane>();
            EtodSurfaceType = EtodSurfaceType=EtodSurfaceType.Area2A;
            SurfaceType = SurfaceType.Area2A;

            Height = Area2AHeight;
        }

        public double Height { get; set; }

        public double LengthOfInnerEdge { get; set; }

        public override IList<Info> PropertyList
        {
            get
            {
                _propertyList.Clear();
                _propertyList.Add(new Info("Length of inner edge", Common.ConvertDistance(LengthOfInnerEdge, RoundType.ToNearest).ToString(CultureInfo.InvariantCulture), InitOmega.DistanceConverter.Unit));
                _propertyList.Add(new Info("Height", Common.ConvertHeight(Height, RoundType.ToNearest).ToString(CultureInfo.InvariantCulture), InitOmega.HeightConverter.Unit));
                return _propertyList;
            }
            set => _propertyList = value;
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
                    var obstacleElev = obstaclePt.Z;

                    var surfaceElevation = plane.Param.GetZ(localObstaclePt);

                    var result = new PointPenetrateModel();
                    result.Surface = "Area 2A";
                    result.Elevation = Common.ConvertHeight(obstaclePt.Z, RoundType.ToNearest);
                    result.Plane = plane.Param.CreateEquationStr(surfaceElevation);
                    result.Penetration = Common.ConvertHeight(obstacleElev - surfaceElevation, RoundType.ToNearest);
                    return result;
                }
            }
            return null;
        }
    }
}
