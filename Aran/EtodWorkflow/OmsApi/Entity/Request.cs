using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace OmsApi.Entity
{
    public class Request : BaseEntity
    {
        public RequestType Type { get; set; }

        public Duration Duration { get; set; }

        public string Designator { get; set; }

        public DateTime BeginningDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Elevation { get; set; }

        public double Height { get; set; }

        public double VerticalAccuracy { get; set; }

        public double HorizontalAccuracy { get; set; }

        public string Geometry { get; set; }

        public bool Submitted { get; set; }

        public Guid Identifier { get; set; }

        public Byte[] Attachment { get; set; }

        public ObstructionType ObstructionType { get; set; }

        public string AirportName { get; set; }

        public string AirportId { get; set; }

        [Required]
        public ApplicationUser CreatedBy { get; set; }

        public Status Status { get; set; }

        public bool Checked { get; set; }

        public bool Submitted2Aixm { get; set; }

        public DateTime Submitted2AixmAt { get; set; }

        public long Submitted2AixmPrivateSlotId { get; set; }

        public string Submitted2AixmPrivateSlotName { get; set; }

        public long Submitted2AixmPublicSlotId { get; set; }

        public string Submitted2AixmPublicSlotName { get; set; }

        //public bool Submitted2AixmAsCompleted { get; set; }

        //public long Submitted2AixmAsCompletedPrivateSlotId { get; set; }
    }

    public enum RequestType
    {
        NewConstruction,
        Alteration,
        Existing
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

    public enum Duration
    {
        Permanent,
        Temporary
    }
}