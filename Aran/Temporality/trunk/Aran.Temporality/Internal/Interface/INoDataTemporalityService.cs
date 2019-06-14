using System.Collections.Generic;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Entity;

namespace Aran.Temporality.Internal.Interface
{
    public interface INoDataTemporalityService
    {
        #region User

        //admin calls 
        long CreateUser(string name);
        long ResetPasswordById(int userId);//set password equal to name
        bool DeleteUserById(int userId);//returns true if success

        //superadmin calls
        bool SetUserRole(int userId, UserRole role);

        //
        IList<User> GetAllUsers();

        #endregion 

        #region WorkPackage

        int CreateWorkPackage(string storage, bool isSafe = false, string description = null);
        bool DeletePackage(int workPackageId);
        IList<WorkPackage> GetAllWorkPackages();
        WorkPackage GetWorkPackageById(int packageId);
        
        #endregion

        #region AccessRight

        //right CRUD
        IList<AccessRight> GetUserRights(int userId);
        int CreateAccessRight(AccessRight accessRight);
        bool DeleteAccessRightById(int rightId);

        //workpackage operations 
        void DeleteAccessRightsByWorkPackage(int packageId);

        #endregion
    }
}
