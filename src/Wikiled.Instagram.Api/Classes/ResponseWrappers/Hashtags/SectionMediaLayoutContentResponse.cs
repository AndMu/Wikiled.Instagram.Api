﻿using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class SectionMediaLayoutContentResponse
    {
        [JsonProperty("medias")]
        public List<InstaMediaAlbumResponse> Medias { get; set; }
    }
}