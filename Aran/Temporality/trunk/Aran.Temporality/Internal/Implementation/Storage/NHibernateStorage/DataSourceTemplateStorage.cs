using Aran.Temporality.Common.Entity;
using Aran.Temporality.Internal.Interface.Storage;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class DataSourceTemplateStorage : CrudStorageTemplate<DataSourceTemplate>, IDataSourceTemplateStorage
    {
        public bool UpdateDataSourceTemplate(DataSourceTemplate entity)
        {
            if (entity == null) return false;
            if (entity.Id == 0) return false;

            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var oldEntity = session.Get<DataSourceTemplate>(entity.Id);

                    if (oldEntity != null)
                    {
                        //do update
                        oldEntity.Name = entity.Name;
                        oldEntity.ChartType = entity.ChartType;


                        session.Update(oldEntity);
                        transaction.Commit();
                    }
                }
            }

            return true;
        }

    }
}
