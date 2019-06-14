using System;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    [Serializable]
    public class DataSourceTemplate : INHibernateEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int ChartType { get; set; }
    }
}
