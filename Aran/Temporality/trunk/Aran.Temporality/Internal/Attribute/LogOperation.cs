using System;

namespace Aran.Temporality.Internal.Attribute
{
    public static class LogActions
    {
        public const string ChangeAccessRights = "Change Group Access Right";
        public const string Login = "Login";
        public const string ChangeModuleAccessRights = "Change User Module Access";
        public const string ChangeRole = "Change User Role";
        public const string ResetPassword = "Reset Password";
        public const string EditUser = "Edit User";
        public const string DeleteUser = "Delete User";
        public const string CreateUser = "Create User";
    }

    [AttributeUsage(AttributeTargets.Method)]
    class LogOperation : System.Attribute
    {
      
        public string Action { get; set; }

        public int[] Arguments { get; set; }

    }
}
