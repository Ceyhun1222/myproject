using System;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Internal.Implementation.Repository.NHibernate.Entity
{
    [Flags]
    internal enum InternalOperation
    {
        None = 0,

        //if WorkPackage + FeatureTypeId are set
        ReadData            = 1 << 0, //can read data of specified FeatureType
        WriteData           = 1 << 1, //can write data of specified FeatureType

        //if  WorkPackage is set 
        ReadPackage         = 1 << 2, //can read any data of specified Package
        WritePackage        = 1 << 3, //can write any data of specified Package
        CommitPackage       = 1 << 4, //can commit and rollback specified Packag

        //if nothing is set
        CreatePackage       = 1 << 5, //can create any Package 
        ReadStorage         = 1 << 6, //can read any data 
        WriteStorage        = 1 << 7, //can write any data 
        TruncateStorage     = 1 << 8  //can truncate storage
    }

    internal enum DataOperation
    {
        ReadData = InternalOperation.ReadData,
        WriteData = InternalOperation.WriteData,
    }

    internal enum PackageOperation
    {
        CommitPackage = InternalOperation.CommitPackage,
        CreatePackage = InternalOperation.CreatePackage,
    }

    internal enum StorageOperation
    {
        TruncateStorage = InternalOperation.TruncateStorage,
    }


    [Serializable]
    public class AccessRight : INHibernateEntity
    {
        public virtual int Id { get;  set; }
        public virtual int UserId { get; set; } //can not be null
        public virtual string Storage { get; set; } //can not be null

        public virtual int WorkPackage { get; set; } //-1 means any, 0 means default
        public virtual int FeatureTypeId { get; set; } //-1 means any
        public virtual int OperationFlag { get; set; }
    }
}
