﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Extra;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Json
{
    internal class InstaUserPresenceContainerDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(InstaUserPresenceContainerResponse);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            var presence = token.ToObject<InstaUserPresenceContainerResponse>();

            var userPresenceRoot = token?.SelectToken("user_presence");
            var extras = userPresenceRoot.ToObject<InstaExtraResponse>();
            try
            {
                foreach (var item in extras.Extras)
                {
                    try
                    {
                        var p = item.Value.ToObject<InstaUserPresenceResponse>();
                        p.Pk = long.Parse(item.Key);
                        presence.Items.Add(p);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            return presence;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}