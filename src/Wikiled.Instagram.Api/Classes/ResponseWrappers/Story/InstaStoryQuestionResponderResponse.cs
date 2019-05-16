using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryQuestionResponderResponse
    {
        [JsonProperty("has_shared_response")] public bool? HasSharedResponse { get; set; }

        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("response")] public string Response { get; set; }

        [JsonProperty("ts")] public long? Ts { get; set; }

        [JsonProperty("user")] public InstaUserShortResponse User { get; set; }
    }
}
