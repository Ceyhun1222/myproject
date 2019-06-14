using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Panda.VisualManoeuvring
{
    public class VM_TrackSegment_JSON
    {
        public string Name { get; set; }
        public double Length { get; set; }
        public string Description { get; set; }
        public double FlightElevation { get; set; }

        public double StartPointPrjX { get; set; }
        public double StartPointPrjY { get; set; }
        public double InitialDirectionDir { get; set; }
        public double DistanceTillDivergence { get; set; }
        public int DivergenceSide { get; set; }
        public double DivergenceAngleRad { get; set; }
        public double DistanceTillConvergence { get; set; }
        public int ConvergenceSide { get; set; }
        public double ConvergenceAngleRad { get; set; }
        public double DistanceTillEndPoint { get; set; }
        public double EndPointPrjX { get; set; }
        public double EndPointPrjY { get; set; }
        public double FinalDirectionDir { get; set; }
    }
}
