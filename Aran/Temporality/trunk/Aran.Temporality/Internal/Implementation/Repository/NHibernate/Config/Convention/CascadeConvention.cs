using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Convention
{
    public class CascadeConvention : IReferenceConvention, IHasManyConvention, IHasManyToManyConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.Cascade.None();
        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Cascade.None();
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.Cascade.None();
        }
    }
}
