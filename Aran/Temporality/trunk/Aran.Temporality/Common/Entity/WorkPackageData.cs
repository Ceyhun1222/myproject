using System;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    [Serializable]
    public class WorkPackageData : INHibernateEntity
    {
        public virtual int Id { get; set; }
        public virtual int PrivateSlotId { get; set; }
        public virtual int FeatureType { get; set; }
        public virtual Guid FeatureIdentifier { get; set; }

        public virtual byte[] Data { get; set; }
    }
}
