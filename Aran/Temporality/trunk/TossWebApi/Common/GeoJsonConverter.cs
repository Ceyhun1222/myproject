using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TossWebApi.Common
{
    public class GeoJsonConverter
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
                    var newPoint = GetPoint(point);
                    if (newPoint != null)
                        exteriorRingPositionList.Add(newPoint.Coordinates);
                }

                if (exteriorRingPositionList.Count == 0)
                    return null;

                lineStringList.Add(new LineString(exteriorRingPositionList));

                foreach (var interiorRing in polygon.InteriorRingList)
                {
                    var interiorRingPositionList = new List<IPosition>();

                    foreach (Aran.Geometries.Point point in ((Aran.Geometries.LineString)interiorRing).ToMultiPoint())
                    {
                        var newPoint = GetPoint(point);
                        if (newPoint != null)
                            interiorRingPositionList.Add(newPoint.Coordinates);
                    }

                    if (interiorRingPositionList.Count > 0)
                        lineStringList.Add(new LineString(interiorRingPositionList));
                }

                polygonList.Add(new Polygon(lineStringList));

            }

            return new MultiPolygon(polygonList);
        }

        public static Aran.Geometries.MultiPolygon GetAranMultiPolygon(MultiPolygon multiPolygon)
        {
            if (multiPolygon == null)
                return null;

            var aranMultiPolygon = new Aran.Geometries.MultiPolygon();

            bool isFirst = true;
            foreach (var polygon in multiPolygon.Coordinates)
            {
                var aranPolygon = new Aran.Geometries.Polygon();
                foreach (var lineString in polygon.Coordinates)
                {
                    var aranRing = new Aran.Geometries.Ring();
                    foreach (var coordinate in lineString.Coordinates)
                    {
                        var aranPoint = GetAranPoint(coordinate);
                        if (aranPoint != null)
                            aranRing.Add(aranPoint);
                    }
                    
                    if (isFirst)
                    {
                        aranPolygon.ExteriorRing = aranRing;
                        isFirst = false;
                        continue;
                    }

                    aranPolygon.InteriorRingList.Add(aranRing);
                }
                aranMultiPolygon.Add(aranPolygon);
            }

            return aranMultiPolygon;
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
                    var position = GetPoint(point)?.Coordinates;
                    if (position != null)
                        lineStringPositionList.Add(position);
                }

                if (lineStringPositionList.Count > 0)
                    lineStringList.Add(new LineString(lineStringPositionList));
            }

            return new MultiLineString(lineStringList);
        }

        public static Aran.Geometries.MultiLineString GetAranMultiLineString(MultiLineString multiLineString)
        {
            if (multiLineString == null)
                return null;

            var aranMultiLineString = new Aran.Geometries.MultiLineString();

            foreach (var lineString in multiLineString.Coordinates)
            {
                var aranLineString = new Aran.Geometries.LineString();
                foreach (var coordinate in lineString.Coordinates)
                {
                    var point = GetAranPoint(coordinate);

                    if (point != null)
                        aranLineString.Add(point);
                }
                if (!aranLineString.IsEmpty)
                    aranMultiLineString.Add(aranLineString);
            }

            return aranMultiLineString;
        }

        public static Point GetPoint(Aran.Geometries.Point point)
        {
            if (point == null)
                return null;

            return new Point(new Position(longitude: point.X, latitude: point.Y, 
                altitude: (double.IsNaN(point.Z) ? 0.0 : point.Z) ));
        }

        public static Aran.Geometries.Point GetAranPoint(IPosition position)
        {
            if (position == null)
                return null;

            return new Aran.Geometries.Point(position.Latitude, position.Longitude,
                position.Altitude ?? double.NaN);
        }

        public static Aran.Geometries.Point GetAranPoint(Point point)
        {
            if (point == null)
                return null;

            return GetAranPoint(point.Coordinates);
        }
    }
}