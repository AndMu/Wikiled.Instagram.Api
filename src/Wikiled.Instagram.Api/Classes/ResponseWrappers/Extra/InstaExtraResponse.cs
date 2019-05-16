using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Extra
{
    internal class InstaExtraResponse
    {
        [JsonExtensionData] internal IDictionary<string, JToken> Extras { get; set; }
    }
}
