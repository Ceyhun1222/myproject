using System;
using System.Collections.Generic;
using System.Linq;
using ChartManagerServices.Repositories;
using ChartServices.DataContract;
using ChartServices.Helpers;

namespace ChartServices.Repositories
{
    public class RepositoryContext
    {        
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public IRepository<T> Repository<T>() where T : class,IEntity
        {
            if (_repositories.Keys.Contains(typeof(T)))
                return _repositories[typeof(T)] as IRepository<T>;
            object repo;
            var unitOfWork = new UnitOfWork();
            if (typeof(T) == typeof(Chart))
            {
                repo = new ChartRepository(unitOfWork);
            }
            else 
                repo = new Repository<T>(unitOfWork);
            _repositories.Add(typeof(T), repo);
            return repo as IRepository<T>;
        }

        public string FolderPath => UnitOfWork.FolderPath;
    }
}
