namespace AerodromeServices
{
   
    public enum UserPrivilege
    {
        ReadOnly,
        Full
    }

    public enum AmdbCallBackType
    {
        Created,
        Deleted,
        Locked,
        Unlocked
    }

    public enum UserCallbackType
    {
        ChangedPassword,
        Deleted,
        PrivilegeUp,
        PrivilegeDown,
        Disabled,
        Enabled,
        Updated
    }
}
