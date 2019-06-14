using ChartServices.DataContract;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartManagerServices.MappingOverrides
{
    class ChartUpdateOverride : IAutoMappingOverride<ChartUpdateData>
    {
        public void Override(AutoMapping<ChartUpdateData> mapping)
        {
        }
    }
}
