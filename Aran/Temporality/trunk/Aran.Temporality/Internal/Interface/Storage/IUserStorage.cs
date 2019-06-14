using System.Collections.Generic;
using Aran.Temporality.Common.Entity;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IUserStorage : ICrudStorage<User>
    {
        //system calls
        User Login(int userId, string password);

        //user calls for himself only
        bool ChangeMyPassword(int userId, string oldPassword, string newPassword);

        //admin calls 
        int CreateUser(string name);
        int ResetPasswordById(int userId);//set password equal to name
        bool DeleteUserById(int userId);//returns true if success
        bool SetUserName(int userId, string name);

        //superadmin calls
        bool SetUserRole(int userId, int role);
        bool SetUserModules(int userId, int currentModuleFlag);

        bool SetUserActiveSlot(int userId, PrivateSlot privateSlot);

        // frim UserStorage
        bool SetUserGroup(int userId, int groupId);
        bool DeleteGroupById(int groupId);

        IList<User> GetUsersOfPrivateSlot(int id);
    }
}
