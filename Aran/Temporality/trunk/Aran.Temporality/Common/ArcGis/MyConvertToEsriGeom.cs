using System;
using Aran.Converters;
using Aran.Geometries;
using ESRI.ArcGIS.Geometry;

namespace Aran.Temporality.Common.ArcGis
{
    class MyConvertToEsriGeom
    {
        public static IGeometry FromGeometry(Geometry aranGeom)
		{
			switch (aranGeom.Type)
			{
				case GeometryType.Point:
					return FromPointZ((Geometries.Point)aranGeom);
				case GeometryType.LineString:
					return FromLineString((LineString)aranGeom);
				case GeometryType.Polygon:
					return FromPolygon((Geometries.Polygon)aranGeom);
				case GeometryType.MultiLineString:
					return FromMultiLineString((MultiLineString)aranGeom);
				case GeometryType.MultiPolygon:
					return FromMultiPolygon((MultiPolygon)aranGeom);
				case GeometryType.MultiPoint:
					return FromMultiPoint((MultiPoint)aranGeom);
				case GeometryType.Ring:
					return (IGeometry)FromRing((Geometries.Ring)aranGeom);

				//Aran.Geometries.Polygon plygon = new Aran.Geometries.Polygon();
				//plygon.Add((Aran.Geometries.Ring)aranGeom);
				//return FromPolygon ( plygon );

			}
			throw new Exception("Geometry Type is not implemented (in FromGeometry)!");
		}

        //public static IGeometry FromGeometryIntersect(Aran.Geometries.Geometry aranGeom)
        //{

        //    switch (aranGeom.Type)
        //    {
        //        case GeometryType.Point:
        //            return FromPointZ((Aran.Geometries.Point)aranGeom);
        //        case GeometryType.LineString:
        //            return FromLineString((LineString)aranGeom);
        //        case GeometryType.Polygon:
        //            return FromPolygonIntersect((Aran.Geometries.Polygon)aranGeom);
        //        case GeometryType.MultiLineString:
        //            return FromMultiLineString((Aran.Geometries.MultiLineString)aranGeom);
        //        case GeometryType.MultiPolygon:
        //            return FromMultiPolygonIntersect((Aran.Geometries.MultiPolygon)aranGeom);
        //        case GeometryType.MultiPoint:
        //            return FromMultiPoint((MultiPoint)aranGeom);
        //        case GeometryType.Ring:
        //            return (IGeometry)FromRing((Aran.Geometries.Ring)aranGeom);

        //        //Aran.Geometries.Polygon plygon = new Aran.Geometries.Polygon();
        //        //plygon.Add((Aran.Geometries.Ring)aranGeom);
        //        //return FromPolygon ( plygon );

        //    }
        //    throw new Exception("Geometry Type is not implemented (in FromGeometry)!");
        //}
		

		public static IPath LineStringToPath(LineString lineString)
		{
			IPointCollection result = new Path() as IPointCollection;

			foreach (Geometries.Point pnt in lineString)
				result.AddPoint(ConvertToEsriGeom.FromPoint(pnt,true));

			return result as IPath;
		}

		public static IRing FromRing(Geometries.Ring rng)
		{
			IPointCollection result = new ESRI.ArcGIS.Geometry.Ring() as IPointCollection;

			foreach (Geometries.Point pnt in rng)
				result.AddPoint(ConvertToEsriGeom.FromPoint(pnt,true));
            IZAware zaware = result as IZAware;
            zaware.ZAware = true;
			return result as IRing;
		}

		public static IPolygon FromPolygon(Geometries.Polygon aranPolygon,bool isZAware=true)
		{
			IGeometryCollection result = new ESRI.ArcGIS.Geometry.Polygon() as IGeometryCollection;

			IRing extRng = FromRing(aranPolygon.ExteriorRing);
            IPointCollection ptCol = extRng as IPointCollection;

			if ( !extRng.IsExterior )
				extRng.ReverseOrientation ( );
			result.AddGeometry(extRng as IGeometry);

			foreach (Geometries.Ring ring in aranPolygon.InteriorRingList)
			{
				IRing rng = FromRing(ring);
				if ( rng.IsExterior )
					rng.ReverseOrientation ( );
				result.AddGeometry(rng as IGeometry);
			}
            if (isZAware)
            {
                IZAware zaware = result as IZAware;
                zaware.ZAware = true;
            }
            IPointCollection ptCol1 = result as IPointCollection;

            ITopologicalOperator2 pTopo = result as ITopologicalOperator2;
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();
			return result as IPolygon;
		}

