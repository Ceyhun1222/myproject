using AerodromeServices.DataContract;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace AerodromeServices.MappingOverrides
{
    public class AmdbUserOverride : IAutoMappingOverride<User>
    {
        public void Override(AutoMapping<User> mapping)
        {
        }
    }
}
