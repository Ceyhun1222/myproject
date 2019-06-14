using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceLoader
{
    public static class GeometryConvert
    {
        public static Aran.Geometries.Point ToPoint (ARAN.GeometryClasses.Point pt)
        {
            var newPoint = new Aran.Geometries.Point ();
            newPoint.SetCoords (pt.X, pt.Y);
            
            if (!double.IsNaN (pt.Z))
                newPoint.Z = pt.Z;

            if (!double.IsNaN (pt.M))
                newPoint.M = pt.M;

            return newPoint;
        }

        public static Aran.Geometries.MultiPolygon ToPolygon (ARAN.GeometryClasses.Polygon srPolygon)
        {
            var aimMPolygon = new Aran.Geometries.MultiPolygon ();

            foreach (ARAN.GeometryClasses.Ring ring in srPolygon)
            {
                var aimPolygon = new Aran.Geometries.Polygon ();
                aimPolygon.ExteriorRing = ToRing (ring);
                aimMPolygon.Add (aimPolygon);
            }

            return aimMPolygon;
        }

        public static Aran.Geometries.MultiLineString ToMulitLineString (ARAN.GeometryClasses.PolyLine srPolyLine)
        {
            var aimMLS = new Aran.Geometries.MultiLineString ();

            foreach (ARAN.GeometryClasses.Part part in srPolyLine)
            {
                var aimLS = new Aran.Geometries.LineString ();
                FillMultiPoint (aimLS, part);
                aimMLS.Add (aimLS);
            }

            return aimMLS;
        }

        private static Aran.Geometries.Ring ToRing (ARAN.GeometryClasses.Ring ring)
        {
            var aimRing = new Aran.Geometries.Ring ();
            FillMultiPoint (aimRing, ring);
            return aimRing;
        }

        private static void FillMultiPoint (Aran.Geometries.MultiPoint aimMP, ARAN.GeometryClasses.MultiPoint mp)
        {
            foreach (ARAN.GeometryClasses.Point pt in mp)
            {
                aimMP.Add (ToPoint (pt));
            }
        }
    }
}
