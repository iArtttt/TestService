using Service.Common.Interfaces.Repositories;
using Service.Common.Interfaces.Service;
using Service.Common.Models;

namespace Service.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;

        public UserService(IUserRepository userRepository, ICustomerRepository customerRepository)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
        }

        public async Task<AccountModel?> Get(int id)
        {
            return await _customerRepository.Get(id) ?? await _userRepository.Get(id);
        }

        public async Task<List<AccountModel>?> GetAll(string? login, string? email)
        {
            var users = await _userRepository.GetAll(login, email) ?? new List<AccountModel>();
            var customers = await _customerRepository.GetAll(login, email) ?? new List<CustomerAccountModel>();

            var result = users.Where(u => !customers.Any(c => c.Id == u.Id)).Concat(customers.OfType<AccountModel>());

            return result.ToList();
        }

        public Task<bool> Remove(int id) => _userRepository.Remove(id);

        public async Task Update(AccountModel model)
        {
            if(model is CustomerAccountModel customer)
            {
                await _customerRepository.Update(customer);
            }
            else
            {
                await _userRepository.Update(model);
            }
        }
    }
}