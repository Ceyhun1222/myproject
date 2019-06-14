using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Delta.Model
{
    public class AddedPointInfo
    {
        public ESRI.ArcGIS.Geometry.IPoint Point { get; set; }
        public bool IsCCW { get; set; }
        public double R { get; set; }
        public bool IsMinor { get; set; }
        public Enums.DrawedAreaEnum DrawedEnum { get; set; }
    }
}
