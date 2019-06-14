using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Delta.Model
{
    public class DesigningArea:DesigningLayerBase
    {
        public string CodeType { get; set; }
        public double UpperLimit { get; set; }
        public double LowerLimit { get; set; }
        public string UomUpperLimit { get; set; }
        public string UomLowerLimit { get; set; }
    }
}
