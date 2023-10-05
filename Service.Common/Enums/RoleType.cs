namespace Service.Common.Enums
{
    [Flags]
    public enum RoleType
    {
        None = 0,
        Admin = 1,
        Vendor = 2,
        Customer = 4,

    }
}