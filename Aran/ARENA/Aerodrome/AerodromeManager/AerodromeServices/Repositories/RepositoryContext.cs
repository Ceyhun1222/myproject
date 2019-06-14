using System;
using System.Collections.Generic;
using System.Linq;
using AerodromeServices.DataContract;
using AerodromeServices.Helpers;

namespace AerodromeServices.Repositories
{
    public class RepositoryContext
    {        
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public IRepository<T> Repository<T>() where T : class,IEntity
        {
            if (_repositories.Keys.Contains(typeof(T)))
                return _repositories[typeof(T)] as IRepository<T>;
            IRepository<T> repo = new Repository<T>(new UnitOfWork());
            _repositories.Add(typeof(T), repo);
            return repo;
        }

        public string FolderPath => UnitOfWork.FolderPath;
    }
}
