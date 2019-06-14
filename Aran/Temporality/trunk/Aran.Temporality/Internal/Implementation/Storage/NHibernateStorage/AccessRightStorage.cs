using System.Collections.Generic;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class AccessRightStorage : CrudStorageTemplate<AccessRightZipped>, IAccessRightStorage
    {
        #region Overrides of AbstractAccessRightStorage

        public IList<AccessRightZipped> GetUserRights(int userId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                return session.CreateCriteria(typeof (AccessRightZipped)).
                                               Add(Restrictions.Eq("UserGroupId", userId)).List<AccessRightZipped>();
            }
        }

        public bool SetUserRights(AccessRightZipped rightZipped)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var old = session.CreateCriteria(typeof (AccessRightZipped)).
                            Add(Restrictions.Eq("UserGroupId", rightZipped.UserGroupId)).
                            Add(Restrictions.Eq("StorageId", rightZipped.StorageId)).
                            Add(Restrictions.Eq("WorkPackage", rightZipped.WorkPackage)).
                            UniqueResult<AccessRightZipped>();
                        if (old == null)
                        {
                            session.Save(rightZipped);
                        }
                        else
                        {
                            old.OperationFlag = rightZipped.OperationFlag;
                            AccessRightUtil.SetZippedData(old, rightZipped);
                            session.Update(old);
                        }
                        transaction.Commit();
                        return true;
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
            return false;
        }

        public AccessRightZipped GetUserRights(int userId, int storageId, int wp)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                return (AccessRightZipped)session.CreateCriteria(typeof(AccessRightZipped)).
                                               Add(Restrictions.Eq("UserGroupId", userId)).
                                               Add(Restrictions.Eq("StorageId", storageId)).
                                               Add(Restrictions.Eq("WorkPackage", wp)).
                                               UniqueResult();
            }
        }

        public void DeleteAccessRightsByWorkPackage(int packageId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Delete("from AccessRightZipped where WorkPackage = " + packageId);
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

        #endregion
    }
}
