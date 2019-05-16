﻿using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;

namespace Wikiled.Instagram.Api.Converters.Json
{
    internal class InstaHashtagSuggestedDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(InstaHashtagSearchResponse);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            var container = token["tags"];
            var tags = token.ToObject<InstaHashtagSearchResponse>();
            if (container != null && container.Any())
            {
                foreach (var item in container)
                {
                    try
                    {
                        tags.Tags.Add(item.ToObject<InstaHashtagResponse>());
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