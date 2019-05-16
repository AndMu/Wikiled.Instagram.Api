using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.TV
{
    public class InstaTvResponse
    {
        [JsonProperty("channels")]
        public List<InstaTvChannelResponse> Channels { get; set; }

        [JsonProperty("my_channel")]
        public InstaTvSelfChannelResponse MyChannel { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }

        //public Badging badging { get; set; }
        //public Composer composer { get; set; }
    }
}