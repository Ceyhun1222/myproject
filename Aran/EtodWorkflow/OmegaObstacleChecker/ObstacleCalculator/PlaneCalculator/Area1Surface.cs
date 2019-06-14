using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;
using ObstacleCalculator.Domain.Models;

namespace ObstacleCalculator.Domain.PlaneCalculator
{
    class Area1Surface:IPlaneSurface
    {
        private readonly SurfaceBase _surface;

        public Area1Surface(SurfaceBase surface)
        {
            _surface = surface;
            Planes = new List<Plane>();
        }
        public List<Plane> Planes { get; }
        public void CalculatePlanes()
        {
            
        }

        public ObstacleReport CalculateObstacleReport(CalculationRequestParametrs calculationRequestParametrs)
        {
            return new ObstacleReport(_surface.Type.ToString(), calculationRequestParametrs.Height- 100,_surface.Identifier.ToString(),_surface.RunwayDesignator);
        }
    }
}
