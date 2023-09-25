using Service.Common.Models;

namespace Service.Common.Interfaces.Service
{
    public interface IAccountService
    {
        Task<AccountModel> AddAccount(AccountModel accountModel, string password);
        Task<bool> IsAccountExist(string loggin);
        Task<AccountModel?> GetAccount(AccountModel accountModel, string? password);
    }
}
