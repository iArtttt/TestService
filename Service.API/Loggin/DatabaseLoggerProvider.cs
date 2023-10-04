using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Service.Common.Options;
using Service.WebAPI.Settings;
using Service.WebAPI.Settings.Entities;

namespace Service.API.Loggin
{
    /// <summary>
    /// Logger provader for the database log. The provider is only needed to create loggers.
    /// </summary>
    internal class DatabaseLoggerProvider : ILoggerProvider
    {
        private readonly DbContextOptions _dbContextOptions;
        private readonly IOptions<DatabaseSettings> _options;
        private readonly Timer _timer;
        private readonly List<DatabaseLogger> _loggers = new List<DatabaseLogger>();
        private volatile bool _timerStop = false;

        public DatabaseLoggerProvider(DbContextOptions dbContextOptions, IOptions<DatabaseSettings> options)
        {
            _dbContextOptions = dbContextOptions;
            _options = options;
            _timer = new Timer(SaveLogs, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        }

        private void SaveLogs(object? state)
        {
            _timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

            bool force = state != null && (bool)state;

            if (_timerStop && !force) return;
            
            using (var dbContext = CreateContext())
            using (var tran = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                var loggers = new List<DatabaseLogger>();

                lock (_loggers)
                {
                    loggers.AddRange(_loggers);
                }

                var logs = new List<ApplicationLog>();
                foreach (var logger in loggers)
                {
                    logs.Clear();

                    if (_timerStop && !force) break;

                    lock (logger.Logs)
                    {
                        logs.AddRange(logger.Logs);
                        logger.Logs.Clear();
                    }

                    dbContext.Logs.AddRange(logs);
                }
                dbContext.SaveChanges();
                tran.Commit();
            }
            GC.Collect();
            _timer.Change(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        }

        private ApplicationDbContext CreateContext()
        {
            var context = new ApplicationDbContext(_dbContextOptions);

            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.LazyLoadingEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return context;
        }

        public ILogger CreateLogger(string categoryName)
        {
            lock (_loggers)
            {
                var loggers = new DatabaseLogger(categoryName, _options);
                _loggers.Add(loggers);
                return loggers;
            }
        }

        public void Dispose()
        {
            try
            {
                _timerStop = true;
                SaveLogs(true);

                _timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
                _timer.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
