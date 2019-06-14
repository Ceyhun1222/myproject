using System;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    [Flags]
    public enum FeatureDependencyConfigurationFlag
    {
        Disabled = 0,
        SaveMissing =   1,
        SaveNormal  =   1<<1
    }

    [Serializable]
    public class FeatureDependencyConfiguration : INHibernateEntity
    {
        public virtual int Id { get; set; }

        [StringLength(64)]
        public virtual string Name { get; set; }

        public virtual byte[] Data { get; set; }

        public virtual int RootFeatureType { get; set; }

        public virtual int Flag { get; set; }

        public virtual DataSourceTemplate DataSourceTemplate { get; set; }
    }
}
