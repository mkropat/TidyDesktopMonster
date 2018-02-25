using System.Collections.Generic;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster.KeyValueStore
{
    internal class InMemoryKeyValueCache : IKeyValueStore
    {
        readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
        readonly IKeyValueStore _store;

        public InMemoryKeyValueCache(IKeyValueStore store)
        {
            _store = store;
        }

        public T Read<T>(string key)
        {
            if (_cache.ContainsKey(key))
                return (T)_cache[key];

            T value = _store.Read<T>(key);
            _cache.Add(key, value);
            return value;
        }

        public void Write<T>(string key, T value)
        {
            if (_cache.ContainsKey(key) && _cache[key] is T && EqualityComparer<T>.Default.Equals((T)_cache[key], value))
                return;

            _store.Write(key, value);
            _cache[key] = value;
        }
    }
}
