using System;
using Aran.Temporality.Internal.Interface;
using FluentNHibernate.Automapping;

namespace Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config
{
    public class ContainerConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            // specify the criteria that types must meet in order to be mapped
            // any type for which this method returns false will not be mapped.

            return typeof (INHibernateEntity).IsAssignableFrom(type);
        }
    }
}
