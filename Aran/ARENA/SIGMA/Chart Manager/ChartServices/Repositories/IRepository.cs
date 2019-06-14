using ChartServices.DataContract;
using System.Linq;

namespace ChartServices.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        IQueryable<T> GetAll();
        //IQueryable<T> GetAll(string[] excludeProperties);
        //T GetById(long id, string[] excludeProperties);
        T GetById(long id);
        T Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
