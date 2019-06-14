using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Temporality.Common.Entity;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IDataSourceTemplateStorage : ICrudStorage<DataSourceTemplate>
    {
        bool UpdateDataSourceTemplate(DataSourceTemplate entity);
    }
}
