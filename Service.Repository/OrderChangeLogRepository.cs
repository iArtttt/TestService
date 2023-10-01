using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Service.Common.Interfaces.Repositories;
using Service.Common.Models;
using Service.Domain;

namespace Service.Repository
{
    internal class OrderChangeLogRepository : IOrderChangeLogRepository
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;

        public OrderChangeLogRepository(MainContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<OrderLogItem>> GetLogs(int orderId, int? customerId = null, int? vendorId = null)
        {
            var result = await _context.OrderChangeStatusLogs.AsNoTracking()
                .Where(o => o.OrderId == orderId)
                .Include(c => c.Order)
                    .ThenInclude(c => c.Products)
                    .ThenInclude(p => p.Product)
                .Where(c => (customerId == null || c.Order.CustomerId == customerId) && 
                            (vendorId == null || c.Order.Products.Any(p => p.Product.CreatedById == vendorId)))
                .OrderBy(d => d.Data)
                .ToListAsync();
            return _mapper.Map<List<OrderLogItem>>(result);
        }
    }
}
