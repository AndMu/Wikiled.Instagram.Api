using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaCheckEmailRegistration
    {
        [JsonProperty("available")] public bool Available { get; set; }

        [JsonProperty("confirmed")] public bool Confirmed { get; set; }

        [JsonProperty("error_type")] public string ErrorType { get; set; }

        [JsonProperty("gdpr_required")] public bool GdprRequired { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("suggestions_with_metadata")]
        public InstaRegistrationSuggestionsList SuggestionsWithMetadata { get; set; }

        [JsonProperty("tos_version")] public string TosVersion { get; set; }

        [JsonProperty("username_suggestions_with_metadata")]
        public InstaRegistrationSuggestionsList UsernameSuggestionsWithMetadata { get; set; }

        [JsonProperty("valid")] public bool Valid { get; set; }
    }
}
