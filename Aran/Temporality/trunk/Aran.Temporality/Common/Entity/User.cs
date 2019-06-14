using System;
using System.ComponentModel;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    [Flags]
    public enum UserRole
    {
        None       = 0,     
        User       = 1 << 0, //ordinary user, access is determined by access rights
        Admin      = 1 << 1, //can manage users (can not change UserRole) and access right ; not used for now
        SuperAdmin = 1 << 2, //can manage users (no restrictions) and access right 
        Creator    = 1 << 3, //can create any storages
        Destroyer  = 1 << 4, //can drop any storages
        Observer   = 1 << 5, //has all access to any data (read only) 
        Tester     = 1 << 6, //has all access to any data (read write) 
        Expert     = 1 << 7, //can validate
        Publisher  = 1 << 8,  //Can use aimsl services
        Aimsl      = 1 << 9  //Can use aimsl services
    }

    [Flags]
    public enum Module
    {
        None = 0,
       
        [Description("TOSSM")]
        Tossm = 1 << 0,

        [Description("PANDA")]
        Panda = 1 << 1,

        [Description("DELTA")]
        Delta = 1 << 2,

        [Description("OMEGA")]
        Omega = 1 << 3,

        [Description("ARENA")]
        Arena = 1 << 4,

        [Description("External Interface")]
        External = 1 << 5
    }

    [Serializable]
    public class User : INHibernateEntity
    {
        public virtual int Id { get; set; }

        [StringLength(200)]
        public virtual string Name { get; set; }

        [StringLength(32)]
        public virtual string Password { get; set; }

        public virtual PrivateSlot ActivePrivateSlot { get; set; }

        public virtual UserGroup UserGroup { get; set; }

        public virtual int RoleFlag { get; set; }

        public virtual int ModuleFlag { get; set; } 
    }
}
