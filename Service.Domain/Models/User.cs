using Service.Common.Enums;
using Service.Domain.Models.Base;

namespace Service.Domain.Models
{
    public class User : EntityBase
    {
        public string Login { get; set; } = null!;

        public byte[] PasswordHash { get; set; } = null!;
        
        public byte[] PasswordSalt { get; set; } = null!;
        
        public string Email { get; set; } = null!;
        
        public RoleType Role { get; set; }
    }
}