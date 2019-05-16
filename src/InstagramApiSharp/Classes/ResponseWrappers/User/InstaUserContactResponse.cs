using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserContactResponse : InstaUserShortResponse
    {
        [JsonProperty("extra_display_name")] public string ExtraDisplayName { get; set; }
    }
}
