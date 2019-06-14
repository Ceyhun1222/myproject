using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Temporality.Common.Entity;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IUserModuleVersionStorage : ICrudStorage<UserModuleVersion>
    {
        bool UpdateUserModuleVersion(UserModuleVersion userModuleVersion);
        IList<UserModuleVersion> GetUserModuleVersions(int userId);
    }
}
