using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Challenge
{
    public class ChallengeRequireStepDataSMSVerify
    {
        [JsonProperty("contact_point")] public string ContactPoint { get; set; }

        [JsonProperty("form_type")] public string FormType { get; set; }

        [JsonProperty("phone_number_preview")] public string PhoneNumberPreview { get; set; }

        [JsonProperty("resend_delay")] public int ResendDelay { get; set; }

        [JsonProperty("security_code")] public string SecurityCode { get; set; }

        [JsonProperty("sms_resend_delay")] public int SmsResendDelay { get; set; }
    }
}
