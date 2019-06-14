using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDM
{
    [Serializable()]
    public class FeatureMetadata
    {
        public FeatureMetadata()
        {

        }
        public double? HorizontalResolution { get; set; } 

        public double? VerticalResolution { get; set; }
    }
}
