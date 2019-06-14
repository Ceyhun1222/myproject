using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Temporality.Common.Entity;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IConfigurationStorage : ICrudStorage<Configuration>
    {
        Configuration GetConfigurationByName(string name);
        int UpdateConfiguration(Configuration configuration);
    }
}
