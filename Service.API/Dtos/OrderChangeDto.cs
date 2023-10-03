namespace Service.API.Dtos
{
    public record OrderChangeDto(bool Success, int ProductId, int OrderId, string? Message);
}
