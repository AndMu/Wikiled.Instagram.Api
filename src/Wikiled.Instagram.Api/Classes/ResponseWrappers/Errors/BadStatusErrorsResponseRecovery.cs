using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors
{
    public class BadStatusErrorsResponseRecovery : BaseStatusResponse
    {
        [JsonProperty("errors")] public MessageErrorsResponsePhone PhoneNumber { get; set; }
    }
}
