using System.ComponentModel.DataAnnotations;

namespace Service.API.Dtos
{
    public record AccountRegisterDto([StringLength(50, MinimumLength = 4)] string Login, [StringLength(100, MinimumLength = 5)] string Password, [EmailAddress] string Email)
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        
        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
