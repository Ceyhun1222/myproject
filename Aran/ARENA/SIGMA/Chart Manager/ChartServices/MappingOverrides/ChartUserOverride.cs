using ChartServices.DataContract;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace ChartServices.MappingOverrides
{
    public class ChartUserOverride : IAutoMappingOverride<ChartUser>
    {
        public void Override(AutoMapping<ChartUser> mapping)
        {
        }
    }
}
