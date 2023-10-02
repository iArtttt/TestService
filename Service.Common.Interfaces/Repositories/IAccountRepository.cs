using Service.Common.Models;

namespace Service.Common.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<AccountModel?> GetAccount(int id);
        Task<AccountModel?> GetAccount(string login);
        Task<(byte[] hash, byte[] salt)?> GetAccountHash(int id);
        Task<AccountModel> AddAccount(AccountModel accountModel, (byte[] hash, byte[] salt) hash);
    }
}
