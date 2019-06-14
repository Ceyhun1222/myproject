using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;
using ObstacleCalculator.Domain.Models;
using ObstacleCalculator.Domain.Utils;

namespace ObstacleCalculator.Domain.PlaneCalculator
{
    class Area3Surface : IPlaneSurface
    {
        private readonly SurfaceBase _surface;
        private readonly GeoJsonToJtsGeo _geoJsonToJtsGeo;
        private List<Plane> _planes;
        private readonly object _lockObject = new object();
        private const double SurfaceHeight = 0.5;

        public Area3Surface(SurfaceBase surface, GeoJsonToJtsGeo geoJsonToJtsGeo)
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

        public ObstacleReport CalculateObstacleReport(CalculationRequestParametrs calculationRequestParametrs)
        {
            double penetrate = double.MinValue;
            bool isInside = false;
            foreach (var plane in Planes)
            {
                if (!plane.Geo.Intersects(calculationRequestParametrs.Geo)) continue;

                penetrate = calculationRequestParametrs.Height - SurfaceHeight;
                isInside = true;
                break;
            }

            if (!isInside)
                return null;

            var obstacleReport = new ObstacleReport(_surface.Type.ToString(), penetrate,_surface.Identifier.ToString(),_surface.RunwayDesignator);
            
            return obstacleReport;
        }

        /// <exception cref="ArgumentNullException"><paramref name=""/> is <see langword="null"/></exception>
        /// <exception cref="InvalidCastException">Condition.</exception>
        public void CalculatePlanes()
        {
            lock (_lockObject)
            {
                _planes.Clear();

                if (_surface.Geo == null)
                    throw new ArgumentNullException(_surface.Type + " geo is empty " + _surface.Identifier);

                var geoPrj = _geoJsonToJtsGeo.ToMultiPolygonPrj(_surface.Geo);
                foreach (var poly in geoPrj)
                {
                    var polyPrj = poly as IPolygon;
                    if (polyPrj == null)
                        throw new InvalidCastException(
                            "Multipolygon's members should be in IPolygon format " + _surface.Type);
                    var plane = new Plane
                    {
                        Geo = polyPrj
                    };

                    _planes.Add(plane);
                }
            }
        }
    }
}
