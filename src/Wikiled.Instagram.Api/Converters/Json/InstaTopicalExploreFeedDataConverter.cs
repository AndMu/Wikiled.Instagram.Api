﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Feed;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.TV;

namespace Wikiled.Instagram.Api.Converters.Json
{
    internal class InstaTopicalExploreFeedDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(InstaTopicalExploreFeedResponse);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var root = JToken.Load(reader);
            var items = root["sectional_items"];
            var feed = root.ToObject<InstaTopicalExploreFeedResponse>();

            foreach (var item in items)
            {
                var layoutContent = item["layout_content"];
                if (layoutContent != null)
                {
                    var twoByTwoItem = layoutContent["two_by_two_item"];
                    var fillItems = layoutContent["fill_items"];
                    var medias = layoutContent["medias"];
                    if (medias != null)
                    {
                        foreach (var med in medias)
                        {
                            var single = med["media"];
                            if (single != null)
                            {
                                var singleMedia = GetMedia(single);
                                feed.Medias.Add(singleMedia);
                            }
                        }
                    }

                    if (fillItems != null)
                    {
                        foreach (var med in fillItems)
                        {
                            var single = med["media"];
                            if (single != null)
                            {
                                var singleMedia = GetMedia(single);
                                feed.Medias.Add(singleMedia);
                            }
                        }
                    }

                    if (twoByTwoItem != null)
                    {
                        var channel = twoByTwoItem["channel"];
                        var igtv = twoByTwoItem["igtv"];
                        if (channel != null)
                        {
                            feed.Channel = GetChannel(channel);
                        }

                        if (igtv != null)
                        {
                            var tvGuide = igtv["tv_guide"];
                            if (tvGuide != null)
                            {
                                var channelsToken = tvGuide["channels"];
                                if (channelsToken != null)
                                {
                                    var channels = GetTVs(channelsToken);
                                    if (channels?.Count > 0)
                                    {
                                        feed.TvChannels.AddRange(channels);
                                    }
                                }
                            }
                        }
                    }
                }

                var channelToken = item["channel"];
                var mediaToken = item["media"];
            }

            return feed;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        private InstaChannelResponse GetChannel(JToken token)
        {
            return token.ToObject<InstaChannelResponse>();
        }

        private InstaMediaItemResponse GetMedia(JToken token)
        {
            return token.ToObject<InstaMediaItemResponse>();
        }

        private List<InstaTvChannelResponse> GetTVs(JToken token)
        {
            return token.ToObject<List<InstaTvChannelResponse>>();
        }
    }
}