using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Convention
{
    public class EnumConvention : IUserTypeConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(e => e.Property.PropertyType.IsEnum);
        }

        public void Apply(IPropertyInstance instance)
        {
            instance.CustomType(instance.Property.PropertyType);
        }
    }
}