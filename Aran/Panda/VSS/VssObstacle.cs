using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ag = Aran.Geometries;

namespace Aran.PANDA.Vss
{
    public class VssObstacle
    {
        public Guid Identifier { get; set; }
        public string Name { get; set; }
        public Ag.Geometry LocationPrj { get; set; }
        public double Elevation { get; set; }
        public double Height { get; set; }

        public double HSurface { get; set; }
        public double VssX { get; set; }
        public double HPenetrate { get; set; }
        public double? NeededSlope { get; set; }
    }
}
