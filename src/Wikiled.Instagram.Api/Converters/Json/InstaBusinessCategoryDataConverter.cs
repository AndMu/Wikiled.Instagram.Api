﻿using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes.Models.Business;

namespace Wikiled.Instagram.Api.Converters.Json
{
    internal class InstaBusinessCategoryDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(InstaBusinessCategoryList);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            var container = token.ToObject<InstaBusinessCategoryContainer>();
            var items = container.Extras.FirstOrDefault().Value["categories"];
            var categories = items.ToObject<InstaBusinessCategoryList>();
            return categories;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}