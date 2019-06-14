using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.Models
{
    public class VerticalStructure
    {
        public string Name { get; set; }

        public ESRI.ArcGIS.Geometry.IGeometry Geo { get; set; }

        public double Elevation { get; set; }
        public IGeometry GeoPrj { get; internal set; }
    }
}
