using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors
{
    public class MessageErrorsResponse
    {
        [JsonProperty("errors")] public List<string> Errors { get; set; }
    }
}
