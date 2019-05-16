using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Challenge
{
    public class InstaLoggedInChallengeDataInfo
    {
        [JsonProperty("choice")] public string Choice { get; set; }

        [JsonProperty("city")] public string City { get; set; }

        [JsonProperty("country")] public string Country { get; set; }

        [JsonProperty("enrollment_date")] public string EnrollmentDate { get; set; }

        [JsonProperty("enrollment_time")] public long? EnrollmentTime { get; set; }

        [JsonProperty("latitude")] public float Latitude { get; set; }

        [JsonProperty("longitude")] public float Longitude { get; set; }

        [JsonProperty("platform")] public string Platform { get; set; }

        [JsonProperty("user_agent")] public string UserAgent { get; set; }
    }
}
