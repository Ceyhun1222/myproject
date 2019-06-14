using System;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Internal.Implementation.Repository.NHibernate.Entity
{
    [Flags]
    public enum UserRole
    {
        None       = 0,     
        User       = 1 << 0, //ordinary user, access is determined by access rights
        Admin      = 1 << 1, //can manage users (can not change UserRole) and access right 
        SuperAdmin = 1 << 2, //can manage users (no restrictions) and access right 
        Creator    = 1 << 3, //can create any storages
        Destroyer  = 1 << 4, //can drop any storages
        Observer   = 1 << 5, //has all access to any data (read only) 
        Tester     = 1 << 6, //has all access to any data (read write) 
    }

    [Serializable]
    public class User : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
        public virtual string Password { get; set; }
     
        public virtual int RoleFlag { get; set; } //Uint32 is not supported by posgre dialect, should use custom dialect in future
    }
}
