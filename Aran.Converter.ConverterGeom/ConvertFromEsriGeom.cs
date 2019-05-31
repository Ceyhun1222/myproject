using System.Diagnostics;
using ESRI.ArcGIS.Geometry;
using Aran.Geometries;
using System;

namespace Aran.Converters
{
	public static class ConvertFromEsriGeom
	{ 
		public static Aran.Geometries.Point ToPoint(ESRI.ArcGIS.Geometry.IPoint esriPnt)
		{
			if (esriPnt == null)
				return null;

			Aran.Geometries.Point result = new Geometries.Point();
			if (esriPnt.IsEmpty)
				return result;

			result.X = esriPnt.X;
			result.Y = esriPnt.Y;

			result.Z = esriPnt.Z;
			result.M = esriPnt.M;
			result.T = 0;
			return result;
		}

		public static Aran.Geometries.MultiPoint ToMultiPoint(ESRI.ArcGIS.Geometry.IMultipoint esriMltPnt)
		{
			var result = new MultiPoint();

			if (esriMltPnt.IsEmpty)
				return result;

			IPointCollection esriPointCollection = esriMltPnt as IPointCollection;

			for (int i = 0; i < esriPointCollection.PointCount; i++)
			{
				IPoint esriPnt = esriPointCollection.get_Point(i);
				Aran.Geometries.Point ourPoint = new Geometries.Point();

				ourPoint.X = esriPnt.X;
				ourPoint.Y = esriPnt.Y;
				ourPoint.Z = esriPnt.Z;
				ourPoint.M = esriPnt.M;
				ourPoint.T = 0;
				result.Add(ourPoint);
			}
			return result;
		}

		public static void ToLinestring(IPointCollection pPointCollection, ref Aran.Geometries.LineString result)
		{
			for (int i = 0; i < pPointCollection.PointCount; i++)
			{
				IPoint esriPoint = pPointCollection.get_Point(i);
				Aran.Geometries.Point ourPoint = new Geometries.Point();

				ourPoint.X = esriPoint.X;
				ourPoint.Y = esriPoint.Y;
				ourPoint.Z = esriPoint.Z;
				ourPoint.M = esriPoint.M;
				ourPoint.T = 0;
				result.Add(ourPoint);
			}
		}

		public static Aran.Geometries.LineString ToLineString(IPath pPath, bool isGeo)
		{
			LineString result = new LineString();
			if (pPath.IsEmpty)
				return result;
			if (isGeo)
				pPath.Generalize(0.0000001);
			else
				pPath.Generalize(0.1);
			ToLinestring(pPath as IPointCollection, ref result);
			return result;
		}

		public static Aran.Geometries.LineString ToLineString(ILine pLine)
		{
			LineString result = new LineString();
			if (pLine.IsEmpty)
				return result;
			result.Add(ToPoint(pLine.FromPoint));
			result.Add(ToPoint(pLine.ToPoint));
			return result;
		}

		public static Aran.Geometries.Ring ToRing(IRing pRing, bool isGeo)
		{
			Aran.Geometries.Ring result = new Aran.Geometries.Ring();
			if (pRing.IsEmpty)
				return result;
			if (isGeo)
				pRing.Generalize(0.0000001);
			else
				pRing.Generalize(0.001);
			LineString lnString = (LineString)result;
			ToLinestring(pRing as IPointCollection, ref  lnString);
			result = (Aran.Geometries.Ring)lnString;
			return result;
		}

		public static Aran.Geometries.MultiLineString ToPolyline(IPolyline pPolyline, bool isGeo)
		{
			Aran.Geometries.MultiLineString result = new Aran.Geometries.MultiLineString();
			if (pPolyline.IsEmpty)
				return result;

			IGeometryCollection pGeometryCollection = pPolyline as IGeometryCollection;

			for (int J = 0; J < pGeometryCollection.GeometryCount; J++)
			{
				IPath pPath = pGeometryCollection.get_Geometry(J) as IPath;
				if (isGeo)
					pPath.Generalize(0.0000001);
				else
					pPath.Generalize(0.1);
				IPointCollection pPointCollection = pPath as IPointCollection;

				Aran.Geometries.LineString lineString = new Aran.Geometries.LineString();
				for (int I = 0; I < pPointCollection.PointCount; I++)
				{
					IPoint pPoint = pPointCollection.get_Point(I);
					Aran.Geometries.Point aPoint = new Aran.Geometries.Point();

					aPoint.X = pPoint.X;
					aPoint.Y = pPoint.Y;
					aPoint.Z = pPoint.Z;
					aPoint.M = pPoint.M;
					aPoint.T = 0;
					lineString.Add(aPoint);
				}
				result.Add(lineString);
			}
			return result;
		}

