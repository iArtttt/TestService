using Microsoft.Extensions.DependencyInjection;
using Service.Common.Interfaces.Repositories;

namespace Service.Repository.Extentions
{
    public static class RepositoryRegistrationExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IDeliveryRepository, DeliveryRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderChangeLogRepository, OrderChangeLogRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        }
    }
}
