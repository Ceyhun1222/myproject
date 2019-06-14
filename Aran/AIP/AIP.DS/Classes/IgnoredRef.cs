using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;

namespace AIP.DataSet.Classes
{
    public class IgnoredRef
    {
        public FeatureType MainFeature { get; set; }
        public Dictionary<FeatureType, List<FeatureType>> IgnoredRefs { get; set; }
        

    }
}
