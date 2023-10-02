using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Service.Common.Interfaces.Repositories;
using Service.Common.Models;
using Service.Domain;
using Service.Domain.Models;

namespace Service.Repository
{
    internal class AccountRepository : IAccountRepository
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;

        public AccountRepository(MainContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountModel> AddAccount(AccountModel accountModel, (byte[] hash, byte[] salt) hash)
        {
            User user = accountModel is CustomerAccountModel
                ? _mapper.Map<Customer>(accountModel) 
                : _mapper.Map<User>(accountModel);

            user.PasswordHash = hash.hash;
            user.PasswordSalt = hash.salt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            accountModel.Id = user.Id;
            return accountModel;
        }

        public async Task<AccountModel?> GetAccount(int id)
        {
            var user = await _context.Users.AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == id);
            return user is Customer customer
                ? _mapper.Map<CustomerAccountModel>(customer)
                : _mapper.Map<AccountModel>(user);
        }

        public async Task<AccountModel?> GetAccount(string login)
        {
            var user = await _context.Users.AsNoTracking()
                .SingleOrDefaultAsync(u => u.Login == login);
            return user is Customer customer
                ? _mapper.Map<CustomerAccountModel>(customer)
                : _mapper.Map<AccountModel>(user);
        }

        public async Task<(byte[] hash, byte[] salt)?> GetAccountHash(int id)
        {
            var user = await _context.Users.AsNoTracking()
                .Select(u => new { u.Id, u.PasswordHash, u.PasswordSalt })
                .SingleOrDefaultAsync(u => u.Id == id);

            return user == null ? null : (user.PasswordHash, user.PasswordSalt);
        }
    }
}