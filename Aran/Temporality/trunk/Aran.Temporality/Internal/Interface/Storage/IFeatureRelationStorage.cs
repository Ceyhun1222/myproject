using System.Collections.Generic;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Interface;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IFeatureRelationStorage
    {

        void DeleteRelationsByWorkPackage(int workPackageId);
        void CommitPackage(int workPackageId);

        IList<FeatureRelation> GetReverseRelations(IFeatureId id);
    }
}
