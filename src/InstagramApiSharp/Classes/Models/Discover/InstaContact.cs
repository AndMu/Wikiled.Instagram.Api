using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Discover
{
    public class InstaContact
    {
        [JsonProperty("email_addresses")] public List<string> EmailAddresses { get; set; }

        [JsonProperty("first_name")] public string FirstName { get; set; }

        [JsonProperty("last_name")] public string LastName { get; set; }

        [JsonProperty("phone_numbers")] public List<string> PhoneNumbers { get; set; }
    }
}
