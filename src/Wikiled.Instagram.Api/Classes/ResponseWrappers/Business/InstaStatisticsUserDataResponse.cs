using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsUserDataResponse
    {
        [JsonProperty("business_manager")] public InstaStatisticsBusinessManagerResponse BusinessManager { get; set; }

        [JsonProperty("business_profile")] public InstaStatisticsBusinessProfileResponse BusinessProfile { get; set; }

        [JsonProperty("followers_count")] public int? FollowersCount { get; set; } = 0;

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("instagram_user_id")] public string InstagramUserId { get; set; }

        [JsonProperty("profile_picture")] public InstaStatisticsImageResponse ProfilePicture { get; set; }

        [JsonProperty("username")] public string Username { get; set; }
    }
}
