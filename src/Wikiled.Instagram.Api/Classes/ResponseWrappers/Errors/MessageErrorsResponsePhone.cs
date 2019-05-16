using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors
{
    public class MessageErrorsResponsePhone
    {
        [JsonProperty("phone_number")] public List<string> Errors { get; set; }
    }
}
