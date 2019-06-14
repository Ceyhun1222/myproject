using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using System.Runtime.InteropServices;
using Aran.PANDA.Common;

namespace Holding
{
	public static class FeatureConvert
	{
        [DllImport("MathFunctions.dll", SetLastError = true, EntryPoint = "_PointAlongGeodesic@40", CallingConvention = CallingConvention.StdCall)]
        static extern long PointAlongGeodesic_MathFunctions(double X, double Y, double Dist, double Azimuth, out double resx, out double resy);

        [DllImport("MathFunctions.dll", EntryPoint = "_ReturnGeodesicDistance@32")]
        public static extern double GetGeodesicDistance(double X0, double y0, double X1, double Y1);

		public static List<Navaid> GetDmeList(Aran.Geometries.Point ptGeo, double maxAltitude, double minAltitude)
		{
            if (minAltitude > maxAltitude)
                return null;
          
            Aran.Geometries.Polygon circle = FeatureConvert.CreateCircle(new Aran.Geometries.Point(ptGeo.X,ptGeo.Y,ptGeo.Z), minAltitude,maxAltitude);

            //var navaids = GlobalParams.Database.HoldingQpi.GetNavaidListByTypes(circle, maxAltitude - minAltitude, CodeNavaidService.DME, CodeNavaidService.VOR_DME);

            var navaids = GlobalParams.Database.HoldingQpi.GetNavaidListByTypes(circle, GlobalParams.SpatialRefOperation, CodeNavaidService.DME, CodeNavaidService.VOR_DME);
            return (from navaid in navaids
                    from component in navaid.NavaidEquipment
                    where component.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.DME && Distance(navaid.Location.Geo, ptGeo) > minAltitude
                    &&  FeatureConvert.CheckPtInLIcenseArea(GeomFunctions.AimToAranPoint(navaid.Location.Geo))
                    select navaid).ToList<Navaid>();			   			
		}

        public static List<Navaid> GetVorDmeList(Aran.Geometries.Point ptGeo,double maxDistance)
        {
            Aran.Geometries.Polygon circle = FeatureConvert.CreateCircle(GeomFunctions.AranToAimPoint(ptGeo), maxDistance);
            //var navaids = GlobalParams.Database.HoldingQpi.GetNavaidListByTypes(GeomFunctions.AranToAimPoint(ptGeo), maxDistance, CodeNavaidService.VOR_DME);
            var navaids = GlobalParams.Database.HoldingQpi.GetNavaidListByTypes(circle, GlobalParams.SpatialRefOperation, CodeNavaidService.VOR_DME);
            navaids = navaids.Where(nav => FeatureConvert.CheckPtInLIcenseArea(GeomFunctions.Assign(nav.Location)))
                        .ToList<Navaid>();
          
            Boolean flag = false;
            List<Navaid> navList = new List<Navaid>();
            
            foreach (Navaid navaid in navaids)
            {
                double distance = Distance(navaid.Location.Geo, ptGeo);
                foreach (NavaidComponent comp in navaid.NavaidEquipment)
                {
                    flag = false;
                    if (comp.TheNavaidEquipment.Type== Aran.Aim.NavaidEquipmentType.VOR)
                    {
                        VOR vor = GlobalParams.Database.HoldingQpi.GetVor(comp.TheNavaidEquipment.Identifier);

                        if (vor.Type ==CodeVOR.VOR)
                        {
                            if (distance > 60 * 1852)
                                break;
                        }
                        else if (vor.Type == CodeVOR.DVOR)
                        {
                            if (distance > 75 * 1852)
                                break;
                        }

                    }
                    else if (comp.TheNavaidEquipment.Type== Aran.Aim.NavaidEquipmentType.DME)
                    {
                        if (distance < 3 * 1852)
                            break;
                    }
                    flag = true;
                }
                if (flag)
                    navList.Add(navaid);
            }

            return navList;
        }

        public static double Distance(Aran.Geometries.Point aimPt, Aran.Geometries.Point ptGeo)
        {
            Aran.Geometries.Point featurePoint = GlobalParams.SpatialRefOperation.ToPrj(GeomFunctions.AimToAranPoint(aimPt));
            if (featurePoint == null)
                return 0;
            return ARANFunctions.ReturnDistanceInMeters(featurePoint, GlobalParams.SpatialRefOperation.ToPrj(ptGeo));
        }

        public static Aran.Geometries.Polygon CreateCircle(Aran.Geometries.Point ptCntGeo, double radius) 
        {
            //double iInRad;
            double x, y;

            //double angleStep = 1;
            Aran.Geometries.Point pt = new Aran.Geometries.Point();
            Aran.Geometries.Ring ring = new Aran.Geometries.Ring();
            Aran.Geometries.Polygon result = new Aran.Geometries.Polygon();

            for (int i = 0; i <= 359; i++)
            {
                PointAlongGeodesic_MathFunctions(ptCntGeo.X, ptCntGeo.Y, radius, i , out x, out y);
                ring.Add(new Aran.Geometries.Point(x,y));
            }
            result.ExteriorRing = ring;
            return result;
        }

        public static Aran.Geometries.Polygon CreateCircle(Aran.Geometries.Point ptCntGeo, double minRadius, double maxRadius) 
        {
            double xMin, yMin, xMax, yMax;
            Aran.Geometries.Point ptMin = new Aran.Geometries.Point();
            Aran.Geometries.Point ptMax = new Aran.Geometries.Point();
            Aran.Geometries.Ring minRing = new Aran.Geometries.Ring();
            Aran.Geometries.Ring maxRing = new Aran.Geometries.Ring();
            Aran.Geometries.Polygon result = new Aran.Geometries.Polygon();

            for (int i = 0; i <= 359; i++)
            {
                
                PointAlongGeodesic_MathFunctions(ptCntGeo.X, ptCntGeo.Y, minRadius, i, out xMin, out yMin);
                PointAlongGeodesic_MathFunctions(ptCntGeo.X, ptCntGeo.Y, maxRadius, i, out xMax, out yMax);
                ptMin = new Aran.Geometries.Point(xMin,yMin);
                ptMax = new Aran.Geometries.Point(xMax, yMax);
                if (minRadius > 0.1)
                    minRing.Add(ptMin);
                maxRing.Add(ptMax);
            }

            if (minRadius > 0.1)
               result.InteriorRingList.Add(minRing);
            
            result.ExteriorRing = maxRing;
            return result;            

        }

        public static Aran.Geometries.Polygon CreateExtent(double minX, double minY, double maxX, double maxY) 
        {
            Aran.Geometries.Polygon result = new Aran.Geometries.Polygon();
            Aran.Geometries.Ring ring = new Aran.Geometries.Ring();
            ring.Add(new Aran.Geometries.Point(minX, minY));
            ring.Add(new Aran.Geometries.Point(minX, maxY));
            ring.Add(new Aran.Geometries.Point(maxX, maxY));
            ring.Add(new Aran.Geometries.Point(maxX, minY));
            result.ExteriorRing = ring;
            return result;
            
        }

        public static bool CheckPtInLIcenseArea(Aran.Geometries.Point ptGeo)
        {
            return true;
            //InitHolding.LicenseRectGeo.IsPointInside(GlobalParams.SpatialRefOperation.ToGeo(ptGeo));
        }

        public static Aran.Geometries.MultiPoint ToMultiPoint(this IEnumerable<Aran.Geometries.Point> pointList)
        {
            var mlt = new Aran.Geometries.MultiPoint();
            foreach (var point in pointList)
                mlt.Add(point);
            return mlt;
        }
	}
}
