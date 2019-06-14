using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;

namespace ObstacleCalculator.Domain.Models
{
    public class CalculationRequestParametrs
    {
        public double Elevation { get; set; }

        public double Height { get; set; }

        public IGeometry Geo { get; set; }

        public double VerticalAccuracy { get; set; }

        public double HorizontalAccuracy { get; set; }
    }
}
