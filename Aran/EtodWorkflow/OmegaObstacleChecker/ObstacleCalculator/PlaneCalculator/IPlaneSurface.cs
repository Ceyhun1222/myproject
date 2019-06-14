using System;
using System.Collections.Generic;
using GeoAPI.Geometries;
using ObstacleCalculator.Domain.Models;
using ObstacleCalculator.Domain.Utils;

namespace ObstacleCalculator.Domain.PlaneCalculator
{
    public interface IPlaneSurface
    {
        List<Plane> Planes { get; }

        void CalculatePlanes();

        ObstacleReport CalculateObstacleReport(CalculationRequestParametrs calculationRequestParametrs);
    }
}
