using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using System.Runtime.InteropServices;
using Aran.PANDA.Common;
using Aran.Panda.Rnav.Holding.Helper;

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
          
            var circle = FeatureConvert.CreateCircle(new Aran.Geometries.Point(ptGeo.X,ptGeo.Y,ptGeo.Z), minAltitude,maxAltitude);
            if (circle.IsEmpty)
                return null;

            var circlePrj = GlobalParams.SpatialRefOperation.ToPrj(circle);

            //var navaids = GlobalParams.Database.HoldingQpi.GetNavaidListByTypes(circle, maxAltitude - minAltitude, CodeNavaidService.DME, CodeNavaidService.VOR_DME);

            Func<NavaidComponent,bool> checkDistance =(navaidComponent)=>
            {
                var navaidEquipment = GlobalParams.Database.HoldingQpi.GetAbstractFeature(navaidComponent.TheNavaidEquipment)
                     as NavaidEquipment;
                return Distance(navaidEquipment.Location.Geo,ptGeo)>minAltitude;
            };

            var navaids = GlobalParams.Database.HoldingQpi.GetNavaidListByTypes(circlePrj[0], GlobalParams.SpatialRefOperation, CodeNavaidService.DME, CodeNavaidService.VOR_DME);

            return (from navaid in navaids
                    from component in navaid.NavaidEquipment
                    where component.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.DME
                    && checkDistance(component)
                    && CheckPtInLIcenseArea(navaid.ComponentGeometry())
                    select navaid).ToList();			   			
		}

        public static List<Navaid> GetVorDmeList(Aran.Geometries.Point ptGeo,double maxDistance)
        {
            var circle = CreateCircle(GeomFunctions.AranToAimPoint(ptGeo), maxDistance);
            var circlePrj = GlobalParams.SpatialRefOperation.ToPrj(circle);

            var navaids = GlobalParams.Database.HoldingQpi.GetNavaidListByTypes(circlePrj[0], GlobalParams.SpatialRefOperation, CodeNavaidService.VOR_DME);

            navaids = navaids.Where(nav => CheckPtInLIcenseArea(GeomFunctions.Assign(nav.Location)))
                        .ToList();
          
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
            Aran.Geometries.Point featurePoint = GlobalParams.SpatialRefOperation.ToPrj(aimPt);
            if (featurePoint == null)
                return 0;
            return ARANFunctions.ReturnDistanceInMeters(featurePoint, GlobalParams.SpatialRefOperation.ToPrj(ptGeo));
        }

        public static Aran.Geometries.MultiPolygon CreateCircle(Aran.Geometries.Point ptCntGeo, double radius) 
        {
            double x, y;

            Aran.Geometries.Point pt = new Aran.Geometries.Point();
            Aran.Geometries.Ring ring = new Aran.Geometries.Ring();
            Aran.Geometries.Polygon circle = new Aran.Geometries.Polygon();

            for (int i = 0; i <= 359; i++)
            {
                PointAlongGeodesic_MathFunctions(ptCntGeo.X, ptCntGeo.Y, radius, i , out x, out y);
                ring.Add(new Aran.Geometries.Point(x,y));
            }
            circle.ExteriorRing = ring;
            return new Aran.Geometries.MultiPolygon { circle };
        }

        public static Aran.Geometries.MultiPolygon CreateCircle(Aran.Geometries.Point ptCntGeo, double minRadius, double maxRadius) 
        {
            double xMin, yMin, xMax, yMax;
            Aran.Geometries.Point ptMin = new Aran.Geometries.Point();
            Aran.Geometries.Point ptMax = new Aran.Geometries.Point();
            Aran.Geometries.Ring minRing = new Aran.Geometries.Ring();
            Aran.Geometries.Ring maxRing = new Aran.Geometries.Ring();
            Aran.Geometries.Polygon circle = new Aran.Geometries.Polygon();

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
               circle.InteriorRingList.Add(minRing);

            circle.ExteriorRing = maxRing;
            return new Aran.Geometries.MultiPolygon { circle };            

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
