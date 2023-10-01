using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Service.Common.Interfaces.Repositories;
using Service.Common.Models;
using Service.Domain;
using Service.Domain.Models;

namespace Service.Repository
{
    internal class DeliveryRepository : IDeliveryRepository
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;

        public DeliveryRepository(MainContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<DeliveryModel?> Create(string name)
        {
            var newDelivery = new DeliveryService { DeliveryName = name };
            await _context.DeliveryServices.AddAsync(newDelivery);
            await _context.SaveChangesAsync();

            return _mapper.Map<DeliveryModel>(newDelivery);
        }

        public async Task<DeliveryModel?> Get(int id)
        {
            var result = await _context.DeliveryServices.AsNoTracking().SingleOrDefaultAsync(d => d.Id == id);
            return _mapper.Map<DeliveryModel>(result);
        }

        public async Task<List<DeliveryModel>?> Get()
        {
            var result = await _context.DeliveryServices.AsNoTracking().ToListAsync();
            return _mapper.Map<List<DeliveryModel>>(result);
        }

        public async Task<bool> IsExist(int id) => await _context.DeliveryServices.AnyAsync(d => d.Id == id);

        public async Task<bool> Remove(int id)
        {
            var result = await _context.DeliveryServices.SingleOrDefaultAsync(d => d.Id == id);
            if (result == null) return false;

            _context.DeliveryServices.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(DeliveryModel model)
        {
            var result = await _context.DeliveryServices.SingleOrDefaultAsync(d => d.Id ==  model.Id);
            if (result == null) return false;

            _context.Entry(result).State = EntityState.Modified;
            result.DeliveryName = model.Name;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
