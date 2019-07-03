using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.Model.Attribute
{
    [AttributeUsage(System.AttributeTargets.Property)]
    public class LinkedFeatureAttribute : System.Attribute
    {
        public int LinkedFeature;

        public LinkedFeatureAttribute(int linkedFeature)
        {
            LinkedFeature = linkedFeature;
        }

        public LinkedFeatureAttribute(FeatureType linkedFeature)
        {
            LinkedFeature = (int)linkedFeature;
        }

        public LinkedFeatureAttribute(ObjectType linkedFeature)
        {
            LinkedFeature = (int)linkedFeature;
        }
    }
}
