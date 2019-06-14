using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Converters.ConvertESRIvvsJTS
{
	public class FromJTSToESRI
	{
		public static ESRI.ArcGIS.Geometry.IGeometry ToGeometry(GeoAPI.Geometries.IGeometry pGeom)
		{
			switch (pGeom.OgcGeometryType)
			{
				case GeoAPI.Geometries.OgcGeometryType.Point:
					return ToPoint((GeoAPI.Geometries.IPoint)pGeom);
				//case OgcGeometryType.LineString:
				//    return ToLinestring(pGeom as ILineString);
				//case OgcGeometryType.Polygon:
				//    return ToPolygon(pGeom as IPolygon);
				//case OgcGeometryType.MultiLineString:
				//    return ToPolyline(pGeom as IMultiLineString);
				//case OgcGeometryType.MultiPolygon:
				//    return ToMultiPolygon(pGeom as IMultiPolygon);
				default:
					return null;
			}
		}

		public static ESRI.ArcGIS.Geometry.IPoint ToPoint(GeoAPI.Geometries.IPoint myPnt)
		{
			if (myPnt == null)
				return null;

			ESRI.ArcGIS.Geometry.IPoint result = (ESRI.ArcGIS.Geometry.Point)new ESRI.ArcGIS.Geometry.Point();
			result.PutCoords(myPnt.X, myPnt.Y);
			result.Z = myPnt.Z;
			result.M = myPnt.M;

			return result;
		}

		//public static Aran.Geometries.LineString ToLinestring(ILineString lineString)
		//{
		//    var result = new Aran.Geometries.LineString();
		//    for (int i = 0; i < lineString.NumPoints; i++)
		//    {
		//        IPoint esriPoint = lineString.GetPointN(i);
		//        Aran.Geometries.Point ourPoint = new Aran.Geometries.Point(esriPoint.X, esriPoint.Y, esriPoint.Z);
		//        result.Add(ourPoint);
		//    }
		//    return result;
		//}

		//public static Aran.Geometries.Ring ToRing(ILinearRing pRing)
		//{
		//    Aran.Geometries.Ring result = new Aran.Geometries.Ring();
		//    if (pRing.IsEmpty)
		//        return result;
		//    for (int i = 0; i < pRing.NumPoints; i++)
		//    {
		//        IPoint esriPoint = pRing.GetPointN(i);
		//        Aran.Geometries.Point ourPoint = new Aran.Geometries.Point(esriPoint.X, esriPoint.Y, esriPoint.Z);
		//        result.Add(ourPoint);
		//    }
		//    return result;
		//}

		//public static Aran.Geometries.MultiLineString ToPolyline(IMultiLineString pPolyline)
		//{
		//    Aran.Geometries.MultiLineString result = new Aran.Geometries.MultiLineString();
		//    if (pPolyline.IsEmpty)
		//        return result;

		//    for (int J = 0; J < pPolyline.Count; J++)
		//    {
		//        Aran.Geometries.LineString lineString = ToLinestring((ILineString)pPolyline[J]);
		//        result.Add(lineString);
		//    }
		//    return result;
		//}

		//private static Aran.Geometries.Polygon ToPolygon(IPolygon pPolygon)
		//{
		//    Aran.Geometries.Polygon result = new Aran.Geometries.Polygon();
		//    if (pPolygon == null || pPolygon.IsEmpty)
		//        return result;

		//		if (pPolygon.ExteriorRing == null || pPolygon.ExteriorRing.IsEmpty)
		//			return result;

		//    result.ExteriorRing = ToRing((ILinearRing)pPolygon.ExteriorRing);
		//    for (int i = 0; i < pPolygon.InteriorRings.Length; i++)
		//        result.InteriorRingList.Add(ToRing((ILinearRing)pPolygon.GetInteriorRingN(i)));

		//    return result;
		//}

		//private static Aran.Geometries.MultiPolygon ToMultiPolygon(IMultiPolygon pPolygon)
		//{
		//    Aran.Geometries.MultiPolygon result = new Aran.Geometries.MultiPolygon();
		//    if (pPolygon.IsEmpty)
		//        return result;

		//    for (int J = 0; J < pPolygon.Count; J++)
		//    {
		//        Aran.Geometries.Polygon poly = ToPolygon((IPolygon)pPolygon[J]);
		//        result.Add(poly);
		//    }
		//    return result;
		//}
	}
}
