using Aran.Aim.Features;
using ESRI.ArcGIS.Geometry;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	static public class Converters
	{
		public static Aran.Geometries.Point ESRIPointToARANPoint(IPoint pPoint)
		{
			Aran.Geometries.Point result = new Aran.Geometries.Point();
			result.X = pPoint.X;
			result.Y = pPoint.Y;
			result.Z = pPoint.Z;
			result.M = pPoint.M;
			result.T = 0;

			return result;
		}

		public static Aran.Geometries.MultiPoint ESRIMultiPointToARANMultiPoint(IMultipoint pMultiPoint)
		{
			IPointCollection pPointCollection = pMultiPoint as IPointCollection;
			Aran.Geometries.MultiPoint result = new Aran.Geometries.MultiPoint();

			for (int i = 0; i < pPointCollection.PointCount; i++)
			{
				IPoint pPoint = pPointCollection.Point[i];
				Aran.Geometries.Point aPoint = new Aran.Geometries.Point();

				aPoint.X = pPoint.X;
				aPoint.Y = pPoint.Y;
				aPoint.Z = pPoint.Z;
				aPoint.M = pPoint.M;
				aPoint.T = 0;
				result.Add(aPoint);
			}

			return result;
		}

		public static void ESRIPointCollectionToARANPointCollection(IPointCollection pPointCollection, ref Aran.Geometries.LineString result)
		{
			for (int i = 0; i < pPointCollection.PointCount; i++)
			{
				IPoint pPoint = pPointCollection.Point[i];
				Aran.Geometries.Point aPoint = new Aran.Geometries.Point();

				aPoint.X = pPoint.X;
				aPoint.Y = pPoint.Y;
				aPoint.Z = pPoint.Z;
				aPoint.M = pPoint.M;
				aPoint.T = 0;
				result.Add(aPoint);
			}
		}

		public static void ESRIPointCollectionToARANPointCollection(IPointCollection pPointCollection, ref Aran.Geometries.Ring result)
		{
			for (int i = 0; i < pPointCollection.PointCount; i++)
			{
				IPoint pPoint = pPointCollection.Point[i];
				Aran.Geometries.Point aPoint = new Aran.Geometries.Point();

				aPoint.X = pPoint.X;
				aPoint.Y = pPoint.Y;
				aPoint.Z = pPoint.Z;
				aPoint.M = pPoint.M;
				aPoint.T = 0;
				result.Add(aPoint);
			}
		}


		public static Aran.Geometries.LineString ESRIPathToARANLineString(IPath pPath)
		{
			pPath.Generalize(1.0e-9);
			Aran.Geometries.LineString result = new Aran.Geometries.LineString();
			ESRIPointCollectionToARANPointCollection(pPath as IPointCollection, ref result);
			return result;
		}

		public static Aran.Geometries.LineString ESRILineToARANLineString(ILine pLine)
		{
			Aran.Geometries.LineString Result = new Aran.Geometries.LineString();
			Result.Add(ESRIPointToARANPoint(pLine.FromPoint));
			Result.Add(ESRIPointToARANPoint(pLine.ToPoint));
			return Result;
		}

		public static Aran.Geometries.Ring ESRIRingToARANRing(IRing pRing)
		{
			pRing.Generalize(1.0e-9);
			Aran.Geometries.Ring result = new Aran.Geometries.Ring();
			ESRIPointCollectionToARANPointCollection(pRing as IPointCollection, ref result);
			return result;
		}

		public static Aran.Geometries.MultiLineString ESRIPolylineToARANPolyline(IPolyline pPolyline)
		{
			IGeometryCollection pGeometryCollection = pPolyline as IGeometryCollection;
			Aran.Geometries.MultiLineString  result = new Aran.Geometries.MultiLineString();

			for (int i = 0; i < pGeometryCollection.GeometryCount; i++)
			{
				Aran.Geometries.LineString LineString = ESRIPathToARANLineString(pGeometryCollection.Geometry[i] as IPath);
				result.Add(LineString);
			}

			return result;
		}

		public static Surface ESRIPolygonToAIXMSurface(IPolygon pPolygon)
		{
			Surface pSurface = new Surface();

			IPolygon4 pPolygon4 = (IPolygon4)pPolygon;
			ESRI.ArcGIS.Geometry.IGeometryBag pExteriorRings = pPolygon4.ExteriorRingBag;
			ESRI.ArcGIS.Geometry.IEnumGeometry pExteriorRingsEnum = (ESRI.ArcGIS.Geometry.IEnumGeometry)pExteriorRings;
			pExteriorRingsEnum.Reset();
			IRing pRing = (ESRI.ArcGIS.Geometry.IRing)pExteriorRingsEnum.Next();

			while (pRing != null)
			{
				Aran.Geometries.Polygon aPolygon = new Aran.Geometries.Polygon();
				Aran.Geometries.Ring aRing = ESRIRingToARANRing(pRing);
				aPolygon.ExteriorRing = aRing;

				ESRI.ArcGIS.Geometry.IGeometryBag pInteriorRings = pPolygon4.InteriorRingBag[pRing];
				ESRI.ArcGIS.Geometry.IEnumGeometry pInteriorRingsEnum = (ESRI.ArcGIS.Geometry.IEnumGeometry)pInteriorRings;
				pInteriorRingsEnum.Reset();

				pRing = (ESRI.ArcGIS.Geometry.IRing)pInteriorRingsEnum.Next();
				while (pRing != null)
				{
					aRing = ESRIRingToARANRing(pRing);
					aPolygon.InteriorRingList.Add(aRing);
					pRing = (ESRI.ArcGIS.Geometry.IRing)pInteriorRingsEnum.Next();
				}

				pSurface.Geo.Add(aPolygon);
				pRing = (ESRI.ArcGIS.Geometry.IRing)pExteriorRingsEnum.Next();
			}

			return pSurface;
		}

		public static Aran.Geometries.MultiPolygon ESRIPolygonToARANPolygon(IPolygon pPolygon)
		{
			Aran.Geometries.MultiPolygon result = new Aran.Geometries.MultiPolygon();

			IPolygon4 pPolygon4 = pPolygon as IPolygon4;
			ESRI.ArcGIS.Geometry.IGeometryBag pExteriorRings = pPolygon4.ExteriorRingBag;

			ESRI.ArcGIS.Geometry.IEnumGeometry pExteriorRingsEnum = pExteriorRings as ESRI.ArcGIS.Geometry.IEnumGeometry;
			pExteriorRingsEnum.Reset();

			IRing pRing = pExteriorRingsEnum.Next() as ESRI.ArcGIS.Geometry.IRing;

			while (pRing != null)
			{
				Aran.Geometries.Polygon aPolygon = new Aran.Geometries.Polygon();

				Aran.Geometries.Ring aRing = ESRIRingToARANRing(pRing);
				aPolygon.ExteriorRing = aRing;

				ESRI.ArcGIS.Geometry.IGeometryBag pInteriorRings = pPolygon4.InteriorRingBag[pRing];
				ESRI.ArcGIS.Geometry.IEnumGeometry pInteriorRingsEnum = pInteriorRings as ESRI.ArcGIS.Geometry.IEnumGeometry;
				pInteriorRingsEnum.Reset();

				pRing = pInteriorRingsEnum.Next() as ESRI.ArcGIS.Geometry.IRing;
				while (pRing != null)
				{
					aRing = ESRIRingToARANRing(pRing);
					aPolygon.InteriorRingList.Add(aRing);
					pRing = pInteriorRingsEnum.Next() as ESRI.ArcGIS.Geometry.IRing;
				}

				result.Add(aPolygon);
				pRing = pExteriorRingsEnum.Next() as ESRI.ArcGIS.Geometry.IRing;
			}

			return result;
		}

		public static Aran.Geometries.Box ESRIEnvelopeToARANBox(IEnvelope pEnvelope)
		{
			Aran.Geometries.Box Result = new Aran.Geometries.Box();
			Result[0].X = pEnvelope.XMin;
			Result[0].Y = pEnvelope.YMin;
			Result[0].Z = pEnvelope.ZMin;
			Result[0].M = pEnvelope.MMin;
			Result[0].T = 0;

			Result[1].X = pEnvelope.XMax;
			Result[1].Y = pEnvelope.YMax;
			Result[1].Z = pEnvelope.ZMax;
			Result[1].M = pEnvelope.MMax;
			Result[1].T = 0;
			return Result;
		}

		public static Aran.Geometries.Geometry ESRIGeometryToARANGeometry(IGeometry pGeom)
		{
			switch (pGeom.GeometryType)
			{
				case esriGeometryType.esriGeometryPoint:
					return ESRIPointToARANPoint(pGeom as IPoint);
				case esriGeometryType.esriGeometryMultipoint:
					return ESRIMultiPointToARANMultiPoint(pGeom as IMultipoint);
				case esriGeometryType.esriGeometryPolyline:
					return ESRIPolylineToARANPolyline(pGeom as IPolyline);
				case esriGeometryType.esriGeometryPolygon:
					return ESRIPolygonToARANPolygon(pGeom as IPolygon);
				case esriGeometryType.esriGeometryEnvelope:
					return ESRIEnvelopeToARANBox(pGeom as IEnvelope);
				case esriGeometryType.esriGeometryPath:
					return ESRIPathToARANLineString(pGeom as IPath);
				//case esriGeometryType.esriGeometryAny:
				//case esriGeometryType.esriGeometryMultiPatch:
				case esriGeometryType.esriGeometryRing:
					return ESRIRingToARANRing(pGeom as IRing);
				case esriGeometryType.esriGeometryLine:
					return ESRILineToARANLineString(pGeom as ILine);
				//case esriGeometryType.esriGeometryCircularArc:
				//	return ESRIPathToGeomLineString(pGeom as ICircularArc)
				//case esriGeometryType.esriGeometryBezier3Curve:
				//				return ESRIPathToGeomLineString(pGeom as IBezier3Curve)
				//			case esriGeometryType.esriGeometryEllipticArc:
				//				return ESRIPathToGeomLineString(pGeom as IEllipticArc)
				//			case esriGeometryType.esriGeometryBag:
				//			case esriGeometryType.esriGeometryTriangleStrip:
				//			case esriGeometryType.esriGeometryTriangleFan:
				//			case esriGeometryType.esriGeometryRay:
				//			case esriGeometryType.esriGeometrySphere:

			}
			return null;
		}

		public static AixmPoint ESRIPointToAixmPoint(IPoint pPoint)
		{
			AixmPoint result = new AixmPoint();
			result.Geo.X = pPoint.X;
			result.Geo.Y = pPoint.Y;
			result.Geo.Z = pPoint.Z;
			result.Geo.M = pPoint.M;
			result.Geo.T = 0;
			return result;
		}

		public static IPoint AixmPointToESRIPoint(AixmPoint aPoint)
		{
			IPoint pPoint = new ESRI.ArcGIS.Geometry.Point();
			pPoint.X = aPoint.Geo.X;
			pPoint.Y = aPoint.Geo.Y;
			pPoint.Z = aPoint.Geo.Z;
			pPoint.M = aPoint.Geo.M;
			return pPoint;
		}

		public static IPoint ARANPointToESRIPoint(Aran.Geometries.Point aPoint)
		{
			IPoint pPoint = new ESRI.ArcGIS.Geometry.Point();
			pPoint.X = aPoint.X;
			pPoint.Y = aPoint.Y;
			pPoint.Z = aPoint.Z;
			pPoint.M = aPoint.M;

			return pPoint;
		}
	}
}
