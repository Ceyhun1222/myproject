using System;
using Aran.Temporality.Common.Abstract.MetaData;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.MetaData;
using Aran.Temporality.Internal.Abstract.Storage;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class FeatureLifeTimeStorage : AbstractNHibernateStorage, IFeatureLifeTimeStorage
    {
        #region Implementation of IFeatureLifeTimeStorage

        private void ApplyFeatureLifeTimeInternal(ISession session, IFeatureId featureId, DateTime begin, DateTime? end)
        {
            if (featureId.Guid==null)
            {
                throw new Exception("feature id should not be null");
            }

            var related = GetById(session, featureId);

            if (related==null)
            {
                session.Save(new FeatureLifeTime
                                 {
                                     WorkPackage = featureId.WorkPackage,
                                     Guid = (Guid)featureId.Guid,
                                     BeginDate = begin,
                                     EndDate = end,
                                     FeatureTypeId = featureId.FeatureTypeId
                                 });
            }
            else
            {
                related.EndDate = end;
                related.BeginDate = begin;
                session.Update(related);
            }
        }


        private FeatureLifeTime GetById(ISession session,IFeatureId featureId)
        {
            var related = session.CreateCriteria(typeof(FeatureLifeTime)).
              Add(Restrictions.Eq("WorkPackage", featureId.WorkPackage)).
              Add(Restrictions.Eq("Guid", featureId.Guid)).
              List<FeatureLifeTime>();

            if (related == null) return null;
            if (related.Count == 0) return null;
            if (related.Count > 1) throw new Exception("not unique lifetime detected");
            return related[0];
        }


        public TimeSlice GetFeatureLifeTime(IFeatureId featureId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var related = GetById(session, featureId);
                return new TimeSlice(related.BeginDate,related.EndDate);
            }
        }

        public void DeleteFeature(IFeatureId featureId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Delete("from FeatureLifeTime where WorkPackage = " + featureId.WorkPackage+
                            " and Guid = '" + featureId.Guid+"'");
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



        public void ApplyFeatureLifeTime(AbstractMetaData meta)
        {
            if (meta.TimeSlice==null) throw new Exception("Meta should have not null TimeSlice");
            ApplyFeatureLifeTime(meta, meta.TimeSlice.BeginPosition, meta.TimeSlice.EndPosition);
        }

        public void ApplyFeatureLifeTime(IFeatureId featureId, DateTime start, DateTime? end)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        ApplyFeatureLifeTimeInternal(session, featureId, start, end);
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



        public void CommitWorkPackage(int workPackageId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                    try
                    {
                        var relatedData = session.CreateCriteria(typeof(FeatureLifeTime)).
                            Add(Restrictions.Eq("WorkPackage", workPackageId)).List<FeatureLifeTime>();

                        if (relatedData!=null)
                        {
                            foreach (var featureLifeTime in relatedData)
                            {
                                var featureId = new FeatureId
                                                    {
                                                        FeatureTypeId = featureLifeTime.FeatureTypeId,
                                                        Guid = featureLifeTime.Guid,
                                                        WorkPackage = featureLifeTime.WorkPackage
                                                    };
                                ApplyFeatureLifeTimeInternal(session, featureId, featureLifeTime.BeginDate, featureLifeTime.EndDate);
                            }
                        }

                        session.Delete("from FeatureLifeTime where WorkPackage = " + workPackageId);
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

        public void DeleteFeatureLifeTimesByWorkPackage(int workPackageId)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Delete("from FeatureLifeTime where WorkPackage = " + workPackageId);
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
