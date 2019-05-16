using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaLastSeenAtResponse
    {
        [JsonExtensionData] internal IDictionary<string, JToken> Extras { get; set; }
    }
}
