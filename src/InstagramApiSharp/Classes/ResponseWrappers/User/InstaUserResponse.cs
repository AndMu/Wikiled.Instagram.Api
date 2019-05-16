using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserResponse : InstaUserShortResponse
    {
        [JsonProperty("follower_count")] public int FollowersCount { get; set; }

        [JsonProperty("byline")] public string FollowersCountByLine { get; set; }

        [JsonProperty("friendship_status")] public InstaFriendshipShortStatusResponse FriendshipStatus { get; set; }

        [JsonProperty("has_anonymous_profile_picture")]
        public bool HasAnonymousProfilePicture { get; set; }

        [JsonProperty("mutual_followers_count")]
        public string MulualFollowersCount { get; set; }

        [JsonProperty("search_social_context")]
        public string SearchSocialContext { get; set; }

        [JsonProperty("social_context")] public string SocialContext { get; set; }

        [JsonProperty("unseen_count")] public int UnseenCount { get; set; }
    }
}
