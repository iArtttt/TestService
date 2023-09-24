using System.Text.Json;

namespace Service.Common.Helpers
{
    public static class CommonHelpers
    {
        public static string? ToJson(this object? obj) => obj != null ? JsonSerializer.Serialize(obj) : null;

        public static T? FromJson<T>(this string? str) => str != null ? JsonSerializer.Deserialize<T>(str) : default;
    }
}
