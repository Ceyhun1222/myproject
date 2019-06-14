using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Enum;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IEventConsistencyStorage : ICrudStorage<EventConsistency>
    {
        int Save(EventConsistency eventConsistency);
        bool Save(List<EventConsistency> eventConsistencies);
        void Clear(RepositoryType repositoryType, string storageName);
        string Get(EventConsistency eventConsistency);
        List<EventConsistency> Get(RepositoryType repositoryType, string storageName, int workPackage);
    }
}