		public static IPolygon FromMultiPolygon(MultiPolygon aranMultiPolygon,bool isZaware=true)
		{
			IPolygon result = new ESRI.ArcGIS.Geometry.Polygon() as IPolygon;
            if (isZaware)
            {
                IZAware zaware = result as IZAware;
                zaware.ZAware = true;
            }

			foreach (Geometries.Polygon plygon in aranMultiPolygon)
			{
			    var pg = FromPolygon(plygon);
			   // result = pg;
			    ITopologicalOperator2 pTopo = pg  as ITopologicalOperator2;
			    result = pTopo.Union(result) as IPolygon;
			}

           ITopologicalOperator2 pTopo2 = result as ITopologicalOperator2;
		   pTopo2.Simplify();
		   return result;
		}

        //With Z aware
        public static IPoint FromPointZ(Geometries.Point aranPoint)
        {
            IPoint result = new ESRI.ArcGIS.Geometry.Point();
            result.PutCoords(aranPoint.X, aranPoint.Y);

            var zAware = (IZAware)result;
            zAware.ZAware = true;

            result.Z = !double.IsNaN(aranPoint.Z) ? aranPoint.Z : 0;

            if (!double.IsNaN(aranPoint.M))
                result.M = aranPoint.M;

            return result;
        }

        public static IMultipoint FromMultiPoint(MultiPoint multiPoint)
        {
            IPointCollection result = new Multipoint() as IPointCollection;
            var zAware = (IZAware)result;
            zAware.ZAware = true;

            foreach (Geometries.Point point in multiPoint)
                result.AddPoint(FromPointZ(point));

            return result as IMultipoint;
        }

        public static IPath LineStringToPathZ(LineString lineString)
        {
            IPointCollection result = new Path() as IPointCollection;
            var zAware = (IZAware)result;
            zAware.ZAware = true;

            foreach (Geometries.Point pnt in lineString)
                result.AddPoint(ConvertToEsriGeom.FromPoint(pnt,true));

            return result as IPath;
        }

        public static IRing FromRingZ(Geometries.Ring rng)
        {
            IPointCollection result = new ESRI.ArcGIS.Geometry.Ring() as IPointCollection;
            var zAware = (IZAware)result;
            zAware.ZAware = true;

            foreach (Geometries.Point pnt in rng)
                result.AddPoint(ConvertToEsriGeom.FromPoint(pnt,true));

            return result as IRing;
        }

        public static IPolyline FromLineString(LineString lineString)
        {
            IPointCollection result = new Polyline() as IPointCollection;
            var zAware = (IZAware)result;
            zAware.ZAware = true;

            foreach (Geometries.Point point in lineString)
                result.AddPoint(FromPointZ(point));

            return result as IPolyline;
        }

        public static IPolyline FromMultiLineString(MultiLineString aranMultiLineString)
        {
            IGeometryCollection result = new Polyline() as IGeometryCollection;

            foreach (LineString lnString in aranMultiLineString)
            {
                IPath pPath = LineStringToPath(lnString);
                result.AddGeometry(pPath as IGeometry);
            }

            var zAware = (IZAware)result;
            zAware.ZAware = true;

            ITopologicalOperator2 pTopo = result as ITopologicalOperator2;
            pTopo.IsKnownSimple_2 = false;
            //pTopo.Simplify();

            return (IPolyline)result;
        }

        public static IPolygon FromPolygonZ(Geometries.Polygon aranPolygon)
        {
            IGeometryCollection result = new ESRI.ArcGIS.Geometry.Polygon() as IGeometryCollection;

            IRing extRng = FromRingZ(aranPolygon.ExteriorRing);
            result.AddGeometry(extRng as IGeometry);

            foreach (Geometries.Ring ring in aranPolygon.InteriorRingList)
            {
                IRing rng = FromRingZ(ring);
                result.AddGeometry(rng as IGeometry);
            }

            var zAware = (IZAware)result;
            zAware.ZAware = true;
            
            ITopologicalOperator2 pTopo = result as ITopologicalOperator2;
            pTopo.IsKnownSimple_2 = false;
            try
            {
                //pTopo.Simplify();
            }
            catch
            {
                pTopo.IsKnownSimple_2 = false;
                try
                {
                    //pTopo.Simplify();
                }
                catch
                {
                    // ignored
                }
            }

            return result as IPolygon;
        }

        public static IPolygon FromMultiPolygonZ(MultiPolygon aranMultiPolygon)
        {
            IPolygon result = new ESRI.ArcGIS.Geometry.Polygon() as IPolygon;

            foreach (Geometries.Polygon plygon in aranMultiPolygon)
            {
                ITopologicalOperator2 pTopo = FromPolygonZ(plygon) as ITopologicalOperator2;
                result = pTopo.Union(result) as IPolygon;
                pTopo = result as ITopologicalOperator2;
                pTopo.IsKnownSimple_2 = false;
                //pTopo.Simplify();
            }
            var zAware = (IZAware)result;
            zAware.ZAware = true;
            
            return result;
        }
	}
    
}
