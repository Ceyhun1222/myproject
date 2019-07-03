using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using Aran;

namespace Aran.Converters.ConverterJtsGeom
{
    public class ConvertFromJtsGeo
    {
        public static Aran.Geometries.Point ToPoint(IPoint myPnt)
        {
            if (myPnt == null)
                return null;

            Aran.Geometries.Point result = new Aran.Geometries.Point(myPnt.X, myPnt.Y, myPnt.Z);
            return result;
        }

        public static Aran.Geometries.LineString ToLinestring(ILineString lineString)
        {
            var result = new Aran.Geometries.LineString();
            for (int i = 0; i < lineString.NumPoints; i++)
            {
                IPoint esriPoint = lineString.GetPointN(i);
                Aran.Geometries.Point ourPoint = new Aran.Geometries.Point(esriPoint.X, esriPoint.Y, esriPoint.Z);
                result.Add(ourPoint);
            }
            return result;
        }

        public static Aran.Geometries.MultiPoint ToMultiPoint(MultiPoint lineString)
        {
            var result = new Aran.Geometries.MultiPoint();
            for (int i = 0; i < lineString.NumPoints; i++)
            {
                var esriPoint = lineString.Coordinates[i];
                var ourPoint = new Aran.Geometries.Point(esriPoint.X, esriPoint.Y, esriPoint.Z);
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
                Aran.Geometries.Point ourPoint = new Aran.Geometries.Point(esriPoint.X, esriPoint.Y, esriPoint.Z);
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
                case OgcGeometryType.LineString:
                    return ToLinestring(pGeom as ILineString);
                case OgcGeometryType.Polygon:
                    return ToPolygon(pGeom as IPolygon);
                case OgcGeometryType.MultiLineString:
                    return ToPolyline(pGeom as IMultiLineString);
                case OgcGeometryType.MultiPolygon:
                    return ToMultiPolygon(pGeom as IMultiPolygon);
                    case OgcGeometryType.MultiPoint:
                    return ToMultiPoint(pGeom as MultiPoint);
                default:
                    return null;
            }
        }

        private static Aran.Geometries.Polygon ToPolygon(IPolygon pPolygon)
        {
            var result = new Aran.Geometries.Polygon();
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
