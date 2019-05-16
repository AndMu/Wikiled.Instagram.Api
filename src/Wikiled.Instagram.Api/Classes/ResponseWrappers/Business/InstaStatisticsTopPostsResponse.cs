using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsTopPostsResponse
    {
        [JsonProperty("edges")] public InstaStatisticsEdgeResponse[] Edges { get; set; }
    }
}
