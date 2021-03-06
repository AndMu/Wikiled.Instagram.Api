﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight;

namespace Wikiled.Instagram.Api.Converters.Json
{
    internal class InstaHighlightReelsListDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(InstaHighlightReelResponse);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            var reels = token.ToObject<InstaHighlightReelResponse>();

            var t = token["reels"];
            reels.Reel = t?.First?.First?.ToObject<InstaHighlightSingleFeedResponse>();
            return reels;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}