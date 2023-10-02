using System.ComponentModel;
using System.Reflection;

namespace Service.Common.Helpers
{
    public static class EnumHelper
    {
        public static string? GetDescription(this Enum value)
        {
            var enumType = value.GetType();
            var memberInfo = enumType.GetMembers().FirstOrDefault(x => x.Name == Enum.GetName(enumType, value));
            var description = memberInfo?.GetCustomAttribute<DescriptionAttribute>()?.Description;

            return description;
        }
    }
}
