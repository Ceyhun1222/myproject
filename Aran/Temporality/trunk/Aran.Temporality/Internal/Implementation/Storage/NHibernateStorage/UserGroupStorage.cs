using System;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class UserGroupStorage : CrudStorageTemplate<UserGroup>, IUserGroupStorage
    {
        public int CreateGroup(string groupName)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var userGroup = session.CreateCriteria(typeof(UserGroup)).Add(Restrictions.Eq("Name", groupName)).UniqueResult<UserGroup>();
                        if (userGroup != null)
                        {
                            //user with same name exists
                            return -1;
                        }

                        userGroup = new UserGroup { Name = groupName };
                        var id = session.Save(userGroup);
                        transaction.Commit();

                        return (int)id;
                    }
                    catch (Exception)
                    {
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                    return -1;
                }
            }
        }

        public bool SetGroupName(int id, string name)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var userGroup = session.Get<UserGroup>(id);

                        if (userGroup == null) return false;

                        var sameuser = session.CreateCriteria(typeof(UserGroup)).Add(Restrictions.Eq("Name", name)).UniqueResult<UserGroup>();
                        if (sameuser != null)
                        {
                            //user with same name exists
                            return false;
                        }

                        userGroup.Name = name;

                        session.Update(userGroup);
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
                    return false;
                }
            }
        }
    }
}
