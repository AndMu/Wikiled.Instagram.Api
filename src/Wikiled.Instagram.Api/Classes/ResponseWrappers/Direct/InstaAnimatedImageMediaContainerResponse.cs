using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaAnimatedImageMediaContainerResponse
    {
        [JsonProperty("fixed_height")]
        public InstaAnimatedImageMediaResponse Media { get; set; }
    }
}