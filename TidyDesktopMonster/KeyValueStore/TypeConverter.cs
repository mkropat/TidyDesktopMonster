using System;

namespace TidyDesktopMonster.KeyValueStore
{
    internal static class TypeConverter
    {
        public static T CoerceToType<T>(object value)
        {
            var type = typeof(T);
            var targetType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? Nullable.GetUnderlyingType(type)
                : type;

            if (value == null)
                return default;
            else if (targetType.IsEnum)
                return Enum.IsDefined(targetType, value)
                    ? (T)Enum.Parse(targetType, value as string)
                    : default;
            else
                return (T)Convert.ChangeType(value, targetType);
        }
    }
}
