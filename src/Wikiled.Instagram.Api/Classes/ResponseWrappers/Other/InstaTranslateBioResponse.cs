using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Other
{
    public class InstaTranslateBioResponse : InstaDefault
    {
        [JsonProperty("translation")]
        public string Translation { get; set; }
    }
}