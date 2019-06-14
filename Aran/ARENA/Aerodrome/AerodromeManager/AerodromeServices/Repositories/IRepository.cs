using AerodromeServices.DataContract;
using System.Linq;

namespace AerodromeServices.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        IQueryable<T> GetAll();
        T GetById(long id);
        T Create(T entity);
        T Update(T entity);
        void Delete(long id);
        string GetFolderPath();
    }
}
