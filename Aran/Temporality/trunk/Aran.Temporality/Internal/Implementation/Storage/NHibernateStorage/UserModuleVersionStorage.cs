using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class UserModuleVersionStorage : CrudStorageTemplate<UserModuleVersion>, IUserModuleVersionStorage
    {
        public bool UpdateUserModuleVersion(UserModuleVersion userModuleVersion)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var old = session.CreateCriteria(typeof(UserModuleVersion)).
                            Add(Restrictions.Eq("User", userModuleVersion.User)).
                            Add(Restrictions.Eq("Module", userModuleVersion.Module)).
                            UniqueResult<UserModuleVersion>();

                        if (old == null)
                        {
                            session.Save(userModuleVersion);
                        }
                        else
                        {
                            old.ActualVersion = userModuleVersion.ActualVersion;
                            session.Update(old);
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch(Exception)
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

        public IList<UserModuleVersion> GetUserModuleVersions(int userId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                try
                {
                    return session.CreateCriteria(typeof (UserModuleVersion)).
                        Add(Restrictions.Eq("User", new User {Id = userId})).
                        List<UserModuleVersion>();
                }
                catch
                {
                    return new List<UserModuleVersion>();
                }
            }
        }
    }
}
