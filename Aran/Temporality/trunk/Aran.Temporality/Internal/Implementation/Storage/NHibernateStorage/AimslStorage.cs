using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class AimslStorage : CrudStorageTemplate<AimslOperation>, IAimslStorage
    {
        public int CreateOperation(AimslOperation aimsloperation)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var operation = session.CreateCriteria(typeof(AimslOperation)).Add(Restrictions.Eq("JobId", aimsloperation.JobId)).UniqueResult<AimslOperation>();
                        if (operation != null)
                        {
                            //operation with same uuid exists
                            return -1;
                        }
                        aimsloperation.Id = 0;
                        var id = session.Save(aimsloperation);
                        transaction.Commit();

                        return (int)id;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(typeof(AimslStorage)).Error(ex, $"Error on creating aimsl operation. JobId:{aimsloperation.JobId}");
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                    return -1;
                }
            }
        }

        public int CreateOperation(string jobId, string fileName, DateTime creationTime)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var operation = session.CreateCriteria(typeof(AimslOperation)).Add(Restrictions.Eq("JobId", jobId)).UniqueResult<AimslOperation>();
                        if (operation != null)
                        {
                            //operation with same uuid exists
                            return -1;
                        }
                        operation = new AimslOperation { JobId = jobId, FileName = fileName, CreationTime = creationTime, LastChangeTime = creationTime };
                        var id = session.Save(operation);
                        transaction.Commit();

                        return (int)id;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(typeof(AimslStorage)).Error(ex, $"Error on creating aimsl operation. JobId:{jobId}");
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                    return -1;
                }
            }
        }


        public bool AddPullPoint(int id, string pullPoint)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var operation = session.Get<AimslOperation>(id);
                if (operation != null)
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        operation.PullPoint = pullPoint;
                        session.Update(operation);
                        transaction.Commit();
                        return true;
                    }
                }
                return false;
            }
        }

        public bool AddSubscription(int id, string subscription)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var operation = session.Get<AimslOperation>(id);
                if (operation != null)
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        operation.Subscription = subscription;
                        session.Update(operation);
                        transaction.Commit();
                        return true;
                    }
                }
                return false;
            }
        }

        public bool AppendMesages(int id, List<string> messages, string status, DateTime lastchangeTime, bool closed)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var operation = session.Get<AimslOperation>(id);
                if (operation != null)
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        operation.Status = status;
                        operation.LastChangeTime = lastchangeTime;

                        StringBuilder messageString = new StringBuilder("");
                        foreach (var message in messages)
                        {
                            messageString.AppendLine(message);
                        }

                        operation.Messages = operation.Messages + messageString;
                        if (closed)
                            operation.InternalStatus = AimslOperationStatusType.Closed;
                        session.Update(operation);
                        transaction.Commit();
                        return true;
                    }
                }
                return false;
            }
        }

        public bool ChangeStatus(int id, string status, string description, bool closed)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var operation = session.Get<AimslOperation>(id);
                if (operation != null)
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        operation.Status = status;
                        operation.Description = description;
                        if (closed)
                            operation.InternalStatus = AimslOperationStatusType.Closed;
                        session.Update(operation);
                        transaction.Commit();
                        return true;
                    }
                }
                return false;
            }
        }

        public bool Destroy(int id, bool timeout = false)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var operation = session.Get<AimslOperation>(id);
                if (operation != null)
                {
                    using (var transaction = session.BeginTransaction())
                    {

                        operation.InternalStatus = AimslOperationStatusType.Destroyed;
                        if(timeout) operation.Status = "TIMEOUT";
                        session.Update(operation);
                        transaction.Commit();
                        return true;
                    }
                }
                return false;
            }
        }

        public IList<AimslOperation> GetAllActiveAimslOperations()
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {

                try
                {
                    return session.CreateCriteria(typeof(AimslOperation)).Add(!Restrictions.Eq("InternalStatus", AimslOperationStatusType.Destroyed)).List<AimslOperation>();
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(typeof(AimslStorage)).Error(ex, ex.Message);
                    return new List<AimslOperation>();
                }

            }
        }
    }
}