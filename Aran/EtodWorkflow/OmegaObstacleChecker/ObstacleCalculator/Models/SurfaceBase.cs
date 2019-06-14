using System;
using GeoJSON.Net.Geometry;

namespace ObstacleCalculator.Domain.Models
{
    public class SurfaceBase
    {
        public Guid Identifier { get; set; }

        public CodeObstacleArea Type { get; set; }

        public MultiPolygon Geo { get; set; }

        public string RunwayDesignator { get; set; }

    }
}
