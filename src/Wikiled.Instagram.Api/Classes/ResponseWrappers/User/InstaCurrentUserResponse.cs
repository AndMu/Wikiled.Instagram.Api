using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaCurrentUserResponse : InstaUserShortResponse
    {
        [JsonProperty("biography")]
        public string Biography { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        [JsonProperty("country_code")]
        public int CountryCode { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("external_url")]
        public string ExternalUrl { get; set; }

        [JsonProperty("gender")]
        public int Gender { get; set; }

        [JsonProperty("has_anonymous_profile_picture")]
        public bool HasAnonymousProfilePicture { get; set; }

        [JsonProperty("hd_profile_pic_url_info")]
        public InstaImageResponse HdProfilePicture { get; set; }

        [JsonProperty("hd_profile_pic_versions")]
        public InstaImageResponse[] HdProfilePicVersions { get; set; }

        [JsonProperty("national_number")]
        public long NationalNumber { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("show_conversion_edit_entry")]
        public bool ShowConversationEditEntry { get; set; }
    }
}