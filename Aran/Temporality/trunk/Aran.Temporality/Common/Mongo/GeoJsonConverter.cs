using Aran.Geometries;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Temporality.Common.Mongo
{
    public class GeoJsonConverter
    {
        public static GeoJsonPoint<GeoJson3DGeographicCoordinates> GetGeoJsonPoint(Point point)
        {
            if (point == null)
                return null;

            return new GeoJsonPoint<GeoJson3DGeographicCoordinates>(
                new GeoJson3DGeographicCoordinates(point.X, point.Y, point.Z));
        }

        public static Point GetAranPoint(GeoJson3DGeographicCoordinates geoJsonCoordinates)
        {
            if (geoJsonCoordinates == null)
                return null;

            return new Point(geoJsonCoordinates.Longitude, geoJsonCoordinates.Latitude, geoJsonCoordinates.Altitude);
        }

        public static Point GetAranPoint(GeoJsonPoint<GeoJson3DGeographicCoordinates> geoJsonPoint)
        {
            if (geoJsonPoint == null)
                return null;

            return new Point(geoJsonPoint.Coordinates.Longitude, geoJsonPoint.Coordinates.Latitude, geoJsonPoint.Coordinates.Altitude);
        }

        public static GeoJsonMultiLineString<GeoJson3DGeographicCoordinates> GetGeoJsonMultiLineString(MultiLineString multiLineString)
        {
            if (multiLineString == null)
                return null;

            var geoJsonLineStringCoordinates = new List<GeoJsonLineStringCoordinates<GeoJson3DGeographicCoordinates>>();

            foreach (LineString lineString in multiLineString)
            {
                if (lineString.Count < 2)
                    continue;

                if (lineString.Distinct(new PointEqualityComparer()).Count() == 1)
                    continue;

                var points = new List<GeoJson3DGeographicCoordinates>();
                foreach (Point point in lineString)
                {
                    points.Add(new GeoJson3DGeographicCoordinates(point.X, point.Y, point.Z));
                }
                geoJsonLineStringCoordinates.Add(new GeoJsonLineStringCoordinates<GeoJson3DGeographicCoordinates>(points));
            }

            if (geoJsonLineStringCoordinates.Count == 0)
                return null;

            var geoJsonMultiLineString = new GeoJsonMultiLineStringCoordinates<GeoJson3DGeographicCoordinates>(geoJsonLineStringCoordinates);
            return new GeoJsonMultiLineString<GeoJson3DGeographicCoordinates>(geoJsonMultiLineString);
        }

        public static MultiLineString GetAranMultiLineString(GeoJsonMultiLineString<GeoJson3DGeographicCoordinates> geoJsonMultiLineString)
        {
            if (geoJsonMultiLineString == null)
                return null;

            var aranMultiLineString = new MultiLineString();

            foreach (var geoJsonLineString in geoJsonMultiLineString.Coordinates.LineStrings)
            {
                var lineString = new LineString();
                foreach (var position in geoJsonLineString.Positions)
                {
                    lineString.Add(GetAranPoint(position));
                }
                aranMultiLineString.Add(lineString);
            }

            return aranMultiLineString;
        }

        private static void Close(List<GeoJson3DGeographicCoordinates> coordinates)
        {
            if (coordinates.Count == 0)
                return;

            if (Math.Abs(coordinates.First().Latitude - coordinates.Last().Latitude) < 0.0000001
                && Math.Abs(coordinates.First().Longitude - coordinates.Last().Longitude) < 0.0000001)
                coordinates.RemoveAt(coordinates.Count - 1);

            coordinates.Add(coordinates.First());
        }

        private static GeoJsonPolygonCoordinates<GeoJson3DGeographicCoordinates> GetGeoJsonPolygonCoordinates(
            Polygon polygon)
        {
            if (polygon == null || polygon.ExteriorRing.Count == 0)
                return null;

            if (polygon.ExteriorRing.Distinct(new PointEqualityComparer()).Count() == 1)
                return null;

            var exteriorPoints = polygon.ExteriorRing.Select(p => new GeoJson3DGeographicCoordinates(p.X, p.Y, p.Z)).ToList();

            Close(exteriorPoints);

            if (exteriorPoints.Count < 4)
                return null;

            var exterior = new GeoJsonLinearRingCoordinates<GeoJson3DGeographicCoordinates>(exteriorPoints);

            var holes = new List<GeoJsonLinearRingCoordinates<GeoJson3DGeographicCoordinates>>();
            foreach (Ring ring in polygon.InteriorRingList)
            {
                if (ring == null || ring.Count == 0)
                    continue;

                if (ring.Distinct(new PointEqualityComparer()).Count() == 1)
                    continue;

                var interiorPoints = ring.Select(p => new GeoJson3DGeographicCoordinates(p.X, p.Y, p.Z)).ToList();

                Close(interiorPoints);

                if (interiorPoints.Count < 4)
                    continue;

                holes.Add(new GeoJsonLinearRingCoordinates<GeoJson3DGeographicCoordinates>(interiorPoints));
            }

            if (holes.Count > 0)
                return new GeoJsonPolygonCoordinates<GeoJson3DGeographicCoordinates>(exterior, holes);
            else
                return new GeoJsonPolygonCoordinates<GeoJson3DGeographicCoordinates>(exterior);
        }

        public static GeoJsonGeometry<GeoJson3DGeographicCoordinates> GetGeoJsonPolygon(Polygon polygon)
        {
            var coordinates = GetGeoJsonPolygonCoordinates(polygon);
            if (coordinates == null)
                return null;

            return new GeoJsonPolygon<GeoJson3DGeographicCoordinates>(coordinates);
        }

        public static GeoJsonMultiPolygon<GeoJson3DGeographicCoordinates> GetGeoJsonMultiPolygon(MultiPolygon multiPolygon)
        {
            if (multiPolygon == null)
                return null;

            var list = new List<GeoJsonPolygonCoordinates<GeoJson3DGeographicCoordinates>>();
            foreach (Polygon polygon in multiPolygon)
            {
                var coordinates = GetGeoJsonPolygonCoordinates(polygon);
                if (coordinates != null)
                    list.Add(coordinates);
            }

            if (list.Count == 0)
                return null;

            return new GeoJsonMultiPolygon<GeoJson3DGeographicCoordinates>(new GeoJsonMultiPolygonCoordinates<GeoJson3DGeographicCoordinates>(list));
        }

        public static MultiPolygon GetAranMultiPolygon(GeoJsonMultiPolygon<GeoJson3DGeographicCoordinates> geoJsonMultiPolygon)
        {
            if (geoJsonMultiPolygon == null)
                return null;

            var aranMultiPolygon = new MultiPolygon();

            foreach (var polygon in geoJsonMultiPolygon.Coordinates.Polygons)
            {
                var aranPolygon = new Polygon();

                var aranExteriorRing = new Ring();
                foreach (var position in polygon.Exterior.Positions)
                {
                    aranExteriorRing.Add(GetAranPoint(position));
                }
                aranPolygon.ExteriorRing = aranExteriorRing;

                foreach (var holes in polygon.Holes)
                {
                    var aranInterionrRing = new Ring();
                    foreach (var position in holes.Positions)
                    {
                        aranInterionrRing.Add(GetAranPoint(position));
                    }
                    aranPolygon.InteriorRingList.Add(aranInterionrRing);
                }

                aranMultiPolygon.Add(aranPolygon);
            }

            return aranMultiPolygon;
        }
    }
}
