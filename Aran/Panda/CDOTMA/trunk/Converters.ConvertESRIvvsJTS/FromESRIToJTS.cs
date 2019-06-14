using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Converters.ConvertESRIvvsJTS
{
	public class FromESRIToJTS
	{
		public static NetTopologySuite.Geometries.Geometry FromGeometry(ESRI.ArcGIS.Geometry.IGeometry esriGeom)
		{
			//if (esriGeom == null)
			//    return null;

			switch (esriGeom.GeometryType)
			{
				case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
					return FromPoint((ESRI.ArcGIS.Geometry.IPoint)esriGeom);

				case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultipoint:
					return FromMultiPoint((ESRI.ArcGIS.Geometry.IPointCollection)esriGeom);

				case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine:
					return FromLine((ESRI.ArcGIS.Geometry.ILine)esriGeom);

				case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
					return FromPolyline((ESRI.ArcGIS.Geometry.IPolyline)esriGeom);

				case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryRing:
					return FromRing((ESRI.ArcGIS.Geometry.IRing)esriGeom);

				case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
					return FromPolygon((ESRI.ArcGIS.Geometry.IPolygon)esriGeom);
			}

			throw new Exception(string.Format("Geometry type {0} is not implemented (in FromESRIToJTS)!", esriGeom.GeometryType));
		}

		public static NetTopologySuite.Geometries.Point FromPoint(ESRI.ArcGIS.Geometry.IPoint esriPnt)
		{
			if (esriPnt == null)
				return null;

			NetTopologySuite.Geometries.Point result = new NetTopologySuite.Geometries.Point(esriPnt.X, esriPnt.Y, esriPnt.Z);
			result.M = esriPnt.M;
			return result;
		}

		public static NetTopologySuite.Geometries.MultiPoint FromMultiPoint(ESRI.ArcGIS.Geometry.IPointCollection esriMultiPoint)
		{
			if (esriMultiPoint == null)
				return null;

			GeoAPI.Geometries.IPoint[] pts = new NetTopologySuite.Geometries.Point[esriMultiPoint.PointCount];
			for (int i = 0; i < esriMultiPoint.PointCount; i++)
				pts[i] = FromPoint(esriMultiPoint.Point[i]);

			NetTopologySuite.Geometries.MultiPoint result = new NetTopologySuite.Geometries.MultiPoint(pts);
			pts = null;
			return result;
		}

		public static NetTopologySuite.Geometries.LineString FromLine(ESRI.ArcGIS.Geometry.ILine esriLine)
		{
			if (esriLine == null)
				return null;

			GeoAPI.Geometries.Coordinate[] coords = new GeoAPI.Geometries.Coordinate[2];
			coords[0] = new GeoAPI.Geometries.Coordinate(esriLine.FromPoint.X, esriLine.FromPoint.Y);
			coords[1] = new GeoAPI.Geometries.Coordinate(esriLine.ToPoint.X, esriLine.ToPoint.Y);

			NetTopologySuite.Geometries.LineString result = new NetTopologySuite.Geometries.LineString(coords);

			coords = null;
			return result;
		}

		public static NetTopologySuite.Geometries.LineString FromPolylinePart(ESRI.ArcGIS.Geometry.IPolyline esriPolyline)
		{
			if (esriPolyline == null)
				return null;

			ESRI.ArcGIS.Geometry.IPointCollection points = (ESRI.ArcGIS.Geometry.IPointCollection)esriPolyline;

			int n = points.PointCount;
			GeoAPI.Geometries.Coordinate[] coords = new GeoAPI.Geometries.Coordinate[n];

			for (int i = 0; i < n; i++)
			{
				if (points.Point[i].IsEmpty)
				{
					Array.Resize<GeoAPI.Geometries.Coordinate>(ref coords, i);
					break;
				}
				coords[i] = new GeoAPI.Geometries.Coordinate(points.Point[i].X, points.Point[i].Y);
			}

			NetTopologySuite.Geometries.LineString result = new NetTopologySuite.Geometries.LineString(coords);
			coords = null;
			return result;
		}

		public static NetTopologySuite.Geometries.LineString FromPolylinePart(ESRI.ArcGIS.Geometry.IPointCollection points)
		{
			if (points == null)
				return null;

			GeoAPI.Geometries.Coordinate[] coords = new GeoAPI.Geometries.Coordinate[points.PointCount];

			for (int i = 0; i < points.PointCount; i++)
			{
				if (points.Point[i].IsEmpty)
				{
					Array.Resize<GeoAPI.Geometries.Coordinate>(ref coords, i);
					break;
				}
				coords[i] = new GeoAPI.Geometries.Coordinate(points.Point[i].X, points.Point[i].Y);
			}

			NetTopologySuite.Geometries.LineString result = new NetTopologySuite.Geometries.LineString(coords);
			coords = null;

			return result;
		}


		public static NetTopologySuite.Geometries.MultiLineString FromPolyline(ESRI.ArcGIS.Geometry.IPolyline esriPolyline)
		{
			if (esriPolyline == null)
				return null;

			ESRI.ArcGIS.Geometry.IGeometryCollection ceomcol = (ESRI.ArcGIS.Geometry.IGeometryCollection)esriPolyline;

			NetTopologySuite.Geometries.LineString[] LineStrings = new NetTopologySuite.Geometries.LineString[ceomcol.GeometryCount];
			if (ceomcol.GeometryCount == 1)
			{
				LineStrings[0] = (NetTopologySuite.Geometries.LineString)FromPolylinePart(esriPolyline);
			}
			else
				for (int i = 0; i < ceomcol.GeometryCount; i++)
				{
					if (ceomcol.Geometry[i].GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline)
						LineStrings[i] = (NetTopologySuite.Geometries.LineString)FromPolylinePart((ESRI.ArcGIS.Geometry.IPolyline)(ceomcol.Geometry[i]));
					if (ceomcol.Geometry[i].GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPath)
						LineStrings[i] = (NetTopologySuite.Geometries.LineString)FromPolylinePart((ESRI.ArcGIS.Geometry.IPointCollection)(ceomcol.Geometry[i]));
				}

			NetTopologySuite.Geometries.MultiLineString result = new NetTopologySuite.Geometries.MultiLineString(LineStrings);
			LineStrings = null;
			return result;
		}

		public static NetTopologySuite.Geometries.LinearRing FromRing(ESRI.ArcGIS.Geometry.IRing esriRing)
		{
			if (esriRing == null)
				return null;

			ESRI.ArcGIS.Geometry.IPointCollection points = (ESRI.ArcGIS.Geometry.IPointCollection)esriRing;

			if (points.PointCount > 0)
			{
				double dx = points.Point[0].X - points.Point[points.PointCount - 1].X;
				double dy = points.Point[0].Y - points.Point[points.PointCount - 1].Y;

				double dist = dx * dx + dy * dy;
				if (dist > 0.0000000001)
					points.AddPoint(points.Point[0]);
			}

			GeoAPI.Geometries.Coordinate[] coords = new GeoAPI.Geometries.Coordinate[points.PointCount];

			for (int i = 0; i < points.PointCount; i++)
				coords[i] = new GeoAPI.Geometries.Coordinate(points.Point[i].X, points.Point[i].Y);

			NetTopologySuite.Geometries.LinearRing result = new NetTopologySuite.Geometries.LinearRing(coords);

			coords = null;
			return result;
		}

		public static NetTopologySuite.Geometries.MultiPolygon FromPolygon(ESRI.ArcGIS.Geometry.IPolygon esriGeom)
		{
			ESRI.ArcGIS.Geometry.IPolygon4 pPolygon = (ESRI.ArcGIS.Geometry.IPolygon4)esriGeom;
			ESRI.ArcGIS.Geometry.IGeometryBag pExteriorRings = pPolygon.ExteriorRingBag;
			ESRI.ArcGIS.Geometry.IEnumGeometry exteriorRingsEnum = ((ESRI.ArcGIS.Geometry.IEnumGeometry)(pExteriorRings));

			GeoAPI.Geometries.IPolygon[] polygons = new NetTopologySuite.Geometries.Polygon[exteriorRingsEnum.Count];

			exteriorRingsEnum.Reset();
			ESRI.ArcGIS.Geometry.IRing extRing = ((ESRI.ArcGIS.Geometry.IRing)(exteriorRingsEnum.Next()));

			int i = 0;
			while (extRing != null)
			{
				GeoAPI.Geometries.ILinearRing shell = FromRing(extRing);
				GeoAPI.Geometries.IPolygon pPgon;

				ESRI.ArcGIS.Geometry.IGeometryBag pInteriorRings = pPolygon.InteriorRingBag[extRing];
				ESRI.ArcGIS.Geometry.IEnumGeometry interiorRingsEnum = ((ESRI.ArcGIS.Geometry.IEnumGeometry)(pInteriorRings));

				if (interiorRingsEnum.Count == 0)
					pPgon = new NetTopologySuite.Geometries.Polygon(shell);
				else
				{
					GeoAPI.Geometries.ILinearRing[] holes = new GeoAPI.Geometries.ILinearRing[interiorRingsEnum.Count];
					ESRI.ArcGIS.Geometry.IRing intRing = ((ESRI.ArcGIS.Geometry.IRing)(interiorRingsEnum.Next()));

					int j = 0;
					while (intRing != null)
					{
						holes[j] = FromRing(intRing);
						j++;
						intRing = ((ESRI.ArcGIS.Geometry.IRing)(interiorRingsEnum.Next()));
					}

					pPgon = new NetTopologySuite.Geometries.Polygon(shell, holes);
				}

				polygons[i] = pPgon;
				i++;
				extRing = ((ESRI.ArcGIS.Geometry.IRing)(exteriorRingsEnum.Next()));
			}

			NetTopologySuite.Geometries.MultiPolygon result = new NetTopologySuite.Geometries.MultiPolygon(polygons);
			polygons = null;
			return result;
		}

		public static NetTopologySuite.Geometries.Point AIXMCoordsToPoint(string X, string Y)
		{
			try
			{
				int len = X.Length;
				string sDeg = X.Substring(0, 3);
				string sMin = X.Substring(3, 2);
				string sSec = X.Substring(5, len - 6);
				string sSgn = X.Substring(len - 1);
				double xc = double.Parse(sDeg) + double.Parse(sMin) / 60.0 + double.Parse(sSec) / 3600.0;

				if (sSgn == "W")
					xc = 360.0 - xc;
				else if (sSgn != "E")
					throw new Exception("Invalid coordinat specification");

				len = Y.Length;
				sDeg = Y.Substring(0, 2);
				sMin = Y.Substring(2, 2);
				sSec = Y.Substring(4, len - 5);
				sSgn = Y.Substring(len - 1);
				double yc = double.Parse(sDeg) + double.Parse(sMin) / 60.0 + double.Parse(sSec) / 3600.0;

				if (sSgn == "S")
					yc = -yc;
				else if (sSgn != "N")
					throw new Exception("Invalid coordinat specification");

				NetTopologySuite.Geometries.Point Result = new NetTopologySuite.Geometries.Point(xc, yc);
				return Result;
			}
			catch
			{
				return null;
			}
		}

		public static void Initialize()
		{
			//приходится явно указывать ArcView лицензию
			//m_pAoInitialize = new AoInitialize();
			//m_pAoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeArcView);
			ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);
		}
	}
}
