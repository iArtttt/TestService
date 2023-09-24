using Service.Common.Enums;
using Service.Domain.Models.Base;

namespace Service.Domain.Models
{
    public class Order : EntityBase
    {
        public decimal TotalPrice { get; set; }
        
        public OrderStatus Status { get; set; }
        
        public List<OrderedProduct> OrderedProducts { get; set; } = null!;
        
        public int CustomerId { get; set; }
        
        public Customer Customer { get; set; } = null!;
        
        public DateTime CreationDate { get; set; }
        
        public List<OrderChangeStatusLog> Changes { get; set; } = null!;
    }
}
