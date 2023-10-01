namespace Service.Domain.Models
{
    public class DeliveryInfo
    {
        public DeliveryService Service { get; set; } = null!;

        public string Address { get; set; } = string.Empty;
    }
}
