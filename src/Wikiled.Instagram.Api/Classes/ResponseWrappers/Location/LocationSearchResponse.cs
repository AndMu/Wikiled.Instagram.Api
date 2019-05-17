using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Location
{
    public class LocationSearchResponse
    {
        [JsonProperty("venues")]
        public List<LocationShortResponse> Locations { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}