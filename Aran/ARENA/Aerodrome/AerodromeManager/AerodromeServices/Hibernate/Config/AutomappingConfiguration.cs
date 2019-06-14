using System;
using AerodromeServices.DataContract;
using FluentNHibernate.Automapping;

namespace AerodromeServices.Hibernate.Config
{
    public class AutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.GetInterface(typeof(IEntity).FullName) != null && type.Name != nameof(AmdbData);
        }
    }
}
