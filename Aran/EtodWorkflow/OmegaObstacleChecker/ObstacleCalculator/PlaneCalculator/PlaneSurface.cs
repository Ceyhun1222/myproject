using System;
using System.Collections.Generic;
using GeoAPI.Geometries;
using ObstacleCalculator.Domain.Models;
using ObstacleCalculator.Domain.Utils;

namespace ObstacleCalculator.Domain.PlaneCalculator
{
    public class PlaneSurface : IPlaneSurface
    {
        private readonly object _lockObject = new object();
        private readonly SurfaceBase _surface;
        private readonly GeoJsonToJtsGeo _geoJsonToJtsGeo;
        private List<Plane> _planes;

        /// <exception cref="ArgumentNullException"><paramref name="surface"/> is <see langword="null"/></exception>
        public PlaneSurface(SurfaceBase surface, GeoJsonToJtsGeo geoJsonToJtsGeo)
        {
            _surface = surface;
            if (_surface == null)
                throw new ArgumentNullException(nameof(surface));

            _geoJsonToJtsGeo = geoJsonToJtsGeo;
            if (_geoJsonToJtsGeo == null)
                throw new ArgumentNullException(nameof(geoJsonToJtsGeo));
        }

        /// <exception cref="InvalidCastException" accessor="get">Condition.</exception>
        public List<Plane> Planes {
            get
            {
                lock (_lockObject)
                {
                    if (_planes == null)
                    {
                        _planes = new List<Plane>();
                        CalculatePlanes();
                    }

                    return _planes;
                }
            }
        }

        /// <exception cref="InvalidCastException">Condition.</exception>
        /// <exception cref="ArgumentNullException"><paramref name=""/> is <see langword="null"/></exception>
        public void CalculatePlanes()
        {
            lock (_lockObject)
            {
                _planes.Clear();

                if (_surface.Geo == null)
                    throw new ArgumentNullException(_surface.Type + " geo is empty " + _surface.Identifier);

                var geoPrj = _geoJsonToJtsGeo.ToMultiPolygonPrj(_surface.Geo);

                for (int i = 0; i < geoPrj.Count; i++)
                {
                    if (geoPrj.Geometries[i] is IPolygon polyPrj)
                    {
                        var plane = new Plane
                        {
                            Geo = polyPrj,
                            Param = LineStringToPlaneParam(polyPrj.ExteriorRing)
                        };
                        _planes.Add(plane);
                    }
                    else
                        throw new InvalidCastException(
                            "Multipolygon's members should be in IPolygon format " + _surface.Type);
                }
            }
        }

        public virtual ObstacleReport CalculateObstacleReport(CalculationRequestParametrs calculationRequestParametrs)
        {
            var maxPenetrate = GetPenetrate(calculationRequestParametrs);
            if (Math.Abs(maxPenetrate - double.MinValue) < 0.01)
                return null;

            var obstacleReport = new ObstacleReport(_surface.Type.ToString(), maxPenetrate,_surface.Identifier.ToString(),_surface.RunwayDesignator);
            
            return obstacleReport;
        }

        private double GetPenetrate(CalculationRequestParametrs calculationRequestParametrs)
        {
            double maxPenetrate = double.MinValue;
            foreach (var plane in Planes)
            {
                if (plane.Geo.Intersects(calculationRequestParametrs.Geo))
                {
                    var intersectGeo = plane.Geo.Intersection(calculationRequestParametrs.Geo);
                    if (intersectGeo == null || intersectGeo.IsEmpty)
                        continue;

                    foreach (var intersectCoordinate in intersectGeo.Coordinates)
                    {
                        var surfaceElevation = plane.Param.GetZ(intersectCoordinate);
                        double penetrate = calculationRequestParametrs.Elevation - surfaceElevation+calculationRequestParametrs.VerticalAccuracy;
                        maxPenetrate = Math.Max(penetrate, maxPenetrate);
                    }
                }
            }
            return maxPenetrate;
        }

        private PlaneParam LineStringToPlaneParam(ILineString lineString)
        {
            if (lineString == null)
                throw new ArgumentNullException($"Linestring is empty!");

            var ptPrjList = new List<Coordinate>();
            for (int i = 0; i < Math.Min(lineString.Coordinates.Length, 3); i++)
            {
                var geoPoint = lineString.Coordinates[i];

                ptPrjList.Add(new GeoAPI.Geometries.Coordinate(geoPoint.X, geoPoint.Y, geoPoint.Z));
            }
            return PlaneParamCalculator.CalcPlaneParam(ptPrjList[0], ptPrjList[1], ptPrjList[2]);
        }
    }
}
