﻿using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes.Models.Business;

namespace Wikiled.Instagram.Api.Converters.Json
{
    internal class InstaBusinessCityLocationDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(InstaBusinessCityLocationList);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            var container = token.ToObject<InstaBusinessCityLocationContainer>();
            var results = container.Extras.FirstOrDefault().Value["search_results"];
            var locations = results["nodes"].ToObject<InstaBusinessCityLocationList>();
            return locations;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}