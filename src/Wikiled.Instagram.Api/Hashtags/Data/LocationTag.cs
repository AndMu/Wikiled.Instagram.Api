using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Hashtags.Data
{
    public class LocationTag
    {
        [JsonProperty("centroid")]
        public List<double> Centroid { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }
    }
}
