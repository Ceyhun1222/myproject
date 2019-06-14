using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Exceptions;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class EventConsistencyStorage : CrudStorageTemplate<EventConsistency>, IEventConsistencyStorage
    {
        public int Save(EventConsistency eventConsistency)
        {
            var c = eventConsistency;
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var eventConsistencies = session.QueryOver<EventConsistency>()
                    .Where(x => x.RepositoryType == c.RepositoryType && x.StorageName == c.StorageName && 
                        x.WorkPackage == c.WorkPackage && x.FeatureType == c.FeatureType &&
                        x.Identifier == c.Identifier && x.Interpretation == c.Interpretation &&
                        x.SequenceNumber == c.SequenceNumber && x.CorrectionNumber == c.CorrectionNumber)
                    .RowCount();

                if (eventConsistencies > 0)
                    throw new OperationException("Unique constraints violated");
            }

            return CreateEntity(c);
        }

        public string Get(EventConsistency eventConsistency)
        {
            var c = eventConsistency;
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var eventConsistencies = session.QueryOver<EventConsistency>()
                    .Where(x => x.RepositoryType == c.RepositoryType && x.StorageName == c.StorageName &&
                        x.WorkPackage == c.WorkPackage && x.FeatureType == c.FeatureType &&
                        x.Identifier == c.Identifier && x.Interpretation == c.Interpretation &&
                        x.SequenceNumber == c.SequenceNumber && x.CorrectionNumber == c.CorrectionNumber)
                    .List().OrderByDescending(x => x.CalculationDate);

                return eventConsistencies.FirstOrDefault()?.Hash;
            }
        }

        public List<EventConsistency> Get(RepositoryType repositoryType, string storageName, int workPackage)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var eventConsistencies = session.QueryOver<EventConsistency>()
                    .Where(x => x.RepositoryType == repositoryType && x.StorageName == storageName && x.WorkPackage == workPackage)
                    .List();

                return eventConsistencies.ToList();
            }
        }

        public bool Save(List<EventConsistency> eventConsistencies)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        foreach (var value in eventConsistencies)
                        {
                            session.Save(value);
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }

                        return false;
                    }
                }
            }

            return true;
        }

        public void Clear(RepositoryType repositoryType, string storageName)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var eventConsistencies = session.QueryOver<EventConsistency>()
                            .Where(x => x.RepositoryType == repositoryType && x.StorageName == storageName)
                            .List();

                        foreach (var eventConsistency in eventConsistencies)
                        {
                            session.Delete(eventConsistency);
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
    }
}