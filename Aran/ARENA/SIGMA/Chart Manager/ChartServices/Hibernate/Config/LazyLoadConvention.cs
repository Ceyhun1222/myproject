using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;

namespace ChartServices.Hibernate.Config
{
    public class LazyLoadConvention : IReferenceConvention
    {
        #region Implementation of IConvention<IManyToOneInspector,IManyToOneInstance>

        public void Apply(IManyToOneInstance instance)
        {
            instance.LazyLoad(Laziness.False);
        }

        #endregion
    }
}
