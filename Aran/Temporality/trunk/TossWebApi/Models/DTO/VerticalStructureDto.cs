using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aran.Aim.Enums;
using GeoJSON.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TossWebApi.Models.DTO
{
    public class VerticalStructureDto
    {
        public Guid Identifier { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CodeVerticalStructure? Type { get; set; }

        public GeoJSONObject Geo { get; set; }

        public double? Elevation { get; set; }

        public double? HorizontalAccuracy { get; set; }

        public double? VerticalAccuracy { get; set; }
    }
}