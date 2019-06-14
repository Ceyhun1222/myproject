using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using ObstacleCalculator.Domain.Models;
using ObstacleCalculator.Domain.Utils;

namespace ObstacleCalculator.Domain.PlaneCalculator
{
    class ConicalSurface:IPlaneSurface
    {
        private readonly SurfaceBase _surface;
        private readonly GeoJsonToJtsGeo _geoJsonToJtsGeo;
        private List<Plane> _planes;
        private readonly object _lockObject = new object();

        public ConicalSurface(SurfaceBase surface, GeoJsonToJtsGeo geoJsonToJtsGeo)
        {
            _surface = surface;
            _geoJsonToJtsGeo = geoJsonToJtsGeo;
        }

        public List<Plane> Planes
        {
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
                            Geo = polyPrj
                        };
                        _planes.Add(plane);
                    }
                    else
                        throw new InvalidCastException(
                            "Multipolygon's members should be in IPolygon format " + _surface.Type);
                }
            }
        }

        /// <exception cref="ArgumentNullException"><paramref name="Conical surface does not have interior ring"/> is <see langword="null"/></exception>
        public ObstacleReport CalculateObstacleReport(CalculationRequestParametrs calculationRequestParametrs)
        {
            var plane = Planes.FirstOrDefault();
            if (plane == null)
                return null;

            var exterior = plane.Geo.ExteriorRing;
            var interior = plane.Geo.InteriorRings.FirstOrDefault();
            if (interior==null)
                throw new ArgumentNullException("Conical surface must have interior ring!");

            if (!plane.Geo.Intersects(calculationRequestParametrs.Geo))
                return null;

            var slope = CalculateSlope(exterior, interior);

            var distanceToInterior = interior.Distance(calculationRequestParametrs.Geo);

            var elevInnerHor = interior.Coordinates.First().Z;

            double partSurfaceElevation = slope * distanceToInterior + elevInnerHor;
            double penetrate = calculationRequestParametrs.Elevation - partSurfaceElevation+calculationRequestParametrs.VerticalAccuracy;

            return new ObstacleReport(_surface.Type.ToString(),penetrate,_surface.Identifier.ToString(),_surface.RunwayDesignator);
        }

        private double CalculateSlope(ILineString exterior, ILineString interior)
        {
            var distanceToInterior = exterior.Distance(interior);

            var exteriorElevation = exterior.Coordinates.First().Z;

            var interiorElevation = interior.Coordinates.First().Z;

            return (exteriorElevation - interiorElevation) / distanceToInterior;
        }
    }
}
