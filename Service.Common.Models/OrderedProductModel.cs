namespace Service.Common.Models
{
    public class OrderedProductModel
    {
        public int Id { get; set; }
        public ProductModel Product { get; set; } = new ProductModel();
        public int Count { get; set; } = 0;
    }
}