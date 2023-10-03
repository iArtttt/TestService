using Microsoft.EntityFrameworkCore;

namespace Service.API.Configurations
{
    /// <summary>
    /// Configuration source provide | build a configuration provider base on options.
    /// </summary>
    internal class DatabaseConfigSource : IConfigurationSource
    {

        public DbContextOptionsBuilder OptionsBuilder { get; } = new DbContextOptionsBuilder();
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DatabaseConfigProvider(OptionsBuilder.Options);
        }
    }
}
