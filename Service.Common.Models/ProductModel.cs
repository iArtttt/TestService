namespace Service.Common.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public ProductCategoryModel Category { get; set; } = new ProductCategoryModel();
        
        public int Count { get; set; }
        
        public decimal Price { get; set; }
        
        public IUserModel User { get; set; } = new AccountModel();
        
        public DateTime Created { get; set; }
    }
}
