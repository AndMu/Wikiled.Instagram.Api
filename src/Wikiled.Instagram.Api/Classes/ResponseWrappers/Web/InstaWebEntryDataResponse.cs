using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Web
{
    public class InstaWebEntryDataResponse
    {
        [JsonProperty("SettingsPages")]
        public List<InstaWebSettingsPageResponse> SettingsPages { get; set; } =
            new List<InstaWebSettingsPageResponse>();
    }
}