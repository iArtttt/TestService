namespace Service.API.Dtos
{
    public record OrderDto(
        int Id, 
        int Status,
        decimal TotalPrice,
        CustomerDto Customer,
        DateTime Created,
        DeliveryInfoDto Delivery,
        List<OrderedProductDto> Product
        );
}
