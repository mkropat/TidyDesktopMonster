using System;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster.KeyValueStore
{
    internal class EnvironmentOverride : IKeyValueStore
    {
        readonly string _prefix;
        readonly IKeyValueStore _store;

        public EnvironmentOverride(string environmentVariablePrefix, IKeyValueStore backingStore)
        {
            _prefix = environmentVariablePrefix + "_";
            _store = backingStore;
        }

        public T Read<T>(string key)
        {
            var value = Environment.GetEnvironmentVariable(_prefix + key);
            return string.IsNullOrEmpty(value)
                ? _store.Read<T>(key)
                : TypeConverter.CoerceToType<T>(value);
        }

        public void Write<T>(string key, T value)
        {
            _store.Write(key, value);
        }
    }
}
