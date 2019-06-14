using AerodromeServices.DataContract;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace AerodromeServices.MappingOverrides
{
    public class AmdbOverride : IAutoMappingOverride<AmdbMetadata>
    {
        public void Override(AutoMapping<AmdbMetadata> mapping)
        {
        }
    }
}
