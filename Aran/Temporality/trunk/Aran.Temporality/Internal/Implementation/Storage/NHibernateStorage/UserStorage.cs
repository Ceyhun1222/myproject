using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Internal.Interface.Storage;
using Aran.Temporality.Internal.Remote.Util;
using FluentNHibernate.Utils;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class UserStorage : CrudStorageTemplate<User>, IUserStorage
    {
        #region Overrides of AbstractUserStorage

        public User Login(int userId, string password)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var user = session.Get<User>(userId);

                if (user == null) return null;

                var userPassword = user.Password ?? "";
                password = password ?? "";

                if (userPassword != password) return null;

                if (user.RoleFlag <= 0) return null;

                if (WcfOperationContext.Current!=null)
                {
                    WcfOperationContext.Current.Items["user"] = user;
                }

                var clone=user.DeepClone();

                return clone;
            }
        }

        public int CreateUser(string name)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var user = session.CreateCriteria(typeof(User)).Add(Restrictions.Eq("Name", name)).UniqueResult<User>();
                        if (user != null)
                        {
                            //user with same name exists
                            return -1;
                        }

                        user = new User { Name = name, Password = "", RoleFlag = (int)UserRole.User };
                        var id=session.Save(user);
                        transaction.Commit();
                        
                        return (int)id;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(typeof(UserStorage)).Error(ex, $"Error on creating user. Username:{name}");
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                    return -1;
                }
            }
        }

        public int ResetPasswordById(int userId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var user = session.Get<User>(userId);

                        if (user == null) return -1;

                        user.Password = "";

                        session.Update(user);
                        transaction.Commit();

                        return user.Id;
                    }
                    catch(Exception ex)
                    {
                        LogManager.GetLogger(typeof(UserStorage)).Error(ex, $"Error on reseting user password. UserId: {userId}");
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                    return -1;
                }
            }
        }

        public bool DeleteUserById(int userId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var user = session.Get<User>(userId);
                        if (user!=null)
                        {
                            session.Delete(user);
                            transaction.Commit();
                            return true;
                        }
                        
                    }
                    catch(Exception ex)
                    {
                        LogManager.GetLogger(typeof(UserStorage)).Error(ex, $"Error on deleting user. UserId: {userId} ");
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                    return false;
                }
            }
        }

        public bool SetUserModules(int userId, int currentModuleFlag)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var user = session.Get<User>(userId);

                        if (user == null) return false;

                        user.ModuleFlag = currentModuleFlag;

                        session.Update(user);
                        transaction.Commit();

                        return true;
                    }
                    catch(Exception ex)
                    {
                        LogManager.GetLogger(typeof(UserStorage)).Error(ex, $"Error on setting user module. UserId: {userId}, moduleFlag {currentModuleFlag}");
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                    return false;
                }
            }
        }

        public bool SetUserRole(int userId, int role)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var user = session.Get<User>(userId);

                        if (user == null) return false;

                        user.RoleFlag = role;

                        session.Update(user);
                        transaction.Commit();

                        return true;
                    }
                    catch(Exception ex)
                    {
                        LogManager.GetLogger(typeof(UserStorage)).Error(ex, $"Error on setting user role. UserId: {userId}, role: {role}");
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                    return false;
                }
            }
        }

        public bool SetUserName(int userId, string name)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var user = session.Get<User>(userId);

                        if (user == null) return false;

                        var sameuser = session.CreateCriteria(typeof(User)).Add(Restrictions.Eq("Name", name)).UniqueResult<User>();
                        if (sameuser != null)
                        {
                            //user with same name exists
                            return false;
                        }

                        user.Name = name;

                        session.Update(user);
                        transaction.Commit();

                        return true;
                    }
                    catch(Exception ex)
                    {
                        LogManager.GetLogger(typeof(UserStorage)).Error(ex, $"Error on setting username. UserId: {userId}, name: {name}");
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                    return false;
                }
            }
        }


     

        public bool SetUserActiveSlot(int userId, PrivateSlot privateSlot)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var user = session.Get<User>(userId);

                        if (user == null) return false;

                        user.ActivePrivateSlot = privateSlot;

                        session.Update(user);
                        transaction.Commit();

                        return true;
                    }
                    catch(Exception ex)
                    {
                        LogManager.GetLogger(typeof(UserStorage)).Error(ex, $"Error on setting user active slot. UserId: {userId}, slotId: {privateSlot.Id}, slot name: {privateSlot.Name}");
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                    return false;
                }
            }
        }

        #endregion

        public bool ChangeMyPassword(int userId, string oldPassword, string newPassword)
        {
            //var identity = StorageService.CurrentIdentity();
            //if (identity == null) return false;
            //if (identity.Name != userId.ToString()) return false;

            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {

                        var user = session.Get<User>(userId);

                        if (user == null)
                        {
                            //user not found
                            return false;
                        }

                        if (!string.IsNullOrEmpty(oldPassword))
                        {
                            if (user.Password != oldPassword) return false;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(user.Password)) return false;
                        }

                        user.Password = newPassword;

                        session.Update(user);
                        transaction.Commit();
                        return true;
                    }
                    catch(Exception ex)
                    {
                        LogManager.GetLogger(typeof(UserStorage)).Error(ex, $"Error on reseting user password. UserId: {userId}");
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                        return false;
                    }
                }
            }
        }

        public IList<User> GetAll()
        {
            return GetAllEntities();
        }

        public IList<User> GetUsersOfPrivateSlot(int id)
        {
            try
            {
                using (var session = Repository.SessionFactory.OpenSession())
                {
                    return session.CreateCriteria(typeof(User)).Add(Restrictions.Eq("ActivePrivateSlot", new PrivateSlot { Id = id })).List<User>();
                }
            }
            catch(Exception ex)
            {
                LogManager.GetLogger(typeof(UserStorage)).Error(ex, $"Error on getting private slot list. UserId: {id}");
                return new List<User>();
            }
        }

        public bool SetUserGroup(int userId, int groupId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var user = session.Get<User>(userId);
                        if (user == null) return false;

                        user.UserGroup = groupId == -1 ? null : session.Get<UserGroup>(groupId);

                        session.Update(user);
                        transaction.Commit();

                        return true;
                    }
                    catch(Exception ex)
                    {
                        LogManager.GetLogger(typeof(UserStorage)).Error(ex, $"Error on setting user group. UserId: {userId}, groupId {groupId}");
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                    return false;
                }
            }
        }

        public bool DeleteGroupById(int groupId)
        {
            if (groupId <= 0) return true;

            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var group=session.Get<UserGroup>(groupId);
                        var userlist = session.CreateCriteria(typeof(User)).Add(Restrictions.Eq("UserGroup", group)).List<User>();
                        if (userlist != null)
                        {
                            foreach (var user in userlist)
                            {
                                user.UserGroup = null;
                                session.Update(user);
                            }
                        }

                        transaction.Commit();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(typeof(UserStorage)).Error(ex, $"Error on deleting group. GroupId {groupId}");
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
