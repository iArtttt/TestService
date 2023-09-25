using Service.Common.Enums;
using Service.Common.Models;

namespace Service.Common.Interfaces.Service
{
    public interface IOrderService
    {
        Task<List<OrderModel>?> GetAll();
        Task<List<OrderModel>?> GetAllByVendor(int vendorId);
        Task<List<OrderModel>?> GetAllByCustomer(int customerId);

        Task<OrderModel?> Get(int id);
        Task<OrderModel?> GetByVendor(int id, int vendorId);
        Task<OrderModel?> GetByCustomer(int id, int customerId);

        // To create

        //Task<OrderModel?> Create(int delivaryId, string? address, ICustomerModel customer);
        //Task<OrderModel?> Create(int delivaryId, string? address, int customerId);

        List<(int id, string name)> GetStatuses();

        Task<(OrderModel? order, string? messege)> AddProduct(int orderId, int productId, int count, int? costumerId = null);
        Task<(OrderModel? order, string? messege)> RemoveProduct(int orderId, int productId, int count, int? costumerId = null);
        Task<(OrderModel? order, string? messege)> RemoveAllProduct(int orderId, int productId, int? costumerId = null);

        Task<(OrderModel? order, string? messege)> ChangeStatus(int orderId, OrderStatus status, int? customerId = null, int? vendorId = null);

        Task<List<OrderLogItem>> GetChangeLogs(int orderId, int? customerId, int? vendorId);
    }
}
