using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Internal.Implementation.Repository.NHibernate.Entity
{

    [Flags]
    public enum WorkPackageFlag
    {
        None = 0,
        IsSafe = 1 << 0,
    }

    public class WorkPackageUtil
    {
        
        public static bool IsSafe(WorkPackage workPackage)
        {
            return (workPackage.Flag & (int)WorkPackageFlag.IsSafe) != 0;
        }

        public static void SetSafe(WorkPackage workPackage, bool isSafe)
        {
            if (isSafe)
            {
                workPackage.Flag |= (int)WorkPackageFlag.IsSafe;
            }
            else
            {
                workPackage.Flag &= ~(int)WorkPackageFlag.IsSafe;
            }
        }
    }

    [Serializable]
    public class WorkPackage : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual string Storage { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual int Flag { get; set; }
        public virtual string Description { get; set; }

        
    }
}
