using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaAccountVerifySmsErrors
    {
        [JsonProperty("verification_code")] public List<string> VerificationCode { get; set; }
    }
}
