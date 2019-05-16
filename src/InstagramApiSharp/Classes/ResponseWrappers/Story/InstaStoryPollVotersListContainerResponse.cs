using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryPollVotersListContainerResponse : InstaDefault
    {
        [JsonProperty("voter_info")] public InstaStoryPollVotersListResponse VoterInfo { get; set; }
    }
}
