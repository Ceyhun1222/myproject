using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;
using ObstacleCalculator.Domain.Models;
using ObstacleCalculator.Domain.Utils;

namespace ObstacleCalculator.Domain.PlaneCalculator
{
    class Area2CSurface:IPlaneSurface
    {
        private readonly object _lockObject = new object();
        private readonly SurfaceBase _surface;
        private readonly GeoJsonToJtsGeo _geoJsonToJtsGeo;
        private List<Plane> _planes;

        /// <exception cref="ArgumentNullException"><paramref name="surface"/> is <see langword="null"/></exception>
        public Area2CSurface(SurfaceBase surface, GeoJsonToJtsGeo geoJsonToJtsGeo)
        {
            _surface = surface;
            if (_surface == null)
                throw new ArgumentNullException(nameof(surface));

            _geoJsonToJtsGeo = geoJsonToJtsGeo;
            if (_geoJsonToJtsGeo == null)
                throw new ArgumentNullException(nameof(geoJsonToJtsGeo));
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
                foreach (var poly in geoPrj)
                {
                    var polyPrj = poly as IPolygon;
                    if (polyPrj == null)
                        throw new InvalidCastException(
                            "Multipolygon's members should be in IPolygon format " + _surface.Type);
                    var plane = new Plane
                    {
                        Geo = polyPrj,
                        //Param = LineStringToPlaneParam(polyPrj.ExteriorRing)
                    };
                    _planes.Add(plane);
                }
            }
        }

        public ObstacleReport CalculateObstacleReport(CalculationRequestParametrs calculationRequestParametrs)
        {
            return null;
        }
    }
}
