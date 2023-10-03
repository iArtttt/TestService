using Service.Common.Enums;

namespace Service.API.Dtos
{
    public record CustomerDto(int Id, string Login, string Email, string Name, string LastName, string Address, string PhoneNumber)
        : UserDto(Id, Login, Email, RoleType.Customer.ToString());
}
