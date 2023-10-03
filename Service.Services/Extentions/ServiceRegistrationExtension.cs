using Microsoft.Extensions.DependencyInjection;
using Service.Common.Interfaces.Infrastructure;
using Service.Common.Interfaces.Service;

namespace Service.Services.Extentions
{
    public static class ServiceRegistrationExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IHashService, HashService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}