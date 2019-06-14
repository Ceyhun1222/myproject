namespace ChartServices
{
    public enum ChartType
    {
        All,
        Sid,
        Star,
        Iac,
        Enroute,
        Aerodrome,
        A,
        Pat,
        Area,
        AerodromeElectronicChart,
        AerodromeParkingDockingChart,
        AerodromeGroundMovementChart,
        AerodromeBirdChart,
        AreaMinimumChart
    }

    public enum UserPrivilege
    {
        ReadOnly,
        Full
    }

    public enum ChartCallBackType
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
