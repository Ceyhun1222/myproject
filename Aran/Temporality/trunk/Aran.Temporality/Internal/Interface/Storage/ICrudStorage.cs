using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface ICrudStorage<T>
    {
        void DeleteAll();
        int CreateEntity(T entity);
        bool DeleteEntityById(int id);
        IList<T> GetAllEntities();
        T GetEntityById(int id);
    }
}
