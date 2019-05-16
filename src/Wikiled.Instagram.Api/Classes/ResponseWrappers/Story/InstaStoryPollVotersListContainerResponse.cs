using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryPollVotersListContainerResponse : InstaDefault
    {
        [JsonProperty("voter_info")]
        public InstaStoryPollVotersListResponse VoterInfo { get; set; }
    }
}