using System.Linq;
using AerodromeServices.DataContract;
using AerodromeServices.Helpers;
using NHibernate;
using NHibernate.Linq;

namespace AerodromeServices.Repositories
{
    public class Repository<T> : IRepository<T> where T : class,IEntity
    {
        private readonly UnitOfWork _unitOfWork;
        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
        }

        private ISession Session => _unitOfWork.Session;

        public IQueryable<T> GetAll()
        {
            _unitOfWork.BeginTransaction();
            var res = Session.Query<T>();
            _unitOfWork.Commit();
            return res;
        }

        public T GetById(long id)
        {
            _unitOfWork.BeginTransaction();
            var res = Session.Get<T>(id);
            _unitOfWork.Commit();
            return res;
        }

        public T Create(T entity)
        {
            _unitOfWork.BeginTransaction();
            var res = Session.Save(entity);
            _unitOfWork.Commit();
            return res as T;            
        }

        public T Update(T entity)
        {
            _unitOfWork.BeginTransaction();            
            var res = Session.Merge(entity);
            _unitOfWork.Commit();
            return res;
        }

        public void Delete(long id)
        {
            _unitOfWork.BeginTransaction();
            Session.Delete(Session.Load<T>(id));
            _unitOfWork.Commit();
        }

        public string GetFolderPath()
        {
            return UnitOfWork.FolderPath;
        }
    }
}