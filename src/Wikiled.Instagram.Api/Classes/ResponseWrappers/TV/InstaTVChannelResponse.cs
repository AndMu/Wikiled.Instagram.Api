using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.TV
{
    public class InstaTvChannelResponse
    {
        [JsonProperty("more_available")]
        public bool HasMoreAvailable { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("items")]
        public List<InstaMediaItemResponse> Items { get; set; }

        [JsonProperty("max_id")]
        public string MaxId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonIgnore]
        public InstaTvChannelType Type => PrivateType.GetChannelType();

        //public Seen_State1 seen_state { get; set; }

        [JsonProperty("user_dict")]
        public InstaTvUserResponse UserDetail { get; set; }

        [JsonProperty("type")]
        internal string PrivateType { get; set; }
    }
}