using Service.Domain.Models.Base;

namespace Service.Domain.Models
{
    public class OrderedProduct : EntityBase
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Count { get; set; }
    }
}