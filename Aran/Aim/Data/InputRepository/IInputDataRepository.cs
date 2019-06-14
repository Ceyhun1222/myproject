using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Aran.Aim.Features;

namespace Aran.Aim.Data.InputRepository
{
    public interface IInputDataRepository
    {
        ConcurrentDictionary<Guid, Feature> FeatureList { get; }
        string ProjectName { get; }

        void AddFeature(Feature feat);
        void AddFeatures(List<Feature> featList);
        List<Feature> GetFeatures(FeatureType featType);
        void ToXml(DateTime effectiveDate, string fileName);
        void ToXml(DateTime effectiveDate, string fileName, FeatureType featType);
        byte[] ToXml(DateTime effectiveDate);
        byte[] ToXml(DateTime effectiveDate, FeatureType featType);
    }
}