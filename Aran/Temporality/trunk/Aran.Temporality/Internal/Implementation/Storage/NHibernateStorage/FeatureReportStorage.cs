using System.Collections.Generic;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class FeatureReportStorage : CrudStorageTemplate<FeatureReportZipped>, IFeatureReportStorage
    {

        public IList<FeatureReportZipped> GetFeatureReportsByIdentifier(string identity)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                return session.CreateCriteria<FeatureReportZipped>().
                Add(Restrictions.Eq("FeatureGuid", identity)).
                List<FeatureReportZipped>();
            }
        }
    }
}
