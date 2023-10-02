using Service.Common.Models;

namespace Service.Common.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<bool> IsProductExist(int productId, int? vendorId = null);

        Task<List<ProductModel>?> GetAll(int? vendorId = null, string? vendor = null, string? category = null);
        Task<ProductModel> Get(int id, string? vendor = null);
        Task<ProductModel> Create(ProductModel productModel);
        Task<ProductModel> Update(ProductModel productModel);

        Task Remove(int id);
    }
}
