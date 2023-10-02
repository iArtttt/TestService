using Service.Common.Models;

namespace Service.Common.Interfaces.Repositories
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategoryModel?> Get(int id);
        Task<List<ProductCategoryModel>?> Get();

        Task<ProductCategoryModel?> Create(ProductCategoryModel model);
        Task<bool> Update(ProductCategoryModel model);
        Task<bool> Remove(int id);
    }
}
