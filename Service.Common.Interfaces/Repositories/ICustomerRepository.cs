using Service.Common.Models;

namespace Service.Common.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<CustomerAccountModel>> GetAll(string? login, string? email);
        Task<CustomerAccountModel> Get(int id);
        Task Update(CustomerAccountModel model);
    }
}
