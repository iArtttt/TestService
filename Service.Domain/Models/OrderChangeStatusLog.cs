using Service.Common.Enums;
using Service.Domain.Models.Base;

namespace Service.Domain.Models
{
    public class OrderChangeStatusLog : EntityBase
    {
        public string? Data { get; set; }

        public string Description { get; set; } = string.Empty;

        public OrderChangeStatusType Type { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime DateChange { get; set; }

        public OrderStatus OldStatus { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; } = null!;
    }
}