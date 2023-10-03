using System.ComponentModel.DataAnnotations;

namespace Service.API.Dtos
{
    public record AccountLoginDto([StringLength(50, MinimumLength = 5)] string Login, [StringLength(100, MinimumLength = 5)] string Password);
}
