using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.TV
{
    public class InstaTvSelfChannelResponse : InstaTvChannelResponse
    {
        [JsonProperty("user_dict")]
        public InstaTvUserResponse User { get; set; }
    }
}