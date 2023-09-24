using Service.Domain.Models.Base;

namespace Service.Domain.Models
{
    public class ProductCategory : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}