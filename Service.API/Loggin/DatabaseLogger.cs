using Microsoft.Extensions.Options;
using Service.Common.Options;
using Service.WebAPI.Settings.Entities;

namespace Service.API.Loggin
{
    /// <summary>
    /// This logger write logs into thw database using dbcontext.
    /// </summary>
    internal class DatabaseLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly IOptions<DatabaseSettings> _options;
        private List<ApplicationLog> _logs = new List<ApplicationLog>();

        public List<ApplicationLog> Logs => _logs;

        public DatabaseLogger(string categoryName, IOptions<DatabaseSettings> options)
        {
            _categoryName = categoryName;
            _options = options;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            // Used to support logger scope. This is a more complex case, so just skip it.
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
            => logLevel < LogLevel.None && _options.Value.LogToDatabase && _options.Value.LogLevel <= logLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            lock (_logs)
            {
                _logs.Add(new ApplicationLog
                {
                    CategoryName = _categoryName,
                    LogLevel = logLevel,
                    EventId = eventId.Id,
                    ThreadId = Thread.CurrentThread.ManagedThreadId,
                    EventName = eventId.Name,
                    Message = formatter(state, exception),
                    ExceptionMessage = exception?.Message,
                    ExceptionStackTrace = exception?.StackTrace,
                    ExceptionSource = exception?.Source,
                    Exception = exception?.ToString(),
                    DateTime = DateTime.UtcNow,
                }); ;
            }
        }
    }
}
