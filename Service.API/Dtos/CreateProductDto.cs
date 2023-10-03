using Service.API.Validators;
using System.ComponentModel.DataAnnotations;

namespace Service.API.Dtos
{
    public record CreateProductDto(
        [StringLength(100, MinimumLength = 5)] string Name,
        [StringLength(500)] string? Description,
        int CategoryId,
        //  ProductCharacteristicsDto Characteristics,
        [Range(0, int.MaxValue)] int Count,
        [DecimalRange] decimal Price,
        int VendorId
        );
}
