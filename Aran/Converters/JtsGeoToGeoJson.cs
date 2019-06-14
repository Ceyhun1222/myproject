using System;
using System.Collections.Generic;
using System.Text;

namespace Aran.Converters.ConvertGeoJsonToJtsGeom
{
    class JtsGeoToGeoJson
    {
        public static MultiPolygon GetMultiPolygon(Aran.Geometries.MultiPolygon multiPolygon)
        {
            if (multiPolygon == null)
                return null;

            var polygonList = new List<Polygon>();

            foreach (Aran.Geometries.Polygon polygon in multiPolygon)
            {
                var lineStringList = new List<LineString>();

                var exteriorRingPositionList = new List<IPosition>();

                foreach (Aran.Geometries.Point point in ((Aran.Geometries.LineString)polygon.ExteriorRing).ToMultiPoint())
                {
                    exteriorRingPositionList.Add(new Position(latitude: point.X, longitude: point.Y, altitude: point.Z));
                }

                lineStringList.Add(new LineString(exteriorRingPositionList));

                foreach (var interiorRing in polygon.InteriorRingList)
                {

                    var interiorRingPositionList = new List<IPosition>();

                    foreach (Aran.Geometries.Point point in ((Aran.Geometries.LineString)interiorRing).ToMultiPoint())
                    {
                        interiorRingPositionList.Add(new Position(latitude: point.X, longitude: point.Y, altitude: point.Z));
                    }

                    lineStringList.Add(new LineString(interiorRingPositionList));
                }

                polygonList.Add(new Polygon(lineStringList));

            }

            return new MultiPolygon(polygonList);
        }

        public static MultiLineString GetMultiLineString(Aran.Geometries.MultiLineString multiLineString)
        {
            if (multiLineString == null)
                return null;
            var lineStringList = new List<LineString>();

            foreach (Aran.Geometries.LineString lineString in multiLineString)
            {
                var lineStringPositionList = new List<IPosition>();

                foreach (Aran.Geometries.Point point in lineString.ToMultiPoint())
                {
                    lineStringPositionList.Add(new Position(latitude: point.X, longitude: point.Y, altitude: point.Z));
                }

                lineStringList.Add(new LineString(lineStringPositionList));
            }

            return new MultiLineString(lineStringList);
        }

        public static Point GetPoint(Aran.Geometries.Point point)
        {
            if (point == null)
                return null;

            return new Point(new Position(latitude: point.X, longitude: point.Y, altitude: point.Z));
        }
    }
}
