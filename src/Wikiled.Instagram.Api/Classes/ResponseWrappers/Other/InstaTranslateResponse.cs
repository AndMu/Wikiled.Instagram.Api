using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Other
{
    public class InstaTranslateResponse
    {
        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("translation")] public string Translation { get; set; }
    }
}
