using Service.Common.Enums;

namespace Service.API.Dtos
{
    public class OrderLogDto
    {
        public string? Data { get; set; }
        
        public string Description { get; set; } = string.Empty;

        public OrderChangeStatusType Type { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime DateChange { get; set; }

        public OrderStatus OldStatus { get; set; }
    }
}
