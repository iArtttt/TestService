using Service.Common.Constants;
using Service.Common.Enums;
using Service.Common.Helpers;

namespace Service.Common.Models
{
    public class OrderModel
    {
        private static readonly Dictionary<OrderStatus, OrderStatus[]> _statusMap = new Dictionary<OrderStatus, OrderStatus[]>();

        static OrderModel()
        {
            _statusMap[OrderStatus.Created] = new[] { OrderStatus.Pending, OrderStatus.Deleted };
            _statusMap[OrderStatus.Pending] = new[] { OrderStatus.Sent, OrderStatus.Deleted };
            _statusMap[OrderStatus.Sent] = new[] { OrderStatus.Done };
        }

        public int Id { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Created;

        public decimal TotalPrice { get; set; } = decimal.Zero;

        public List<OrderedProductModel> Products { get; set; } = new List<OrderedProductModel>();

        public ICustomerModel Customer { get; set; } = new CustomerAccountModel();

        public DateTime Created { get; set; } = DateTime.Now;

        public List<OrderLogItem> Changes { get; set; } = new List<OrderLogItem>();

        public OrderModel()
        {
            AddOrredLog(OrderChangeStatusType.Created, Status);
        }

        public bool CanChangeToStatus(OrderStatus status)
        {
            if (_statusMap.TryGetValue(Status, out var result))
                return result.Contains(status);
            return false;
        }

        public void AddProduct(ProductModel product, int count)
        {
            if (count <= 0) throw new InvalidOperationException("Invalid product count value.");

            var orderProduct = Products.SingleOrDefault(p => p.Id == product.Id);

            if (product.Count - (orderProduct != null ? orderProduct.Count + count : count) < 0)
                throw new InvalidOperationException("Not enought product count value");

            if (orderProduct == null)
            {
                orderProduct = new OrderedProductModel
                {
                    Product = product,
                    Count = count
                };
                Products.Add(orderProduct);
            }
            else
            {
                orderProduct.Count += count;
            }

            UpdateTotalPrice();
            AddOrredLog(OrderChangeStatusType.AddProduct, Status, new { ProductId = product.Id, Count = count });
        }

        public void RemoveProduct(ProductModel product, int count)
        {
            if (count <= 0) throw new InvalidOperationException("Invalid product count value.");


            if (count == int.MaxValue)
                Products.RemoveAll(p => p.Product.Id == product.Id);
            else
            {
                var orderProduct = Products.SingleOrDefault(p => p.Id == product.Id);


                if (orderProduct != null)
                {

                    if (product.Count - (orderProduct.Count + count) < 0)
                        throw new InvalidOperationException("Not enought products in stok.");


                    orderProduct.Count -= count;

                    if (orderProduct.Count <= 0)
                        Products.Remove(orderProduct);

                }
                else
                    throw new InvalidOperationException("Can't find the product in order.");
            }

            UpdateTotalPrice();
            AddOrredLog(OrderChangeStatusType.AddProduct, Status, new { ProductId = product.Id, Count = count });
        }

        public void ChangeStatus(OrderStatus status)
        {
            if (!CanChangeToStatus(status))
                throw new InvalidOperationException("Invalid status value");

            if (status == OrderStatus.Pending)
            {
                if (Products.Any(p => (p.Product.Count - p.Count) < 0))
                    throw new InvalidOperationException("Not enought products in stok.");

                Products.ForEach(p => p.Product.Count -= p.Count);
            }
            else if(status == OrderStatus.Deleted && Status == OrderStatus.Pending)
                Products.ForEach(p => p.Product.Count += p.Count);

            var oldStatus = Status;
            Status = status;
            UpdateTotalPrice();

            AddOrredLog(OrderChangeStatusType.ChangeStatus, oldStatus);
        }


        private void UpdateTotalPrice() => TotalPrice = Products.Sum(p => p.Count * p.Product.Price);

        private void AddOrredLog(OrderChangeStatusType type, OrderStatus oldStatus, object? data = null)
        {
            Changes.Add(new OrderLogItem
            {
                Data = data.ToJson(),
                Type = type,
                OldStatus = oldStatus,
                Status = Status,
                DateChange = DateTime.Now,
                Description = type.GetDescription() ?? OrderChangeStatusDescription.Undefined
            });
        }
    }
}
