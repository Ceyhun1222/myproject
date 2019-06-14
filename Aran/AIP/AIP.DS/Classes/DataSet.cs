using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AIP.DataSet.Lib;
using Aran.Aim;

namespace AIP.DataSet.Classes
{
    
    public class DataSetRule
    {
        public FeatureType MainFeatureType { get; set; }

        public List<FeatureType> RefFeatureTypes { get; set; }
        public List<FeatureType> SubFeatureTypes { get; set; }
    }
    
    
}
