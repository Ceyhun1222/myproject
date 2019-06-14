using System;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.MetaData;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IFeatureLifeTimeStorage
    {
        void ApplyFeatureLifeTime(IFeatureId featureId, DateTime start, DateTime? end);
        TimeSlice GetFeatureLifeTime(IFeatureId featureId);
        void DeleteFeature(IFeatureId featureId);
        void DeleteFeatureLifeTimesByWorkPackage(int workPackageId);
        void CommitWorkPackage(int workPackageId);
    }
}
