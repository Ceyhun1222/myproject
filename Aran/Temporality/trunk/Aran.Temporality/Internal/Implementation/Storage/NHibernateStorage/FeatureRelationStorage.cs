using System.Collections.Generic;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class FeatureRelationStorage : CrudStorageTemplate<FeatureRelation>, IFeatureRelationStorage
    {
        #region Overrides of IFeatureRelationStorage

        public void DeleteRelationsByWorkPackage(int workPackageId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {

                        session.Delete("from FeatureRelation where WorkPackage = " + workPackageId);
                        transaction.Commit();
                    }
                    catch
                    {
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
        }

        public void CommitPackage(int workPackageId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.CreateQuery("update FeatureRelation set WorkPackage=0 where WorkPackage = " + workPackageId).ExecuteUpdate();
                        transaction.Commit();
                    }
                    catch
                    {
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
        }

        public IList<FeatureRelation> GetReverseRelations(IFeatureId id)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                return session.CreateCriteria<FeatureRelation>().
                Add(Restrictions.Eq("WorkPackage", id.WorkPackage)).
                    //Add(Restrictions.Eq("FeatureTypeId", id.FeatureTypeId)).//this restriction is actually not needed
                Add(Restrictions.Eq("TargetGuid", id.Guid)).List<FeatureRelation>();
            }
        }

        #endregion
    }
}
