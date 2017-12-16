namespace TidyDesktopMonster.Interface
{
    public interface IKeyValueStore
    {
        T Read<T>(string key);
        void Write<T>(string key, T value);
    }
}
