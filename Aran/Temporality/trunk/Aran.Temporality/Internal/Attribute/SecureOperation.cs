using System;
using Aran.Temporality.Common.Entity.Enum;

namespace Aran.Temporality.Internal.Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class SecureOperation : System.Attribute 
    {
        public InternalOperation Operation { get; set; }

        public SecureOperation(DataOperation operation)
        {
            Operation = (InternalOperation)operation;
        }

        public SecureOperation(AccessOperation operation)
        {
            Operation = (InternalOperation)operation;
        }

        public SecureOperation(PackageOperation operation)
        {
            Operation = (InternalOperation)operation;
        }

        public SecureOperation(StorageOperation operation)
        {
            Operation = (InternalOperation)operation;
        }
    }
}
