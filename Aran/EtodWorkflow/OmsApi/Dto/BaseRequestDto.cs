using OmsApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class BaseRequestDto
    {
        public long Id { get; set; }

        public RequestType Type { get; set; }

        public string Designator { get; set; }

        public Duration Duration { get; set; }

        public ObstructionType ObstructionType { get; set; }
        
        //If obstruction is temporary then its endDate should be set
        public DateTime BeginningDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Elevation { get; set; }

        public double Height { get; set; }

        public double VerticalAccuracy { get; set; }

        public double HorizontalAccuracy { get; set; }

        public string AirportName { get; set; }

        public string AirportId { get; set; }
    }
}