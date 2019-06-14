using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class LogEntryStorage : CrudStorageTemplate<LogEntry>, ILogEntryStorage
    {
        //public IList<int> GetLogIds(DateTime fromDate, DateTime toDate, string userMask, string addressMask, string actionMask, string parameterMask)
        //{
        //    using (var session = Repository.SessionFactory.OpenSession())
        //    {
        //        return session.Query<LogEntry>()
        //            .Where(log => log.Date >= fromDate && log.Date <= toDate)
        //            .Select(log => log.Id)
        //            .ToList();
        //    }
        //}

        public IList<LogEntry> GetLogByIds(List<int> ids)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var list = session.QueryOver<LogEntry>()
                                  .WhereRestrictionOn(t => t.Id)
                                  .IsIn(ids)
                                  .List();

                return list;
            }
        }

        public IList<int> GetLogIds(DateTime fromDate, DateTime toDate, string storageMask, string applicationMask, 
            string userMask, string addressMask, string actionMask, string parameterMask, bool? accessGranted)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var query = session.QueryOver<LogEntry>().Where(t => t.Date >= fromDate && t.Date <= toDate);

                if (!string.IsNullOrWhiteSpace(storageMask))
                {
                    query = query.And(t => t.Storage.IsInsensitiveLike(storageMask));
                }

                if (!string.IsNullOrWhiteSpace(applicationMask))
                {
                    query = query.And(t => t.Application.IsInsensitiveLike(applicationMask));
                }

                if (!string.IsNullOrWhiteSpace(userMask))
                {
                    query = query.And(t => t.UserName.IsInsensitiveLike(userMask));
                }

                if (!string.IsNullOrWhiteSpace(addressMask))
                {
                    query = query.And(t => t.Ip.IsInsensitiveLike(addressMask));
                }

                if (!string.IsNullOrWhiteSpace(actionMask))
                {
                    query = query.And(t => t.Action.IsInsensitiveLike(actionMask));
                }

                if (!string.IsNullOrWhiteSpace(parameterMask))
                {
                    query = query.And(t => t.Parameters.IsInsensitiveLike(parameterMask));
                }

                if (accessGranted!=null)
                {
                    query = query.And(t => t.AccessGranted == accessGranted);
                }


                return query.Select(t=>t.Id).List<int>();
            }
        }

        public IList<object> GetLogValues(string field)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                var list = session.QueryOver<LogEntry>()
                    .Select(Projections.Distinct(Projections.ProjectionList().Add(Projections.Property(field)))).List<object>();

                return list;
            }
        }
    }
}
