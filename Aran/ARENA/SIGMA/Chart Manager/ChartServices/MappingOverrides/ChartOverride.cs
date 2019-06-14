using ChartServices.DataContract;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace ChartServices.MappingOverrides
{
    public class ChartOverride : IAutoMappingOverride<ChartWithReference>
    {
        public void Override(AutoMapping<ChartWithReference> mapping)
        {
            mapping.IgnoreProperty(t => t.HasUpdate);
            //mapping.Polymorphism.Explicit();
        }
    }
}
