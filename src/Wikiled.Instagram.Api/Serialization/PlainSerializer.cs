using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Serialization
{
    public class PlainSerializer : ISerializer
    {
        public string Serialize<T>(T instance)
        {
            return SerializationHelper.SerializeToString(instance);
        }

        public T DeSerialize<T>(string text)
        {
            return SerializationHelper.DeserializeFromString<T>(text);
        }
    }
}