		public static Aran.Geometries.Box ToBox(IEnvelope pEnvelope)
		{
			Aran.Geometries.Box result = new Aran.Geometries.Box();
			if (pEnvelope.IsEmpty)
				return result;

			result[0].X = pEnvelope.XMin;
			result[0].Y = pEnvelope.YMin;
			result[0].Z = pEnvelope.ZMin;
			result[0].M = pEnvelope.MMin;
			result[0].T = 0;

			result[1].X = pEnvelope.XMax;
			result[1].Y = pEnvelope.YMax;
			result[1].Z = pEnvelope.ZMax;
			result[1].M = pEnvelope.MMax;
			result[1].T = 0;
			return result;
		}

		public static Aran.Geometries.Geometry ToPolygonPrj(IPolygon pPolygon)
		{
			return ToPolygon(pPolygon, false);
		}

		public static Aran.Geometries.Geometry ToPolygonGeo(IPolygon pPolygon)
		{
			return ToPolygon(pPolygon, true);
		}

		public static Aran.Geometries.Geometry ToGeometry(IGeometry pGeom, bool isGeo = false)
		{
			switch (pGeom.GeometryType)
			{
				case esriGeometryType.esriGeometryPoint:
					return ToPoint(pGeom as IPoint);
				case esriGeometryType.esriGeometryMultipoint:
					return ToMultiPoint(pGeom as IMultipoint);
				case esriGeometryType.esriGeometryPolyline:
					return ToPolyline(pGeom as IPolyline, isGeo);
				case esriGeometryType.esriGeometryPolygon:
					return ToMultiPolygon(pGeom as IPolygon, isGeo);
				case esriGeometryType.esriGeometryEnvelope:
					return ToBox(pGeom as IEnvelope);
				case esriGeometryType.esriGeometryPath:
					return ToLineString(pGeom as IPath, isGeo);
				case esriGeometryType.esriGeometryRing:
					return ToRing(pGeom as IRing, isGeo);
				case esriGeometryType.esriGeometryLine:
					return ToLineString(pGeom as ILine);
				default:
					return null;
			}
		}

		private static Aran.Geometries.Geometry ToPolygon(IPolygon pPolygon, bool isGeo)
		{
            IPolygon4 pPolygon4 = pPolygon as IPolygon4;

		    if (pPolygon.IsEmpty || pPolygon.ExteriorRingCount == 0)
		        return new Geometries.MultiPolygon();
            
			IGeometryBag pExteriorRings = pPolygon4.ExteriorRingBag;
			IEnumGeometry pExteriorRingsEnum = pExteriorRings as ESRI.ArcGIS.Geometry.IEnumGeometry;
			if (pExteriorRingsEnum.Count < 2)
				return ToSinglePolygon(pPolygon, isGeo);

			return ToMultiPolygon(pPolygon, isGeo);
		}

