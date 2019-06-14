using System;
using System.Linq;
using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.Geometries;
using GeoJSON.Net.Geometry;

namespace ObstacleCalculator.Domain.Utils
{
    public class GeoJsonToJtsGeo
    {
        private readonly IMathTransform _geoToPrjTransform;

        /// <exception cref="ArgumentNullException"><paramref name="geoToPrjTransform"/> is <see langword="null"/></exception>
        public GeoJsonToJtsGeo(IMathTransform geoToPrjTransform)
        {
            _geoToPrjTransform = geoToPrjTransform;
            if (_geoToPrjTransform==null)
                throw new ArgumentNullException(nameof(geoToPrjTransform));
        }
        public GeoAPI.Geometries.IPolygon ToPolygonPrj(LineString lnStringGeo)
        {
            var coords = new GeoAPI.Geometries.Coordinate[lnStringGeo.Coordinates.Count];
            int i = 0;
            lnStringGeo.Coordinates.ToList().ForEach(coord =>
            {
                coords[i] = _geoToPrjTransform.Transform(new GeoAPI.Geometries.Coordinate(coord.Longitude, coord.Latitude));
                if (coord.Altitude.HasValue)
                    coords[i].Z = coord.Altitude.Value;
                i++;
            });

            var jtsRing = new NetTopologySuite.Geometries.LinearRing(coords);
            var resultPoly = new NetTopologySuite.Geometries.Polygon(jtsRing);
            return resultPoly;
        }

        public GeoAPI.Geometries.ILinearRing ToLinearRing(LineString linearRing)
        {
            int i = 0;
            var coords = new GeoAPI.Geometries.Coordinate[linearRing.Coordinates.Count];
            linearRing.Coordinates.ToList()
                .ForEach(coord =>
                {
                    coords[i] =
                        _geoToPrjTransform.Transform(
                            new GeoAPI.Geometries.Coordinate(coord.Longitude, coord.Latitude));
                    if (coord.Altitude.HasValue)
                        coords[i].Z = coord.Altitude.Value;
                    i++;
                });
            return new NetTopologySuite.Geometries.LinearRing(coords);
        }

        public GeoAPI.Geometries.IPolygon ToPolygonPrj(Polygon polyGeo)
        {
            ILinearRing jtsExteriorRing = null;

            ILinearRing[] jtsInteriorHoles = null;
            if (polyGeo.Coordinates.Count > 1)
                jtsInteriorHoles = new ILinearRing[polyGeo.Coordinates.Count - 1];

            for (int i = 0; i < polyGeo.Coordinates.Count; i++)
            {
                var jtsRing = ToLinearRing(polyGeo.Coordinates[i]);
                if (i == 0)
                    jtsExteriorRing = jtsRing;
                else
                    jtsInteriorHoles[i - 1] = jtsRing;
            }

            var resultPoly = new NetTopologySuite.Geometries.Polygon(jtsExteriorRing, jtsInteriorHoles);
            return resultPoly;
        }

        public GeoAPI.Geometries.IMultiPolygon ToMultiPolygonPrj(MultiPolygon lnStringGeo)
        {
            //
            IPolygon[] resultPolyArray = new IPolygon[lnStringGeo.Coordinates.Count];
            int i = 0;
            foreach (var poly in lnStringGeo.Coordinates)
            {
                resultPolyArray[i++] = ToPolygonPrj(poly);
            }
            return new NetTopologySuite.Geometries.MultiPolygon(resultPolyArray);
        }
    }
}
