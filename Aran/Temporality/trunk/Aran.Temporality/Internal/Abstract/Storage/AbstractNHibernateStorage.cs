using Aran.Temporality.Internal.Abstract.Repository;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Internal.Abstract.Storage
{
    internal abstract class AbstractNHibernateStorage
    {

        public AbstractNHibernateRepository<INHibernateEntity> Repository { get; set; }
    }
}
