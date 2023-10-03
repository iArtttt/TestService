using Service.Domain.Models;
using System.Runtime.CompilerServices;

namespace Service.API.Dtos
{
    public record AccountDto(int Id, string Login, string Token, string Role);
}
