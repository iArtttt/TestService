using Microsoft.Extensions.Logging;

namespace Service.Common.Options
{
    public class DatabaseSettings
    {
        public bool LogToDatabase { get; set; }
        public bool LogOrdersChanges { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}
