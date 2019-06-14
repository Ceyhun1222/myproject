using GeoJSON.Net;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class TossVerticalStructureDto
    {
        public Guid Identifier { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public double? Elevation { get; set;}

        public double? VerticalAccuracy{get; set; }

        public double? HorizontalAccuracy { get; set; }

        [JsonConverter(typeof(GeoJsonConverter))]
        public IGeoJSONObject Geo { get; set; }
    }
}
