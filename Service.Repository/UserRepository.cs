using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Service.Common.Interfaces.Repositories;
using Service.Common.Models;
using Service.Domain;

namespace Service.Repository
{
    internal class UserRepository : IUserRepository
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;

        public UserRepository(MainContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AccountModel?> Get(int id)
            => _mapper.Map<AccountModel>(await _context.Users.FindAsync(id));

        public async Task<List<AccountModel>?> GetAll(string? login, string? email)
        {
            var users = await _context.Users
                .Where(u => (login == null || EF.Functions.Like(u.Login, $"%{login}%"))
                        && (email == null ||  EF.Functions.Like(u.Email, $"%{email}%")))
                .ToListAsync();

            return _mapper.Map<List<AccountModel>>(users);
        }

        public async Task<bool> Remove(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == id);

            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task Update(AccountModel model)
        {
            var user = await _context.Users.FindAsync(model.Id) ?? throw new NullReferenceException();

            user.Login = model.Login;
            user.Email = model.Email;
            await _context.SaveChangesAsync();
        }
    }
}
