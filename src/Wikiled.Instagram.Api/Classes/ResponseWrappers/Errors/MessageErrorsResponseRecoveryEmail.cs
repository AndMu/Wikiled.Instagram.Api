using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors
{
    public class MessageErrorsResponseRecoveryEmail : BaseStatusResponse
    {
        [JsonProperty("message")] public string Message { get; set; }
    }
}
