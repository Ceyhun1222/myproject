using System;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
     [Serializable]
    public class UserGroup : INHibernateEntity
    {
         public virtual int Id { get; set; }

         [StringLength(200)]
         public virtual string Name { get; set; }
    }
}
