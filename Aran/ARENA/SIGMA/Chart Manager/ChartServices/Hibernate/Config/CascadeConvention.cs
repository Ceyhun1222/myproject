using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace ChartServices.Hibernate.Config
{
    public class CascadeConvention : IReferenceConvention, IHasManyConvention, IHasManyToManyConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.Cascade.None();
        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Cascade.SaveUpdate();
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.Cascade.None();
        }
    }
}
