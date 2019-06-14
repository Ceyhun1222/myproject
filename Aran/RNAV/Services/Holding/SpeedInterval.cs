using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holding
{
    public class SpeedInterval
    {
        public double  Min { get; set; }
        public double  Max { get; set; }
        public categories Category { get; set; }
        public flightPhase FlightPhase { get; set; }
        public double MaxAltitude { get; set; }
    }
}
