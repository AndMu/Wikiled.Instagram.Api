using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    internal class InstaBusinessCategoryContainer
    {
        [JsonExtensionData] internal IDictionary<string, JToken> Extras { get; set; }
    }
}
