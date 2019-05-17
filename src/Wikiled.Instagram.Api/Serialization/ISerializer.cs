namespace Wikiled.Instagram.Api.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T instance);
        T DeSerialize<T>(string text);
    }
}