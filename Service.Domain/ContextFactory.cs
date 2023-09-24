using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Service.Domain
{
    public class MainContextFactory : IDesignTimeDbContextFactory<MainContext>
    {
        public MainContext CreateDbContext(string[] args)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..\\Service.API"));
            configBuilder.AddJsonFile("appsettings.Development.json");
            var configuration = configBuilder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<MainContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MainContextDb"));

            return new MainContext(optionsBuilder.Options);
        }
    }
}
