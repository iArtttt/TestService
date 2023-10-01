using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Service.Common.Interfaces.Repositories;
using Service.Common.Models;
using Service.Domain;

namespace Service.Repository
{
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;

        public CustomerRepository(MainContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerAccountModel> Get(int id) => _mapper.Map<CustomerAccountModel>(await _context.Customers.FindAsync(id));

        public async Task<List<CustomerAccountModel>> GetAll(string? login, string? email)
        {
            var users = await _context.Customers
                .Where(c => (login == null || EF.Functions.Like(c.Login, $"%{login}%"))
                && (email == null || EF.Functions.Like(email, $"%{email}%")))
                .ToListAsync();

            return _mapper.Map<List<CustomerAccountModel>>(users);
        }

        public async Task Update(CustomerAccountModel model)
        {
            var entity = await _context.Customers.FindAsync(model.Id) ?? throw new NullReferenceException();

            entity.Login = model.Login;
            entity.Email = model.Email;
            entity.Address = model.Address;
            entity.PhoneNumber = model.PhoneNumder;
            entity.Name = model.Name;
            entity.LastName = model.LastName;

            await _context.SaveChangesAsync();
        }
    }
}
