using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Delta.Model
{
    public class IntersectItemType
    {
        public IntersectItemType()
        {
            IsArea = true;
        }
        public Enums.FeatureType FeatureType { get; set; }
        public string Header { get; set; }
        public bool IsArea{ get; set; }
    }
}
