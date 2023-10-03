using Microsoft.EntityFrameworkCore;
using Service.WebAPI.Settings;

namespace Service.API.Configurations
{
    /// <summary>
    /// Configuration data provider that uses the database. We use the <see cref="ConfigurationProvider" /> base class to simplify our code.
    /// </summary>
    internal class DatabaseConfigProvider : ConfigurationProvider
    {
        private readonly DbContextOptions _options;
        private readonly Timer _timer;

        public DatabaseConfigProvider(DbContextOptions options)
        {
            _options = options;
            _timer = new Timer(ReloadByTimer);
        }

        private void ReloadByTimer(object? state)
        {
            _timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

            Load();
            OnReload();
        }

        public override void Load()
        {
            using (var ctx = new ApplicationDbContext(_options))
            {
                Data = ctx.Settings.ToDictionary(d => "DatabaseSettings:" +  d.Name, v => v.Value)!;
            }
            _timer.Change(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        }
    }
}