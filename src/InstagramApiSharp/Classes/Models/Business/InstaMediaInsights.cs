using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaMediaInsights
    {
        [JsonProperty("avg_engagement_count")] public int AverageEngagementCount { get; set; }

        [JsonProperty("comment_count")] public int CommentCount { get; set; }

        [JsonProperty("engagement_count")] public int EngagementCount { get; set; }

        [JsonProperty("impression_count")] public int ImpressionCount { get; set; }

        [JsonProperty("like_count")] public int LikeCount { get; set; }

        [JsonProperty("reach_count")] public int ReachCount { get; set; }

        [JsonProperty("save_count")] public int SaveCount { get; set; }
    }
}
