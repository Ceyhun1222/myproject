using System;
using System.Collections;
using System.Collections.Generic;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Internal.Abstract.Storage;
using Aran.Temporality.Internal.Interface;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Persister.Entity;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class CrudStorageTemplate<T> : AbstractNHibernateStorage, ICrudStorage<T> where T : class, INHibernateEntity
    {
        public IEnumerator GetEnumerator()
        {
            return null;
        }


        public void DeleteAll()
        {
            //using (var session = Repository.SessionFactory.OpenSession())
            //{
            //    using (var transaction = session.BeginTransaction())
            //    {
            //        var deleteQuery = "from " + typeof (T).Name + " e";
            //        session.Delete(deleteQuery);
            //        session.Flush();
            //        transaction.Commit();
            //    }
            //}


            var metadata = Repository.SessionFactory.GetClassMetadata(typeof(T)) as AbstractEntityPersister;
            var table = metadata.TableName;

            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    string deleteAll = $"DELETE FROM \"{table}\"";
                    session.CreateSQLQuery(deleteAll).ExecuteUpdate();

                    transaction.Commit();
                }
            }
        }

        public int CreateEntity(T entity)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                        var id = (int)session.Save(entity);
                        transaction.Commit();
                        return id;
                }
            }
        }

        public bool DeleteEntityById(int id)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var wp = session.Get<T>(id);

                        if (wp != null)
                        {
                            session.Delete(wp);
                            transaction.Commit();
                            return true;
                        }
                    }
                    catch(Exception exception)
                    {
                        LogManager.GetLogger(typeof(CrudStorageTemplate<T>)).Error(exception, "Error on deleting entity by id.");
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                    return false;
                }
            }
        }


        public IList<T> GetAllEntities()
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                return session.CreateCriteria(typeof(T)).List<T>();
            }
        }

        public T GetEntityById(int id)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                return session.Get<T>(id);
            }
        }
    }
}
