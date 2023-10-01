using Service.Common.Models;

namespace Service.Common.Interfaces.Repositories
{
    public interface IOrderChangeLogRepository
    {
        Task<List<OrderLogItem>> GetLogs(int orderId, int? customerId = null, int? vendorId = null);
    }
}
