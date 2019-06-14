using GeoAPI.Geometries;
using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;

namespace ConverterGeoJson
{
    class JtsGeoToGeoJson
    {
        public MultiPolygon ToMultiPolygon(GeoAPI.Geometries.IMultiPolygon multiPolygon)
        {
            if (multiPolygon == null)
                return null;

            var polygonList = new List<GeoJSON.Net.Geometry.Polygon>();

            foreach (IPolygon polygon in multiPolygon)
            {
                var lineStringList = new List<LineString>();

                var exteriorRingPositionList = new List<IPosition>();

                foreach (var point in polygon.ExteriorRing.Coordinates)
                    exteriorRingPositionList.Add(new Position(latitude: point.X, longitude: point.Y, altitude: point.Z));

                lineStringList.Add(new LineString(exteriorRingPositionList));

                foreach (var interiorRing in polygon.InteriorRings)
                {

                    var interiorRingPositionList = new List<IPosition>();

                    foreach (var point in interiorRing.Coordinates)
                        interiorRingPositionList.Add(new Position(latitude: point.X, longitude: point.Y, altitude: point.Z));

                    lineStringList.Add(new LineString(interiorRingPositionList));
                }

                polygonList.Add(new Polygon(lineStringList));
            }

            return new MultiPolygon(polygonList);
        }

        public MultiLineString ToMultiLineString(GeoAPI.Geometries.IMultiLineString multiLineString)
        {
            if (multiLineString == null)
                return null;
            var lineStringList = new List<LineString>();

            foreach (var lineString in multiLineString)
            {
                var lineStringPositionList = new List<IPosition>();

                foreach (var point in lineString.Coordinates)
                    lineStringPositionList.Add(new Position(latitude: point.X, longitude: point.Y, altitude: point.Z));

                lineStringList.Add(new LineString(lineStringPositionList));
            }

            return new MultiLineString(lineStringList);
        }

        public Point ToPoint(IPoint point)
        {
            if (point == null)
                return null;

            return new Point(new Position(latitude: point.Y, longitude: point.X, altitude: point.Z));
        }
    }
}
