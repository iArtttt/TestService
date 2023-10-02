using Service.Domain.Models.Base;

namespace Service.Domain.Models
{
    public class DeliveryService : EntityBase
    {
        public string DeliveryName { get; set; } = string.Empty;
    }
}
