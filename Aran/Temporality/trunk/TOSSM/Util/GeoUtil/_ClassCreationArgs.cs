using System.Collections.Generic;
using Aran.Aim;
using ESRI.ArcGIS.Geodatabase;

namespace TOSSM.Util.GeoUtil
{
    public class ClassCreationArgs
    {
        public ClassCreationArgs Parent;
        public List<IField> Fields=new List<IField>();
        
        public FeatureType FeatureType { get; set; }

        public string ClassName { get; set; }
        public string ClassAlias { get; set; }

        public override string ToString()
        {
            return ClassName;
        }

        public string SimpleName { get; set; }

        public string PathName { get; set; }
    }
}