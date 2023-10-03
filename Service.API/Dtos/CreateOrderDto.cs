using System.ComponentModel.DataAnnotations;

namespace Service.API.Dtos
{
    public record CreateOrderDto(int DeliveryId, [StringLength(150)] string? Address, int? CustomerId);
}
