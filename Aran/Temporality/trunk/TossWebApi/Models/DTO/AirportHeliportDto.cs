using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TossWebApi.Models.DTO
{
    public class AirportHeliportDto
    {
        public Guid Identifier { get; set; }

        public Point Geo { get; set; }

        public string Name { get; set; }

        public string Designator { get; set; }

        public string Elevation { get; set; }
    }
}