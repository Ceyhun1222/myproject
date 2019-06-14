using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Delta.Model
{
    public abstract class DesigningLayerBase
    {
        public Aran.Geometries.Geometry Geo { get; set; }
        public string Name { get; set; }

        public string Designer { get; set; }
        public DateTime InputDate { get; set; }
        public Guid FeatIdentifier { get; set; }
    }
}
