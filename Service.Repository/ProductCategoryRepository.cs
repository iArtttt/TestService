using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Service.Common.Interfaces.Repositories;
using Service.Common.Models;
using Service.Domain;
using Service.Domain.Models;

namespace Service.Repository
{
    internal class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;

        public ProductCategoryRepository(MainContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;            
        }

        public async Task<ProductCategoryModel?> Create(ProductCategoryModel model)
        {
            var newCategory = _mapper.Map<ProductCategory>(model);

            await _context.ProductCategories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            model.Id = newCategory.Id;

            return model;
        }

        public async Task<ProductCategoryModel?> Get(int id)
        {
            var category = await _context.ProductCategories.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
            if (category == null) return null;

            return _mapper .Map<ProductCategoryModel>(category);
        }

        public async Task<List<ProductCategoryModel>?> Get()
        {
            var categories = await _context.ProductCategories.AsNoTracking().ToListAsync();
            return _mapper.Map<List<ProductCategoryModel>>(categories);
        }

        public async Task<bool> Remove(int id)
        {
            var category = await _context.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
            if (category == null) return false;

            _context.ProductCategories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(ProductCategoryModel model)
        {
            if (!await _context.ProductCategories.AnyAsync(c => c.Id == model.Id)) return false;

            var category = _mapper.Map<ProductCategory>(model);
            _context.ProductCategories.Attach(category);
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
