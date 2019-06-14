using System.Linq;
using ChartServices.DataContract;
using ChartServices.Helpers;
using NHibernate;
using NHibernate.Linq;

namespace ChartServices.Repositories
{
    public class Repository<T> : IRepository<T> where T : class,IEntity
    {
        protected UnitOfWork UnitOfWork { get; private set; }

        public Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = (UnitOfWork)unitOfWork;
        }

        protected ISession _session => UnitOfWork.Session;

        public virtual IQueryable<T> GetAll()
        {
            UnitOfWork.BeginTransaction();
            var res = _session.Query<T>();
            UnitOfWork.Commit();
            return res;
        }

        public virtual T GetById(long id)
        {
            UnitOfWork.BeginTransaction();
            var res = _session.Get<T>(id);
            UnitOfWork.Commit();
            return res;
        }

        public virtual T Create(T entity)
        {
            UnitOfWork.BeginTransaction();
            var res = _session.Save(entity);
            UnitOfWork.Commit();
            return res as T;            
        }

        public virtual void Update(T entity)
        {
            UnitOfWork.BeginTransaction();
            _session.Update(entity);
            //var res = _session.Merge(entity);
            UnitOfWork.Commit();
            //return res;
        }

        public virtual void Delete(T entity)
        {
            UnitOfWork.BeginTransaction();
            _session.Delete(entity);
            //var queryString = string.Format("delete {0} where id = :id", typeof(T));
            //_session.CreateQuery(queryString)
            //       .SetParameter("id", id)
            //       .ExecuteUpdate();
            UnitOfWork.Commit();
        }
    }
}