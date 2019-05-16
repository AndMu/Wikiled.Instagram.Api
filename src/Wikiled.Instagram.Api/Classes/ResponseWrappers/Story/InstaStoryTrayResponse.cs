using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryTrayResponse
    {
        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("is_portrait")] public bool IsPortrait { get; set; }

        [JsonProperty("top_live")] public InstaTopLiveResponse TopLive { get; set; } = new InstaTopLiveResponse();

        [JsonProperty("tray")] public List<InstaStoryResponse> Tray { get; set; } = new List<InstaStoryResponse>();
    }
}
