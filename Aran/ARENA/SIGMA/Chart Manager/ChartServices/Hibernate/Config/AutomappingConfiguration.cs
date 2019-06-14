using System;
using ChartServices.DataContract;
using FluentNHibernate.Automapping;

namespace ChartServices.Hibernate.Config
{
    public class AutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.GetInterface(typeof(IEntity).FullName) != null;// && type.Name != nameof(ChartData);
        }
    }
}
