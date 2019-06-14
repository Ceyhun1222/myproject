using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Temporality.Common.Interface;

namespace Aran.Temporality.Exception
{
    public class FeatureDependancyException : System.Exception
    {
        public FeatureDependancyException(string desc):base(desc)
        {
        }

        public IFeatureId SourceFeatureId { get; set; }
        public IFeatureId TargetFeatureId { get; set; }
    }
}
