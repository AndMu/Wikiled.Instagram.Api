using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Shopping
{
    public class InstaProductImageResponse
    {
        [JsonProperty("image_versions2")] public InstaImageCandidatesResponse Images { get; set; }
    }
}
