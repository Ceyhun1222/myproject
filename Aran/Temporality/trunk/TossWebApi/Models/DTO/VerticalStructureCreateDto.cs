using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeoJSON.Net;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using MongoDB.Driver.GeoJsonObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TossWebApi.Models.DTO
{
    public class VerticalStructureCreateDto
    {
        public List<Guid> ObstacleAreadIdList { get; set; }

        public Guid Identifier { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ObstructionType Type { get; set; }

        public double Elevation { get; set; }

        public double Height { get; set; }

        public double VerticalAccuracy { get; set; }

        public double HorizontalAccuracy { get; set; }

        [JsonConverter(typeof(GeometryConverter))]
        public IGeometryObject Geometry { get; set; }

        // Temporary unusable

        public bool IsTemporary { get; set; }

        public DateTime BeginEffectiveDate { get; set; }

        public DateTime EndEffectiveDate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Submit2AixmAs SubmitConstructionType { get; set; }
    }

    public enum ObstructionType
    {
        Building,
        Antenna,
        Tower,
        Crane,
        Pole,
        Monument,
        Other
    }

    public enum Submit2AixmAs
    {
        Test,
        Production
    }
}