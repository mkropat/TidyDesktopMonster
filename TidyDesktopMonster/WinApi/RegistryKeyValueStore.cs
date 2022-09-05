using Microsoft.Win32;
using System;
using System.IO;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster.WinApi
{
    public class RegistryKeyValueStore : IKeyValueStore
    {
        readonly RegistryHive _hive;
        readonly string _regPath;

        public RegistryKeyValueStore(string packageName, string vendorName = null, RegistryHive hive = RegistryHive.CurrentUser)
        {
            _hive = hive;
            _regPath = string.IsNullOrEmpty(vendorName)
                ? Path.Combine("Software", packageName)
                : Path.Combine("Software", vendorName, packageName);
        }

        public T Read<T>(string key)
        {
            using (var root = OpenRoot())
            using (var regKey = root.OpenSubKey(_regPath))
            {
                return regKey == null
                    ? default
                    : CoerceToType<T>(regKey.GetValue(key));
            }
        }

        T CoerceToType<T>(object value)
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

        public void Write<T>(string key, T value)
        {
            using (var root = OpenRoot())
            using (var subkey = root.CreateSubKey(_regPath))
            {
                switch (value)
                {
                    case bool v:
                        subkey.SetValue(key, v, RegistryValueKind.DWord);
                        break;

                    default:
                        subkey.SetValue(key, value);
                        break;
                }
            }
        }

        RegistryKey OpenRoot()
        {
            return RegistryKey.OpenBaseKey(_hive, RegistryView.Default);
        }
    }
}
