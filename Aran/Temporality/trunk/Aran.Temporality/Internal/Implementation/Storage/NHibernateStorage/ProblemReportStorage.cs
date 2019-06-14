using System;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class ProblemReportStorage : CrudStorageTemplate<ProblemReport>, IProblemReportStorage
    {
        #region Implementation of IProblemReportStorage

        public bool UpdateProblemReport(ProblemReport report)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var old = session.CreateCriteria(typeof(ProblemReport)).
                            Add(Restrictions.Eq("PublicSlotId", report.PublicSlotId)).
                            Add(Restrictions.Eq("PrivateSlotId", report.PrivateSlotId)).
                            Add(Restrictions.Eq("ConfigId", report.ConfigId)).
                            Add(Restrictions.Eq("ReportType", report.ReportType)).
                            UniqueResult<ProblemReport>();

                        if (old == null)
                        {
                            report.DateTime = DateTime.Now;
                        
                            session.Save(report);
                        }
                        else
                        {
                            old.ReportData = report.ReportData;
                            old.DateTime = DateTime.Now;
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

        #endregion

        public ProblemReport GetProblemReport(int publicSlotId, int privateSlotId, int configId, ReportType reportType)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                try
                {
                    var old = session.CreateCriteria(typeof(ProblemReport)).
                       Add(Restrictions.Eq("PublicSlotId", publicSlotId)).
                        Add(Restrictions.Eq("PrivateSlotId", privateSlotId)).
                        Add(Restrictions.Eq("ConfigId", configId)).
                        Add(Restrictions.Eq("ReportType", (int)reportType)).
                        UniqueResult<ProblemReport>();
                    return old;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
