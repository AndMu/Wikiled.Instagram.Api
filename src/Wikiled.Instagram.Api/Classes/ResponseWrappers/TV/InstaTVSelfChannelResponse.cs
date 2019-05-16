using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.TV
{
    public class InstaTVSelfChannelResponse : InstaTVChannelResponse
    {
        [JsonProperty("user_dict")] public InstaTVUserResponse User { get; set; }
    }
}
