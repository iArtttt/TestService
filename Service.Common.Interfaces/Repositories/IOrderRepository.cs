using Service.Common.Enums;
using Service.Common.Models;

namespace Service.Common.Interfaces.Repositories
{
    public interface IOrderRepository : ITransactionProvider
    {
        Task<List<OrderModel>?> GetAll();
        Task<List<OrderModel>?> GetAllByVendor(int vendorId);
        Task<List<OrderModel>?> GetAllCustomer(int customerId);

        Task<OrderModel?> Get(int id, params OrderStatus[]? statuses);
        Task<OrderModel?> GetByVendor(int id, int vendorId, params OrderStatus[]? statuses);
        Task<OrderModel?> GetByCustomer(int id, int customerId, params OrderStatus[]? statuses);

        Task<OrderModel?> Create(OrderModel model);
        Task<OrderModel?> Update(OrderModel model);
    }
}
