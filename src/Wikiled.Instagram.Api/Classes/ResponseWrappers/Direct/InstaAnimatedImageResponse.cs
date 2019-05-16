﻿using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaAnimatedImageResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public InstaAnimatedImageMediaContainerResponse Images { get; set; }

        [JsonProperty("is_random")]
        public bool? IsRandom { get; set; }

        [JsonProperty("is_sticker")]
        public bool? IsSticker { get; set; }

        [JsonProperty("user")]
        public InstaAnimatedImageUserResponse User { get; set; }
    }
}