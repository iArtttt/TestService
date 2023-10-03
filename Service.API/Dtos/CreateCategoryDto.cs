using System.ComponentModel.DataAnnotations;

namespace Service.API.Dtos
{
    public record CreateCategoryDto([StringLength(100, MinimumLength = 4)] string Name, [StringLength(100)] string? Description);
}
