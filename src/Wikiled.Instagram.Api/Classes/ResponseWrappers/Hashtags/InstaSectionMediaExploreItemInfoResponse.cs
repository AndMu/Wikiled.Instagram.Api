using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class InstaSectionMediaExploreItemInfoResponse
    {
        [JsonProperty("aspect_ratio")] public float AspectRatio { get; set; }

        [JsonProperty("autoplay")] public bool Autoplay { get; set; }

        [JsonProperty("num_columns")] public int NumBolumns { get; set; }

        [JsonProperty("total_num_columns")] public int TotalNumBolumns { get; set; }
    }
}
