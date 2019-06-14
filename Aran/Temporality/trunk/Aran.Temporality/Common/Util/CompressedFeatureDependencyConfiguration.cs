using System;
using Aran.Aim;

namespace Aran.Temporality.Common.Util
{
    [Serializable]
    public class CompressedFeatureDependencyConfiguration
    {
        public FeatureType FeatureType;
        public bool IsDirect;//related to parent node
        public string[] MandatoryLinks;
        public string[] OptionalLinks;
        public string[] MandatoryProperties;
        public string[] OptionalProperties;
        public CompressedFeatureDependencyConfiguration[] Children;
    }
}