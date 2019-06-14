using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace Aran.Omega.Topology
{
    public static class ConvertToAranGeo
    {
        public static Aran.Geometries.Point ToPoint(IPoint myPnt)
        {
            if (myPnt == null)
                return null;

            Aran.Geometries.Point result = new Geometries.Point(myPnt.X,myPnt.Y,myPnt.Z);
            return result;
        }

        public static Aran.Geometries.LineString ToLinestring(ILineString lineString)
        {
            var result = new Aran.Geometries.LineString();
            for (int i = 0; i < lineString.NumPoints; i++)
            {
                IPoint esriPoint = lineString.GetPointN(i);
                Aran.Geometries.Point ourPoint = new Geometries.Point(esriPoint.X,esriPoint.Y,esriPoint.Z);
                result.Add(ourPoint);
            }
            return result;
        }

        public static Aran.Geometries.Ring ToRing(ILinearRing pRing)
        {
            Aran.Geometries.Ring result = new Aran.Geometries.Ring();
            if (pRing.IsEmpty)
                return result;
            for (int i = 0; i < pRing.NumPoints; i++)
            {
                IPoint esriPoint = pRing.GetPointN(i);
                Aran.Geometries.Point ourPoint = new Geometries.Point(esriPoint.X,esriPoint.Y,esriPoint.Z);
                result.Add(ourPoint);
            }
            return result;
        }

        public static Aran.Geometries.MultiLineString ToPolyline(IMultiLineString pPolyline)
        {
            Aran.Geometries.MultiLineString result = new Aran.Geometries.MultiLineString();
            if (pPolyline.IsEmpty)
                return result;

            for (int J = 0; J < pPolyline.Count; J++)
            {
                Aran.Geometries.LineString lineString = ToLinestring((ILineString)pPolyline[J]);
                result.Add(lineString);
            }
            return result;
        }

        public static Aran.Geometries.Geometry ToGeometry(IGeometry pGeom)
        {
            switch (pGeom.OgcGeometryType)
            {
                case OgcGeometryType.Point:
                    return ToPoint(pGeom as IPoint);
                    break;
                case OgcGeometryType.LineString:
                    return ToLinestring(pGeom as ILineString);
                    break;
                case OgcGeometryType.Polygon:
                    return ToPolygon(pGeom as IPolygon);
                    break;
                case OgcGeometryType.MultiLineString:
                    return ToPolyline(pGeom as IMultiLineString);
                    break;
                case OgcGeometryType.MultiPolygon:
                    return ToMultiPolygon(pGeom as IMultiPolygon);
                    break;
                default:
                    return null;
                    break;
            }
        }

        private static Aran.Geometries.Polygon ToPolygon(IPolygon pPolygon)
        {
            Aran.Geometries.Polygon result = new Geometries.Polygon();
            if (pPolygon == null || pPolygon.IsEmpty)
            {
                return result;
            }
            else
                if (pPolygon.ExteriorRing == null || pPolygon.ExteriorRing.IsEmpty)
                    return result;

            result.ExteriorRing = ToRing((ILinearRing)pPolygon.ExteriorRing);
            for (int i = 0; i < pPolygon.InteriorRings.Length; i++)
            {
                result.InteriorRingList.Add(ToRing((ILinearRing)pPolygon.GetInteriorRingN(i)));
            }
            return result;
        }
       
        private static Aran.Geometries.MultiPolygon ToMultiPolygon(IMultiPolygon pPolygon)
        {
            Aran.Geometries.MultiPolygon result = new Aran.Geometries.MultiPolygon();
            if (pPolygon.IsEmpty)
                return result;

            for (int J = 0; J < pPolygon.Count; J++)
            {
                Aran.Geometries.Polygon poly = ToPolygon((IPolygon)pPolygon[J]);
                result.Add(poly);
            }
            return result;
        }
    }
}
