using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Shopping
{
    public class InstaMerchantResponse
    {
        [JsonProperty("pk")]
        public long Pk { get; set; }

        [JsonProperty("profile_pic_url")]
        public string ProfilePicture { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}