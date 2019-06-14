using Aran.Temporality.Common.Entity;
using NHibernate.Criterion;
using System.Collections.Generic;
using Aran.Temporality.Internal.Interface.Storage;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class FeatureScreenshotStorage : CrudStorageTemplate<FeatureScreenshot>, IFeatureScreenshotStorage
    {
        public IList<FeatureScreenshot> GetFeatureScreenshotsByIdentifier(string identity)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                return session.CreateCriteria<FeatureScreenshot>().
                Add(Restrictions.Eq("FeatureGuid", identity)).
                List<FeatureScreenshot>();
            }
        }
    }
}
