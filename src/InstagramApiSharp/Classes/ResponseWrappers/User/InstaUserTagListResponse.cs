﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserTagListResponse
    {
        [JsonProperty("in")] public List<InstaUserTagResponse> In { get; set; } = new List<InstaUserTagResponse>();
    }
}
