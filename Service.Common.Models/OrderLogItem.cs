using Service.Common.Enums;

namespace Service.Common.Models
{
    public class OrderLogItem
    {
        public string? Data { get; set; }
        public string Description { get; set; } = string.Empty;
        public OrderChangeStatusType Type { get; set; }
        public OrderStatus Status { get; set; }
        public OrderStatus OldStatus { get; set; }
        public DateTime DateChange { get; set; }

    }
}
