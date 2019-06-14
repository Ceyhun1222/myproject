using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;

namespace ObstacleChecker.API.Dto
{
    public class AirportHeliportDto
    {
        public Guid Identifier { get; set; }

        public Point Geo { get; set; }

        public string Name { get; set; }

        public string Designator { get; set; }
    }
}
