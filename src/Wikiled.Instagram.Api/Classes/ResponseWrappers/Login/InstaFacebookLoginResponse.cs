using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Login
{
    internal class InstaFacebookLoginResponse
    {
        [JsonProperty("code")] public int? Code { get; set; }

        [JsonProperty("created_user")] public InstaUserShortResponse CreatedUser { get; set; }

        [JsonProperty("fb_user_id")] public string FbUserId { get; set; }

        [JsonProperty("logged_in_user")] public InstaUserShortResponse LoggedInUser { get; set; }

        [JsonProperty("multiple_users_on_device")]
        public bool? MultipleUsersOnDevice { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}
