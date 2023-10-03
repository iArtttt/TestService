namespace Service.API.Dtos
{
    //public record ProductCharacteristicsDto(double Weight, double Height, double Width, double Long);
    public record ProductDto(int Id, string Name, string? Description, CategoryDto Category, /*ProductCharacteristicsDto Characteristics,*/ int Count, decimal Price, DateTime Created)
    {
        public UserDto Vendor { get; set; } = null!;
    }
}
