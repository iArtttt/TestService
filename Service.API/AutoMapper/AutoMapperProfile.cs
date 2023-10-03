using AutoMapper;
using Service.API.Dtos;
using Service.Common.Models;

namespace Service.API.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<AccountModel, UserDto>();
            CreateMap<CustomerAccountModel, CustomerDto>();
            CreateMap<AccountRegisterDto, CustomerAccountModel>();
            CreateMap<AccountRegisterDto, AccountModel>();

            CreateMap<ProductCategoryModel, CategoryDto>();
            CreateMap<CreateCategoryDto, ProductCategoryModel>();
            CreateMap<ProductCategoryModel, CreateCategoryDto>();

            CreateMap<DeliveryModel, DeliveryDto>();
            CreateMap<CreateDeliveryDto, DeliveryModel>();
            CreateMap<DeliveryInfoModel, DeliveryInfoDto>()
            .ConstructUsing(src => new DeliveryInfoDto(new DeliveryDto(src.Id, src.Name), src.Address));

            CreateMap<CreateProductDto, ProductModel>()
                .ForPath(dst => dst.User.Id, opt => opt.MapFrom(src => src.VendorId))
                .ForPath(dst => dst.Category.Id, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<ProductModel, ProductDto>();
            //CreateMap<ProductCharacteristicsDto, ProductCharacteristicsModel>();
            //CreateMap<ProductCharacteristicsModel, ProductCharacteristicsDto>();

            CreateMap<OrderModel, OrderDto>();
            CreateMap<OrderedProductModel, OrderedProductDto>();

            CreateMap<OrderLogItem, OrderLogDto>();
        }
    }
}
