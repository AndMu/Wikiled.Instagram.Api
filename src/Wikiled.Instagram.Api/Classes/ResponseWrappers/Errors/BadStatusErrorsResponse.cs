using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors
{
    public class BadStatusErrorsResponse : BaseStatusResponse
    {
        [JsonProperty("message")] public MessageErrorsResponse Message { get; set; }
    }
}
