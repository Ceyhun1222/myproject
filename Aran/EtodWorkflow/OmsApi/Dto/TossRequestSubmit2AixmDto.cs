using GeoJSON.Net;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;
using OmsApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class TossRequestSubmit2AixmDto
    {
        public Guid Identifier { get; set; }

        //public bool IsTemporary { get; set; }

        public DateTime BeginEffectiveDate { get; set; }

        public DateTime EndEffectiveDate { get; set; }

        public ObstructionType Type { get; set; }

        public double Elevation { get; set; }

        public double Height { get; set; }

        public double VerticalAccuracy { get; set; }

        public double HorizontalAccuracy { get; set; }

        [JsonConverter(typeof(GeoJsonConverter))]
        public IGeoJSONObject Geometry { get; set; }

        //public Submit2AixmAs SubmitConstructionType { get; set; }

        public IList<Guid> ObstacleAreadIdList { get; set; }

        //public long SlotId { get; set; }
    }

    public enum Submit2AixmAs
    {
        Test,
        Production
    }
}
