using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Delta.Model
{
    public class DesigningSegment:DesigningLayerBase
    {
        public string CodeTypeSegPath { get; set; }
        public double ValueTrueTrack { get; set; }
        public double ValueReverseTrueTrack { get; set; }
        public string UomDist { get; set; }
        public double ValLen { get; set; }
        public string WptStart { get; set; }
        public string WptEnd { get; set; }
        public string RouteName { get; set; }
    }
}
