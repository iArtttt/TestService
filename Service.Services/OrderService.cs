using Service.Common.Enums;
using Service.Common.Interfaces.Repositories;
using Service.Common.Interfaces.Service;
using Service.Common.Models;

namespace Service.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderChangeLogRepository _changeLogRepository;
        private readonly IDeliveryRepository _deliveryRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IOrderChangeLogRepository changeLogRepository,
            IDeliveryRepository delivaryRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _changeLogRepository = changeLogRepository;
            _deliveryRepository = delivaryRepository;
        }
        public async Task<(OrderModel? order, string? messege)> AddProduct(int orderId, int productId, int count, int? customerId = null)
        {
            if (count <= 0) throw new ArgumentException("Count must be grater then 0", nameof(count));

            await using var tran = await _orderRepository.BeginTransactionAsync();
            try
            {
                var order = customerId == null
                    ? await _orderRepository.Get(orderId)
                    : await _orderRepository.GetByCustomer(orderId, customerId.Value);


                if (order == null)
                {
                    await tran.RollbackAsync();
                    return (null, "Can't find the order by id.");
                }

                var product = await _productRepository.Get(productId);
                if (product == null)
                {
                    await tran.RollbackAsync();
                    return (order, "Can't find the product by id.");
                }

                try
                {
                    order.AddProduct(product, count);
                }
                catch (InvalidOperationException ex)
                {
                    await tran.RollbackAsync();
                    return (order, ex.Message);
                }

                order = await _orderRepository.Update(order);
                await tran.CommitAsync();
                return (order, null);


            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        public async Task<(OrderModel? order, string? messege)> ChangeStatus(int orderId, OrderStatus status, int? customerId = null, int? vendorId = null)
        {
            await using var tran = await _orderRepository.BeginTransactionAsync();
            try
            {
                OrderModel? order;
                if (customerId != null)
                    order = await _orderRepository.GetByCustomer(orderId, customerId.Value);
                else if (vendorId != null)
                    order = await _orderRepository.GetByVendor(orderId, vendorId.Value);
                else
                    order = await _orderRepository.Get(orderId);

                if (order == null)
                {
                    await tran.RollbackAsync();
                    return (null, "Can't find the order by id");
                }

                try
                {
                    order.ChangeStatus(status);
                }
                catch (InvalidOperationException ex)
                {
                    await tran.RollbackAsync();
                    return (null, ex.Message);
                }

                order = await _orderRepository.Update(order);
                await tran.CommitAsync();
                return (order, null);

            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        public async Task<OrderModel?> Create(int delivaryId, string? address, ICustomerModel customer)
        {
            if (!await _deliveryRepository.IsExist(delivaryId)) return null;

            var model = new OrderModel();
            model.Created = DateTime.Now;
            model.Status = OrderStatus.Created;
            model.TotalPrice = 0;
            model.Delivery = new DeliveryInfoModel
            {
                Id = delivaryId,
                Address = address ?? ((CustomerAccountModel)customer).Address,
            };

            model.Customer = customer;
            return await _orderRepository.Create(model);
        }
        public async Task<OrderModel?> Create(int delivaryId, string? address, int customerId)
        {
            var account = await _customerRepository.Get(customerId);

            if (account == null) return null;

            return await Create(delivaryId, address, account);
        }

        public async Task<OrderModel?> Update(int orderId, int deliveryId,  string? address, int customerId)
        {
            var account = await _customerRepository.Get(orderId);
            if (account == null) return null;

            if (!await _deliveryRepository.IsExist(deliveryId)) return null;

            var model = await GetByCustomer(orderId, customerId);
            if (model == null) return null;

            model.Delivery = new DeliveryInfoModel();
            model.Delivery.Id = deliveryId;
            model.Delivery.Address = address ?? account.Address;
            model = await _orderRepository.Update(model);
            
            return model;
        }

        public Task<OrderModel?> Get(int id) => _orderRepository.Get(id);

        public Task<List<OrderModel>?> GetAll() => _orderRepository.GetAll();

        public Task<List<OrderModel>?> GetAllByCustomer(int customerId) => _orderRepository.GetAllCustomer(customerId);

        public Task<List<OrderModel>?> GetAllByVendor(int vendorId) => _orderRepository.GetAllByVendor(vendorId);

        public Task<OrderModel?> GetByCustomer(int id, int customerId) => _orderRepository.GetByCustomer(id, customerId);

        public Task<OrderModel?> GetByVendor(int id, int vendorId) => _orderRepository.GetByVendor(id, vendorId);

        public Task<List<OrderLogItem>> GetChangeLogs(int orderId, int? customerId, int? vendorId)
            => _changeLogRepository.GetLogs(orderId, customerId, vendorId);
        

        public List<(int id, string name)> GetStatuses()
        {
            var statuses = Enum.GetValues<OrderStatus>()
                .Select(s => ((int)s, s.ToString()))
                .ToList();
            return statuses;
        }

        public Task<(OrderModel? order, string? messege)> RemoveAllProduct(int orderId, int productId, int? customerId = null)
            => RemoveProduct(orderId, productId, int.MaxValue, customerId);
        public async Task<(OrderModel? order, string? messege)> RemoveProduct(int orderId, int productId, int count, int? customerId = null)
        {
            if (count <= 0) throw new ArgumentException("Count must be greater than 0.", nameof(count));

            await using var tran = await _orderRepository.BeginTransactionAsync();
            try
            {
                var order = customerId == null
                    ? await _orderRepository.Get(orderId)
                    : await _orderRepository.GetByCustomer(orderId, customerId.Value);
                if (order == null)
                {
                    await tran.RollbackAsync();
                    return (null, "Can't find the order by id.");
                }

                var product = await _productRepository.Get(productId);
                if (product == null)
                {
                    await tran.RollbackAsync();
                    return (null, "Can't find the product by id.");
                }

                try
                {
                    order.RemoveProduct(product, count);
                }
                catch (InvalidOperationException ex)
                {
                    await tran.RollbackAsync();
                    return (order, ex.Message);
                }

                order = await _orderRepository.Update(order);
                await tran.CommitAsync();
                return (order, null);
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }

        }

    }
}