		private static Aran.Geometries.Polygon ToSinglePolygon(IPolygon pPolygon, bool isGeo)
		{
			if (pPolygon.IsEmpty)
				return new Geometries.Polygon();

			IPolygon4 pPolygon4 = pPolygon as IPolygon4;
			IGeometryBag pExteriorRings = pPolygon4.ExteriorRingBag;
			IEnumGeometry pExteriorRingsEnum = pExteriorRings as ESRI.ArcGIS.Geometry.IEnumGeometry;

			LineString lnStrng;
			Aran.Geometries.Polygon Result = new Aran.Geometries.Polygon();

			pExteriorRingsEnum.Reset();
			IRing pRing = pExteriorRingsEnum.Next() as ESRI.ArcGIS.Geometry.IRing;

			if (pRing != null)
			{
				if (isGeo)
					pRing.Generalize(0.0000001);
				else
					pRing.Generalize(0.001);

				Aran.Geometries.Ring aRing = new Aran.Geometries.Ring();

				lnStrng = (LineString)aRing;
				ToLinestring(pRing as IPointCollection, ref lnStrng);

				aRing = (Aran.Geometries.Ring)lnStrng;

				Result.ExteriorRing = aRing;

				IGeometryBag pInteriorRings = pPolygon4.get_InteriorRingBag(pRing);
				IEnumGeometry pInteriorRingsEnum = pInteriorRings as ESRI.ArcGIS.Geometry.IEnumGeometry;
				pInteriorRingsEnum.Reset();

				pRing = pInteriorRingsEnum.Next() as ESRI.ArcGIS.Geometry.IRing;
				while (pRing != null)
				{
					if (isGeo)
						pRing.Generalize(0.0000001);
					else
						pRing.Generalize(0.001);

					Aran.Geometries.Ring interiorRing = new Aran.Geometries.Ring();

					lnStrng = (LineString)interiorRing;
					ToLinestring(pRing as IPointCollection, ref lnStrng);
					interiorRing = (Aran.Geometries.Ring)lnStrng;
					Result.InteriorRingList.Add(interiorRing);
					pRing = pInteriorRingsEnum.Next() as ESRI.ArcGIS.Geometry.IRing;
				}
			}
			return Result;
		}

	    private static Aran.Geometries.MultiPolygon ToMultiPolygon(IPolygon pPolygon, bool isGeo)
	    {
	        try
	        {
	            if (pPolygon.IsEmpty)
	                return new Geometries.MultiPolygon();

	            IPolygon4 pPolygon4 = pPolygon as IPolygon4;
	            if (pPolygon4.ExteriorRingBag == null)
	                return null;
	            IGeometryBag pExteriorRings = pPolygon4.ExteriorRingBag;
	            IEnumGeometry pExteriorRingsEnum = pExteriorRings as ESRI.ArcGIS.Geometry.IEnumGeometry;

	            LineString lnStrng;
	            MultiPolygon Result = new Aran.Geometries.MultiPolygon();

	            pExteriorRingsEnum.Reset();
	            IRing pRing = pExteriorRingsEnum.Next() as ESRI.ArcGIS.Geometry.IRing;

	            while (pRing != null)
	            {
	                if (isGeo)
	                    pRing.Generalize(0.0000001);
	                else
	                    pRing.Generalize(0.001);

	                Aran.Geometries.Ring aRing = new Aran.Geometries.Ring();

	                Aran.Geometries.Polygon aPolygon = new Aran.Geometries.Polygon();

	                lnStrng = (LineString) aRing;
	                ToLinestring(pRing as IPointCollection, ref lnStrng);
	                aRing = (Aran.Geometries.Ring) lnStrng;
	                aPolygon.ExteriorRing = aRing;

	                try
	                {
	                    IGeometryBag pInteriorRings = pPolygon4.get_InteriorRingBag(pRing);
	                    IEnumGeometry pInteriorRingsEnum = pInteriorRings as ESRI.ArcGIS.Geometry.IEnumGeometry;
	                    pInteriorRingsEnum.Reset();
	                    pRing = pInteriorRingsEnum.Next() as ESRI.ArcGIS.Geometry.IRing;
	                    while (pRing != null)
	                    {
	                        if (isGeo)
	                            pRing.Generalize(0.0000001);
	                        else
	                            pRing.Generalize(0.001);

	                        Aran.Geometries.Ring interiorRing = new Aran.Geometries.Ring();

	                        lnStrng = (LineString) interiorRing;
	                        ToLinestring(pRing as IPointCollection, ref lnStrng);
	                        interiorRing = (Aran.Geometries.Ring) lnStrng;
	                        aPolygon.InteriorRingList.Add(interiorRing);
	                        pRing = pInteriorRingsEnum.Next() as ESRI.ArcGIS.Geometry.IRing;
	                    }
	                }
	                catch (Exception e)
	                {
	                    Debug.WriteLine(e.Message);
	                }


	                Result.Add(aPolygon);
	                pRing = pExteriorRingsEnum.Next() as ESRI.ArcGIS.Geometry.IRing;
	            }
	            return Result;
	        }
	        catch (Exception)
	        {
	            return null;
	        }
	    }
	}
}
