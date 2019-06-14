using GeoAPI.Geometries;
using GeoJSON.Net;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aran.Converters.ConverterGeoJson
{
    class GeoJsonToJtsGeo
    {
        //public static GeoAPI.Geometries.IGeometry FromGeometry(GeoJSON.Net.Geometry.IGeometryObject aranGeom)
        //{
        //    switch (aranGeom.Type)
        //    {
        //        case GeoJSONObjectType.Point:
        //            return FromPoint((Aran.Geometries.Point)aranGeom);
        //        case GeoJSONObjectType.LineString:
        //            return FromLineString((Aran.Geometries.LineString)aranGeom);
        //        case GeoJSONObjectType.Polygon:
        //            return FromPolygon((Aran.Geometries.Polygon)aranGeom);
        //        case GeoJSONObjectType.MultiLineString:
        //            return FromMultiLineString((Aran.Geometries.MultiLineString)aranGeom);
        //        case GeoJSONObjectType.MultiPolygon:
        //            return FromMultiPolygon((Aran.Geometries.MultiPolygon)aranGeom);
        //        case GeoJSONObjectType.Ring:
        //            return (IGeometry)FromRing((Aran.Geometries.Ring)aranGeom);

        //            //Aran.Geometries.Polygon plygon = new Aran.Geometries.Polygon();
        //            //plygon.Add((Aran.Geometries.Ring)aranGeom);
        //            //return FromPolygon ( plygon );

        //    }
        //    throw new Exception("Geometry Type is not implemented (in FromGeometry)!");
        //}

        public ILineString FromLineString(GeoJSON.Net.Geometry.LineString lineString)
        {
            Coordinate[] points = new Coordinate[lineString.Coordinates.Count];
            for (int i = 0; i < lineString.Coordinates.Count; i++)
                points[i] = FromPoint(lineString.Coordinates[i]).Coordinate;

            ILineString result = new NetTopologySuite.Geometries.LineString(points);
            return result;
        }

        public ILinearRing FromRing(GeoJSON.Net.Geometry.LineString rng)
        {
            var list = new List<Coordinate>();
            for (int i = 0; i < rng.Coordinates.Count; i++)
                list.Add(FromPoint(rng.Coordinates[i]).Coordinate);

            if (list.Count > 0)
            {
                if (!list[0].Equals2D(list[rng.Coordinates.Count - 1]))
                    list.Add(list[0]);
            }
            ILinearRing result = new LinearRing(list.ToArray());
            return result;
        }

        public IMultiLineString FromMultiLineString(GeoJSON.Net.Geometry.MultiLineString jsonMultiLineString)
        {
            ILineString[] lineStrings = new NetTopologySuite.Geometries.LineString[jsonMultiLineString.Coordinates.Count];
            for (int i = 0; i < jsonMultiLineString.Coordinates.Count; i++)
                lineStrings[i] = FromLineString(jsonMultiLineString.Coordinates[i]);

            IMultiLineString result = new NetTopologySuite.Geometries.MultiLineString(lineStrings);
            return result;
        }

        public IPolygon FromPolygon(GeoJSON.Net.Geometry.Polygon jsonPolygon)
        {
            IPolygon result = null;
            if (jsonPolygon.Coordinates.Count == 0) return result;

            ILinearRing extRng = FromRing(jsonPolygon.Coordinates[0]);

            var interiorRingCount = jsonPolygon.Coordinates.Count-1;
            if (interiorRingCount > 0)
            {
                ILinearRing[] interiorRings = new LinearRing[interiorRingCount];
                for (int i = 1; i < interiorRingCount; i++)
                {
                    ILinearRing rng = FromRing(jsonPolygon.Coordinates[i]);
                    interiorRings[i] = rng;
                }
                result = new NetTopologySuite.Geometries.Polygon(extRng, interiorRings);
            }
            else
                result = new NetTopologySuite.Geometries.Polygon(extRng);

            return result;
        }

        public IMultiPolygon FromMultiPolygon(GeoJSON.Net.Geometry.MultiPolygon jsonMultiPolygon)
        {
            IPolygon[] polygons = new IPolygon[jsonMultiPolygon.Coordinates.Count];

            int i = 0;
            foreach (var polygon in jsonMultiPolygon.Coordinates)
                polygons[i++] = FromPolygon(polygon);

            IMultiPolygon result = new NetTopologySuite.Geometries.MultiPolygon(polygons);
            return result;
        }

        //With Z aware
        public IPoint FromPoint(GeoJSON.Net.Geometry.IPosition jsonPoint)
        {
            IPoint result = new NetTopologySuite.Geometries.Point(jsonPoint.Longitude, jsonPoint.Latitude, jsonPoint.Altitude.HasValue? jsonPoint.Altitude.Value:0);
            return result;
        }
    }
}
