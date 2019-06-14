using System;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    [Serializable]
    public class AccessRightZipped : INHibernateEntity
    {
        public AccessRightZipped()
        {
        }

        public AccessRightZipped(AccessRightZipped other)
        {
            if (other!=null)
            {
                Id = other.Id;
                UserGroupId = other.UserGroupId;
                StorageId = other.StorageId;
                WorkPackage = other.WorkPackage;
                OperationFlag = other.OperationFlag;
              
                AccessRightUtil.SetZippedData(this,other);
            }
        }

        #region public virtual properies

        public virtual int Id { get; set; }

        public virtual int UserGroupId { get; set; } //can not be null
        public virtual int StorageId { get; set; } //can not be null -1 means any
        public virtual int WorkPackage { get; set; } //-1 means any, 0 means default

        public virtual int OperationFlag { get; set; } //for FeatureTypeId == -1 only

        public virtual byte[] ZippedData { get; set; }

        #endregion
    }
}
