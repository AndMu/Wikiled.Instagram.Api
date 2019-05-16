using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Shopping
{
    public class InstaProductTagsContainerResponse
    {
        [JsonProperty("in")] public List<InstaProductContainerResponse> In { get; set; }
    }
}
