using Service.Common.Enums;

namespace Service.Common.Models
{
    public class AccountModel : IUserModel
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public RoleType Role { get; set; }
    }

    public class CustomerAccountModel : AccountModel, ICustomerModel
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumder { get; set; } = string.Empty;
    }

    public interface IUserModel
    {
        int Id { get; set; }
        string Login { get; set; }
        string Email { get; set; }
    }

    public interface ICustomerModel : IUserModel
    {
        string Name { get; set; }
        string LastName { get; set; }
        string Address { get; set; }
        string PhoneNumder { get; set; }

    }
}