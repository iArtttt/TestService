using AutoMapper;
using Microsoft.Extensions.Options;
using Service.Common.Models;
using Service.Common.Options;
using Service.Domain.Models;

namespace Service.Repository.AutoMapper
{
    public class OrderMappingAction : IMappingAction<OrderModel, Order>
    {
        private readonly IOptions<DatabaseSettings> _options;

        public OrderMappingAction(IOptions<DatabaseSettings> options)
        {
            _options = options;
        }

        public void Process(OrderModel source, Order destination, ResolutionContext context)
        {
            if (!_options.Value.LogOrdersChanges) destination.Changes = null!;
        }
    }

    public class AutoMapperModelProfile : Profile
    {
        public AutoMapperModelProfile()
        {
            CreateMap<User, AccountModel>();
            CreateMap<User, IUserModel>().As<AccountModel>();
            CreateMap<Customer, IUserModel>().As<CustomerAccountModel>();
            CreateMap<Customer, CustomerAccountModel>();

            CreateMap<AccountModel, User>();
            CreateMap<CustomerAccountModel, Customer>();

            CreateMap<DeliveryService, DeliveryModel>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.DeliveryName));

            CreateMap<ProductCategoryModel, ProductCategory>();
            CreateMap<ProductCategory, ProductCategoryModel>();

            CreateMap<ProductModel, Product>()
                .ForMember(dst => dst.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dst => dst.CreatedById, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dst => dst.Category, opt => opt.Ignore());
            
            //  To do
            //  CreateMap<ProductCharacteristics, ProductCharacteristicsModel>();
            //  CreateMap<ProductCharacteristicsModel, ProductCharacteristics>();

            CreateMap<Product, ProductModel>()
                .ForMember(dst => dst.User, opt => opt.MapFrom(str => str.CreatedBy));
            
            CreateMap<OrderModel, Order>()
                .ForMember(dst => dst.Customer, opt => opt.Ignore())
                .AfterMap<OrderMappingAction>();
            CreateMap<Order, OrderModel>();

            CreateMap<OrderedProductModel, OrderedProduct>();
            CreateMap<OrderedProduct, OrderedProductModel>();

            CreateMap<DeliveryInfoModel, DeliveryInfo>()
                .ForPath(dst => dst.Service.Id, opt => opt.MapFrom(src => src.Id))
                .ForPath(dst => dst.Service.DeliveryName, opt => opt.MapFrom(src => src.Name));
            CreateMap<DeliveryInfo, DeliveryInfoModel>()
                .ForPath(dst => dst.Id, opt => opt.MapFrom(src => src.Service.Id))
                .ForPath(dst => dst.Name, opt => opt.MapFrom(src => src.Service.DeliveryName));

            CreateMap<OrderLogItem, OrderChangeStatusLog>();
            CreateMap<OrderChangeStatusLog, OrderLogItem>();
        }
    }
}
