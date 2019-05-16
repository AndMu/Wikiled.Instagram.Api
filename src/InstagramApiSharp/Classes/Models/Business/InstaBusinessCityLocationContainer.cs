using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    internal class InstaBusinessCityLocationContainer
    {
        [JsonExtensionData] internal IDictionary<string, JToken> Extras { get; set; }
    }
}
