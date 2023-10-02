using Service.Common.Models;

namespace Service.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<List<AccountModel>?> GetAll(string? login, string? email);

        Task<AccountModel?> Get(int id);

        Task Update (AccountModel model);

        Task<bool> Remove (int id);
    }
}
