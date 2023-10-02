using Service.Domain.Models.Base;

namespace Service.Domain.Models
{
    public class Product : EntityBase
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public ProductCategory Category { get; set; } = null!;
        
        public decimal Price { get; set; }

        public int Count { get; set; }

        public DateTime Created { get; set; }

        public int CreatedById { get; set; }

        public User CreatedBy { get; set; } = null!;
    }
}
