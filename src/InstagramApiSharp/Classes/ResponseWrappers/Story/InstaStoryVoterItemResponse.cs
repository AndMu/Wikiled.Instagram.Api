using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryVoterItemResponse
    {
        [JsonProperty("ts")] public long Ts { get; set; }

        [JsonProperty("user")] public InstaUserShortFriendshipResponse User { get; set; }

        [JsonProperty("vote")] public double? Vote { get; set; }
    }
}
