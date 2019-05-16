﻿using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Location
{
    public class InstaPlaceShortResponse
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("external_source")]
        public string ExternalSource { get; set; }

        [JsonProperty("facebook_places_id")]
        public long FacebookPlacesId { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pk")]
        public long Pk { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }
    }
}