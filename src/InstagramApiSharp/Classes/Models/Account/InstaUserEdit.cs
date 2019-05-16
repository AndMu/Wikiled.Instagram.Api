using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaUserEdit
    {
        [JsonProperty("allowed_commenter_type")]
        public string AllowedCommenterType { get; set; }

        [JsonProperty("biography")] public string Biography { get; set; }

        [JsonProperty("biography_with_entities")]
        public InstaBiographyEntities BiographyWithEntities { get; set; }

        [JsonProperty("birthday")] public object Birthday { get; set; }

        [JsonProperty("can_boost_post")] public bool CanBoostPost { get; set; }

        [JsonProperty("can_link_entities_in_bio")]
        public bool CanLinkEntitiesInBio { get; set; }

        [JsonProperty("can_see_organic_insights")]
        public bool CanSeeOrganicInsights { get; set; }

        [JsonProperty("country_code")] public int CountryCode { get; set; }

        [JsonProperty("email")] public string Email { get; set; }

        [JsonProperty("external_lynx_url")] public string ExternalLynxUrl { get; set; }

        [JsonProperty("external_url")] public string ExternalUrl { get; set; }

        [JsonProperty("full_name")] public string FullName { get; set; }

        [JsonIgnore] public InstaGenderType Gender => (InstaGenderType)GenderNum;

        [JsonProperty("has_anonymous_profile_picture")]
        public bool HasAnonymousProfilePicture { get; set; }

        [JsonProperty("has_biography_translation")]
        public bool HasBiographyTranslation { get; set; }

        [JsonProperty("has_persistent_profile_school")]
        public bool HasPersistentProfileSchool { get; set; }

        [JsonProperty("has_placed_orders")] public bool HasPlacedOrders { get; set; }

        [JsonProperty("is_eligible_for_school")]
        public bool IsEligibleForSchool { get; set; }

        [JsonProperty("is_private")] public bool IsPrivate { get; set; }

        [JsonProperty("is_verified")] public bool IsVerified { get; set; }

        [JsonProperty("max_num_linked_entities_in_bio")]
        public int MaxNumLinkedEntitiesInBio { get; set; }

        [JsonProperty("national_number")] public long NationalNumber { get; set; }

        [JsonProperty("phone_number")] public string PhoneNumber { get; set; }

        [JsonProperty("pk")] public long Pk { get; set; }

        [JsonProperty("profile_pic_id")] public string ProfilePicId { get; set; }

        [JsonProperty("hd_profile_pic_versions")]
        public ImageResponse[] ProfilePicture { get; set; }

        [JsonProperty("hd_profile_pic_url_info")]
        public ImageResponse ProfilePictureUrlInfo { get; set; }

        [JsonProperty("profile_pic_url")] public string ProfilePicUrl { get; set; }

        [JsonProperty("reel_auto_archive")] public string ReelAutoArchive { get; set; }

        [JsonProperty("show_conversion_edit_entry")]
        public bool ShowConversionEditEntry { get; set; }

        [JsonProperty("show_insights_terms")] public bool ShowInsightsTerms { get; set; }

        [JsonProperty("username")] public string Username { get; set; }

        [JsonProperty("gender")] internal int GenderNum { get; set; }
    }
}
