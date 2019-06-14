using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Delta.Model
{
    public class DesigningBuffer:DesigningLayerBase
    {
        public string CodeType { get; set; }
        public double BufferWidth { get; set; }
        public string UOMBufferWidth { get; set; }
        public double UpperLimit { get; set; }
        public double LowerLimit { get; set; }
        public string UomUpperLimit { get; set; }
        public string UomLowerLimit { get; set; }
        public string MarkerLayer { get; set; }
        public string MarkerObjectName { get; set; }
        public string Code_Type { get; set; }
    }
}
