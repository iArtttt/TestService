using Service.Common.Interfaces.Infrastructure;
using Service.Common.Interfaces.Repositories;
using Service.Common.Interfaces.Service;
using Service.Common.Models;

namespace Service.Services
{
    internal class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IHashService _hashService;

        public AccountService(IAccountRepository accountRepository, IHashService hashService)
        {
            _accountRepository = accountRepository;
            _hashService = hashService;            
        }
        public Task<AccountModel> AddAccount(AccountModel accountModel, string password)
        {
            var hash = _hashService.GetHash(password);

            return _accountRepository.AddAccount(accountModel, hash);
        }

        public async Task<AccountModel?> GetAccount(string login, string? password)
        {
            var account = await _accountRepository.GetAccount(login);

            if (account != null && !string.IsNullOrEmpty(password))
            {
                var hash = (await _accountRepository.GetAccountHash(account.Id)).Value;
                var hashForCheck = _hashService.GetHash(password, hash.salt).hash;

                if (hash.hash.SequenceEqual(hashForCheck))
                    return null;

            }

            return account;
        }

        public async Task<bool> IsAccountExist(string loggin)
        {
            var account = await _accountRepository.GetAccount(loggin);
            return account != null;
        }
    }
}
