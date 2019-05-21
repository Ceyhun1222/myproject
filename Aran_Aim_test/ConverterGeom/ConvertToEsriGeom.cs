using ESRI.ArcGIS.Geometry;
using Aran.Geometries;
using System;

namespace Aran.Converters
{
    public static class ConvertToEsriGeom
    {
        public static IGeometry FromGeometry(Aran.Geometries.Geometry aranGeom, bool isZaware = false, ISpatialReference SR = null, bool doSimplify = true)
        {
            switch (aranGeom.Type)
            {
                case GeometryType.Point:
                    return FromPoint((Aran.Geometries.Point)aranGeom, isZaware);
                case GeometryType.LineString:
                    return FromLineString((LineString)aranGeom, isZaware,SR,doSimplify);
                case GeometryType.Polygon:
                    return FromPolygon((Aran.Geometries.Polygon)aranGeom, isZaware, SR);
                case GeometryType.MultiLineString:
                    return FromMultiLineString((Aran.Geometries.MultiLineString)aranGeom, isZaware, SR,doSimplify);
                case GeometryType.MultiPolygon:
                    return FromMultiPolygon((Aran.Geometries.MultiPolygon)aranGeom, isZaware, SR);
                case GeometryType.MultiPoint:
                    return FromMultiPoint((MultiPoint)aranGeom);
                case GeometryType.Ring:
                    return (IGeometry)FromRing((Aran.Geometries.Ring)aranGeom, isZaware);

                    //Aran.Geometries.Polygon plygon = new Aran.Geometries.Polygon();
                    //plygon.Add((Aran.Geometries.Ring)aranGeom);
                    //return FromPolygon ( plygon );

            }
            throw new Exception("Geometry Type is not implemented (in FromGeometry)!");
        }

        public static IMultipoint FromMultiPoint(MultiPoint multiPoint, bool isZaware = false)
        {
            IPointCollection result = new ESRI.ArcGIS.Geometry.Multipoint() as IPointCollection;
            IZAware zaware = result as IZAware;
            zaware.ZAware = isZaware;

            foreach (Aran.Geometries.Point point in multiPoint)
                result.AddPoint(FromPoint(point, isZaware));

            return result as IMultipoint;
        }

        public static IPath LineStringToPath(LineString lineString, bool isZaware = false)
        {
            IPointCollection result = new ESRI.ArcGIS.Geometry.Path() as IPointCollection;
            IZAware zaware = result as IZAware;
            zaware.ZAware = isZaware;

            foreach (Aran.Geometries.Point pnt in lineString)
                result.AddPoint(ConvertToEsriGeom.FromPoint(pnt, isZaware));

            return result as IPath;
        }

        public static IRing FromRing(Aran.Geometries.Ring rng, bool isZaware = false)
        {
            IPointCollection result = new ESRI.ArcGIS.Geometry.Ring() as IPointCollection;
            IZAware zaware = result as IZAware;
            zaware.ZAware = isZaware;

            foreach (Aran.Geometries.Point pnt in rng)
                result.AddPoint(ConvertToEsriGeom.FromPoint(pnt, isZaware));

            return result as IRing;
        }

        public static IPolyline FromLineString(
            LineString lineString,
            bool isZaware = false,
            ISpatialReference SR = null,
            bool doSimplify = true)
        {
            IPointCollection result = new ESRI.ArcGIS.Geometry.Polyline() as IPointCollection;
            IZAware zaware = result as IZAware;
            zaware.ZAware = isZaware;

            foreach (Aran.Geometries.Point point in lineString)
                result.AddPoint(FromPoint(point, isZaware));

            SetSpartialReference(result, SR);

            if (doSimplify)
            {
                ITopologicalOperator2 pTopo = result as ITopologicalOperator2;
                pTopo.IsKnownSimple_2 = false;
                pTopo.Simplify();
            }

            return result as IPolyline;
        }

        public static IPolyline FromMultiLineString(Aran.Geometries.MultiLineString aranMultiLineString, bool isZAware = false, ISpatialReference SR = null, bool doSimplify = true)
        {
            IGeometryCollection result = new ESRI.ArcGIS.Geometry.Polyline() as IGeometryCollection;
            IZAware zaware = result as IZAware;
            zaware.ZAware = isZAware;

            foreach (LineString lnString in aranMultiLineString)
            {
                IPath pPath = LineStringToPath(lnString, isZAware);
                result.AddGeometry(pPath as IGeometry);
            }

            SetSpartialReference(result, SR);

            if (doSimplify)
            {
                ITopologicalOperator2 pTopo = result as ITopologicalOperator2;
                pTopo.IsKnownSimple_2 = false;
                pTopo.Simplify();
            }

            return (IPolyline)result;
        }

        public static IPolygon FromPolygon(Aran.Geometries.Polygon aranPolygon, bool isZAware = false, ISpatialReference SR = null)
        {
            IGeometryCollection result = new ESRI.ArcGIS.Geometry.Polygon() as IGeometryCollection;
            IZAware zaware = result as IZAware;
            zaware.ZAware = isZAware;

            IRing extRng = FromRing(aranPolygon.ExteriorRing, isZAware);
            IPointCollection ptCol = extRng as IPointCollection;

            if (!extRng.IsExterior)
                extRng.ReverseOrientation();

            result.AddGeometry(extRng as IGeometry);

            foreach (Aran.Geometries.Ring ring in aranPolygon.InteriorRingList)
            {
                IRing rng = FromRing(ring, isZAware);
                if (rng.IsExterior)
                    rng.ReverseOrientation();

                result.AddGeometry(rng as IGeometry);
            }
            SetSpartialReference(result, SR);
            IPointCollection ptCol1 = result as IPointCollection;

            ITopologicalOperator2 pTopo = result as ITopologicalOperator2;
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();
            return result as IPolygon;
        }

        public static IPolygon FromMultiPolygon(Aran.Geometries.MultiPolygon aranMultiPolygon, bool isZaware = false, ISpatialReference SR = null)
        {
            IPolygon result = new ESRI.ArcGIS.Geometry.Polygon() as IPolygon;
            IZAware zaware = result as IZAware;
            zaware.ZAware = isZaware;


            foreach (Aran.Geometries.Polygon plygon in aranMultiPolygon)
            {
                ITopologicalOperator2 pTopo = FromPolygon(plygon, isZaware, SR) as ITopologicalOperator2;
                result = pTopo.Union(result) as IPolygon;
                SetSpartialReference(result, SR);
                pTopo = result as ITopologicalOperator2;
                pTopo.IsKnownSimple_2 = false;
                pTopo.Simplify();
            }

            return result;
        }

        public static IPoint FromPoint(Aran.Geometries.Point aranPoint, bool isZaware = false)
        {
            IPoint result = new ESRI.ArcGIS.Geometry.Point();
            result.PutCoords(aranPoint.X, aranPoint.Y);

            var zAware = (IZAware)result;
            zAware.ZAware = isZaware;

            if (!double.IsNaN(aranPoint.Z))
                result.Z = aranPoint.Z;
            else
                result.Z = 0.0;

            if (!double.IsNaN(aranPoint.M))
                result.M = aranPoint.M;

            return result;
        }

        public static void SetSpartialReference(Object obj, ISpatialReference SR)
        {
            if (SR != null)
                (obj as IGeometry).SpatialReference = SR;
        }
    }
}
