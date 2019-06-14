using Aran.Temporality.Internal.Interface.Storage;
using NHibernate.Criterion;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class StorageStorage : CrudStorageTemplate<Common.Entity.Storage>, IStorageStorage
    {
        #region Implementation of IStorageStorage

        public Common.Entity.Storage GetStorageByName(string storageName)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                return session.CreateCriteria(typeof(Common.Entity.Storage)).
                    Add(Restrictions.Eq("Name", storageName)).
                    UniqueResult<Common.Entity.Storage>();
            }
        }

        #endregion
    }
}
