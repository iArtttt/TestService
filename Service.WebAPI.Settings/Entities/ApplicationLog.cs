using Microsoft.Extensions.Logging;

namespace Service.WebAPI.Settings.Entities
{
    public class ApplicationLog
    {
        public Guid Id { get; set; }
        public LogLevel LogLevel { get; set; }
        public int EventId { get; set; }
        public int ThreadId { get; set; }
        public string? EventName { get; set; }
        public string? Message { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? ExceptionStackTrace { get; set; }
        public string? ExceptionSource { get; set; }
        public string? Exception { get; set; }
        public string? CategoryName { get; set; }
        public DateTime DateTime { get; set; }
    }
}
