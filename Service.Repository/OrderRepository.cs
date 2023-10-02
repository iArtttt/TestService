using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Service.Common.Enums;
using Service.Common.Interfaces.Repositories;
using Service.Common.Models;
using Service.Domain;
using Service.Domain.Models;
using System.Data;
using System.Linq.Expressions;

namespace Service.Repository
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(MainContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;            
        }

        private IQueryable<Order> GetOrdersQuery(params OrderStatus[]? statuses)
        {
            var dbOrders = _context.Orders.AsNoTracking()
                .Include(c => c.Customer)
                .Include(c => c.Delivery.Service)
                .Include(c => c.Products)
                    .ThenInclude(p => p.Product)
                        .ThenInclude(p => p.Category)
                .Include(c => c.Products)
                    .ThenInclude(p => p.Product)
                        .ThenInclude(p => p.CreatedBy)
                .Where(c => statuses == null || statuses.Length <= 0 || statuses.Contains(c.Status));

            return dbOrders;
        }

        private async Task<List<OrderModel>?> GetOrdersWhere(Expression<Func<Order, bool>>? match = null, params OrderStatus[]? statuses)
        {
            var query = GetOrdersQuery(statuses);
            if (match != null) query = query.Where(match);

            return _mapper.Map<List<OrderModel>>(await query.ToListAsync());
        }
        private async Task<OrderModel?> GetOrderWhere(int id, Expression<Func<Order, bool>>? match = null, params OrderStatus[]? statuses)
        {
            var query = GetOrdersQuery(statuses);
            if (match != null) query = query.Where(match);

            return _mapper.Map<OrderModel>(await query.SingleOrDefaultAsync(c => c.Id == id));
        }


        public IRepositoryTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            => new RepositoryTransaction(_context.Database.BeginTransaction(isolationLevel));

        public async Task<IRepositoryTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            => new RepositoryTransaction(await _context.Database.BeginTransactionAsync(isolationLevel));

        public async Task<OrderModel?> Create(OrderModel model)
        {
            var order = _mapper.Map<Order>(model);
            order.Delivery.Service = (await _context.DeliveryServices.FindAsync(order.Delivery.Service.Id))!;

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return _mapper.Map<OrderModel>(order);
        }

        public Task<OrderModel?> Get(int id, params OrderStatus[]? statuses) => GetOrderWhere(id, null, statuses);
        public Task<List<OrderModel>?> GetAll() => GetOrdersWhere();

        public Task<List<OrderModel>?> GetAllByVendor(int vendorId)
            => GetOrdersWhere(o => o.Products.Any(p => p.Product.CreatedById == vendorId));
        public Task<List<OrderModel>?> GetAllCustomer(int customerId)
            => GetOrdersWhere(c => c.CustomerId == customerId);

        public Task<OrderModel?> GetByCustomer(int id, int customerId, params OrderStatus[]? statuses) 
            => GetOrderWhere(id, o => o.CustomerId == customerId, statuses);

        public Task<OrderModel?> GetByVendor(int id, int vendorId, params OrderStatus[]? statuses)
            => GetOrderWhere(id, o => o.Products.Any(s => s.Product.CreatedById == vendorId), statuses);

        public async Task<OrderModel?> Update(OrderModel model)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                var order = _mapper.Map<Order>(model);

                order.Changes?.RemoveAll(c => c.Id > 0);

                _context.AttachRange(order.Products.Where(p => p.Id > 0));

                (await _context.OrderedProducts
                    .Where(p => p.ProductId == order.Id)
                    .ToListAsync())
                    .ForEach(p =>
                    {
                        if (model.Products.Any(m => m.Id == p.Id)) _context.Entry(p).State = EntityState.Modified;
                        else _context.Entry(p).State = EntityState.Deleted;

                        _context.Entry(p.Product).Property(p => p.Count).IsModified = true;
                    });
                _context.Attach(order);
                _context.Entry(order).State = EntityState.Modified;
                _context.ChangeTracker.DetectChanges();

                await _context.SaveChangesAsync();

                return model;
            }
            finally
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
    }
}
