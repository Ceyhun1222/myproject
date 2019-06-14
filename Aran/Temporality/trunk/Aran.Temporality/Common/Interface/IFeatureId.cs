using System;

namespace Aran.Temporality.Common.Interface
{
    public interface IFeatureId
    {
        Guid? Guid { get; }
        int FeatureTypeId { get; }
        int WorkPackage { get; }
    }
}