using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;
using ObstacleCalculator.Domain.Models;
using ObstacleCalculator.Domain.Utils;

namespace ObstacleCalculator.Domain.PlaneCalculator
{
    class Area2DSurface:IPlaneSurface
    {
        private readonly SurfaceBase _surface;
        private readonly object _lockObject =new object();
        private List<Plane> _planes;
        private readonly GeoJsonToJtsGeo _geoJsonToJtsGeo;
        private const double Area2DHeight = 100;

        public Area2DSurface(SurfaceBase surface,GeoJsonToJtsGeo geoJsonToJtsGeo)
        {
            _surface = surface;
            _geoJsonToJtsGeo = geoJsonToJtsGeo;

            if (_geoJsonToJtsGeo == null || _surface==null)
                throw new ArgumentNullException($"Area2D surface's inject parametr is null!");
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
                    };
                    _planes.Add(plane);
                }
            }
        }

        public ObstacleReport CalculateObstacleReport(CalculationRequestParametrs calculationRequestParametrs)
        {
            foreach (var plane in Planes)
            {
                if (!plane.Geo.Intersects(calculationRequestParametrs.Geo))
                    continue;

                return new ObstacleReport(_surface.Type.ToString(),calculationRequestParametrs.Height- Area2DHeight,_surface.Identifier.ToString(),_surface.RunwayDesignator);
            }
            return null;
        }
    }
}
