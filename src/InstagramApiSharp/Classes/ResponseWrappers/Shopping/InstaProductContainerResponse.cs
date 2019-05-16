using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Shopping
{
    public class InstaProductContainerResponse
    {
        [JsonProperty("position")] public double[] Position { get; set; }

        [JsonProperty("product")] public InstaProductResponse Product { get; set; }
    }
}
