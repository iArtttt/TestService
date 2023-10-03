using System.ComponentModel.DataAnnotations;

namespace Service.API.Dtos
{
    public record CreateDeliveryDto([StringLength(100, MinimumLength = 4)] string Name);
}
