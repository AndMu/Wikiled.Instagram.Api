using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;

namespace Wikiled.Instagram.Api.Converters.Json
{
    internal class InstaHashtagSearchDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(HashtagSearchResponse);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            var container = token["results"];
            var tags = token.ToObject<HashtagSearchResponse>();
            if (container != null && container.Any())
            {
                foreach (var item in container)
                {
                    try
                    {
                        tags.Tags.Add(item.ToObject<HashtagResponse>());
                    }
                    catch
                    {
                    }
                }
            }

            return tags;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}