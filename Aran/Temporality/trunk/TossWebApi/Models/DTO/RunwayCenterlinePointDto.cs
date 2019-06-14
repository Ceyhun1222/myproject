using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aran.Aim.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TossWebApi.Models.DTO
{
    public class RunwayCenterlinePointDto
    {
        public Guid Identifier { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CodeRunwayPointRole Role { get; set; }

        public Point Geo { get; set; }

        public string RunwayDirectionName { get; set; }

        public string Elevation { get; set; }
    }
}