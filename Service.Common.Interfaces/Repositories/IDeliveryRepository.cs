using Service.Common.Models;

namespace Service.Common.Interfaces.Repositories
{
    public interface IDeliveryRepository
    {
        Task<DeliveryModel?> Get(int id);
        Task<List<DeliveryModel>?> Get();
        Task<DeliveryModel?> Create(string name);
        Task<bool> Update(DeliveryModel model);
        Task<bool> Remove(int id);
        Task<bool> IsExist(int id);
    }
}
