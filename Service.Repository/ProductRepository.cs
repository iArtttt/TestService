using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Service.Common.Interfaces.Repositories;
using Service.Common.Models;
using Service.Domain;
using Service.Domain.Models;

namespace Service.Repository
{
    internal class ProductRepository : IProductRepository
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(MainContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;            
        }

        public async Task<ProductModel> Create(ProductModel productModel)
        {
            var product = _mapper.Map<Product>(productModel);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            await _context.ProductCategories.Where(p => p.Id == product.CategoryId).ToListAsync();
            await _context.Users.Where(u => u.Id == product.CreatedById).LoadAsync();

            return _mapper.Map<ProductModel>(product);
        }

        public async Task<ProductModel> Get(int id, string? vendor = null)
        {
            var product = await _context.Products.AsNoTracking()
                .Include(c => c.CreatedBy)
                .Include(c => c.Category)
                .Where(v => vendor == null || v.CreatedBy.Login == vendor)
                .SingleOrDefaultAsync(v => v.Id == id);

            return _mapper.Map<ProductModel>(product);
        }

        public async Task<List<ProductModel>?> GetAll(int? vendorId = null, string? vendor = null, string? category = null)
        {
            var products = await _context.Products.AsNoTracking()
                .Include(c => c.CreatedBy)
                .Include(c => c.Category)
                .Where(v => vendor == null || v.CreatedBy.Login == vendor)
                .Where(v => vendorId == null || v.CreatedBy.Id == vendorId)
                .Where(v => category == null || v.Category.Name == category)
                .ToListAsync();

            return _mapper.Map<List<ProductModel>>(products);
        }

        public async Task<bool> IsProductExist(int productId, int? vendorId = null)
            => await _context.Products.AnyAsync(p => p.Id == productId && (vendorId == null || p.CreatedById == vendorId));

        public async Task Remove(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ProductModel> Update(ProductModel productModel)
        {
            var product = _mapper.Map<Product>(productModel);
            _context.Attach(product);
            _context.Entry(product).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            await _context.ProductCategories.Where(c => c.Id == product.CategoryId).LoadAsync();
            await _context.Users.Where(u => u.Id == product.CreatedById).LoadAsync();

            return _mapper.Map<ProductModel>(product);
        }
    }
}
