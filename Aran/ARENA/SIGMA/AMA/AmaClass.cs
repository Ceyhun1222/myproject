using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geometry;

namespace SigmaChart
{
    public class AmaClass
    {
        public IPoint CenterPoint { get; set; }
        public double Elevation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
