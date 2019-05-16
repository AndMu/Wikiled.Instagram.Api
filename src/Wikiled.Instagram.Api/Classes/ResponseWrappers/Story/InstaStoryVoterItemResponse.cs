using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryVoterItemResponse
    {
        [JsonProperty("ts")]
        public long Ts { get; set; }

        [JsonProperty("user")]
        public InstaUserShortFriendshipResponse User { get; set; }

        [JsonProperty("vote")]
        public double? Vote { get; set; }
    }
}