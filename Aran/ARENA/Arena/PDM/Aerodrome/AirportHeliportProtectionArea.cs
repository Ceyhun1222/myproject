using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDM
{
    public class AirportHeliportProtectionArea:PDMObject
    {
        public double? Width { get; set; }

        public UOM_DIST_HORZ Width_UOM { get; set; }

        public double? Length { get; set; }

        public UOM_DIST_HORZ Length_UOM { get; set; }

        public bool? Lighting { get; set; }

        public bool? ObstacleFree { get; set; }

        public SurfaceCharacteristics SurfaceProperties { get; set; }

        public string Annotation { get; set; }
    }
}
