using System;
using Aran.Temporality.Common.Interface;

namespace Aran.Temporality.Common.OperationResult
{
    //TODO: rename
#warning rename
    public class FeatureDependencyException : Exception
    {
        public FeatureDependencyException(string desc):base(desc)
        {
        }

        public IFeatureId SourceFeatureId { get; set; }
        public IFeatureId TargetFeatureId { get; set; }
    }
}
