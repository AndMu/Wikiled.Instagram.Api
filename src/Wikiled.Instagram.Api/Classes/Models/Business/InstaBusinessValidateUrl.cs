using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaBusinessValidateUrl
    {
        [JsonProperty("error_msg")]
        public string ErrorMessage { get; set; }

        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
    }
}