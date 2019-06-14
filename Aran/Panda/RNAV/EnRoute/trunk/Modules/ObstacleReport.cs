using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Features;

namespace Aran.PANDA.RNAV.EnRoute.Modules
{
    public class ObstacleReport
    {
        public ObstacleReport(VerticalStructure vs,Side sideType)
        {
            Obstacle = vs;
            Ignored = true;
            SideType = sideType;
            UnicalName = vs.Name;
            TypeName = vs.Type.ToString();
        }

        public VerticalStructure Obstacle { get; private set; }
        public string UnicalName { get; set; }
        public string TypeName { get; set; }

        public Geometries.Geometry Geo { get; set; }
        public Aran.Geometries.Point Vertex { get; set; }
        public double Height { get; set; }
        public double HorAccuracy { get; set; }
        public double VerAccuracy { get; set; }

        public bool Prima { get; set; }
        public double SecCoeff { get; set; }

        public double Moc { get; set; }
        public double ReqH { get; set; }
        public bool Ignored { get; set; }

        public double Dist { get; set; }
        public double CLShift { get; set; }

        public Side SideType { get; set; }

    }
}
