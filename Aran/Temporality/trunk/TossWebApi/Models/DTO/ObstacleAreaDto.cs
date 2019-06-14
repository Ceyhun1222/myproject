using Aran.Aim.Enums;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace TossWebApi.Models.DTO
{
    public class ObstacleAreaDto
    {
        public Guid Identifier { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CodeObstacleArea Type { get; set; }
        
        public MultiPolygon Geo { get; set; }

        public string RunwayDesignator { get; set; }
    }
}