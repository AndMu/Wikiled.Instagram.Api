using System;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logic.Versions;

namespace Wikiled.Instagram.Api.Classes.Android.DeviceInfo
{
    public class ApiRequestMessage
    {
        private static readonly Random Rnd = new Random();

        public static ApiRequestMessage CurrentDevice { get; private set; }

        [JsonProperty("adid")]
        public string AdId { get; set; }

        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("google_tokens")]
        public string GoogleTokens { get; set; } = "[]";

        [JsonProperty("guid")]
        public Guid Guid { get; set; }

        [JsonProperty("login_attempt_count")]
        public string LoginAttemptCount { get; set; } = "1";

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("phone_id")]
        public string PhoneId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("_uuid")]
        public string Uuid => Guid.ToString();

        public static ApiRequestMessage FromDevice(AndroidDevice device)
        {
            var requestMessage = new ApiRequestMessage
            {
                PhoneId = device.PhoneGuid.ToString(), Guid = device.DeviceGuid, DeviceId = device.DeviceId
            };
            return requestMessage;
        }

        public static string GenerateDeviceIdFromGuid(Guid guid)
        {
            var hashedGuid = InstaCryptoHelper.CalculateMd5(guid.ToString());
            return $"android-{hashedGuid.Substring(0, 16)}";
        }

        internal static string GenerateDeviceId()
        {
            return GenerateDeviceIdFromGuid(Guid.NewGuid());
        }

        //internal static string GenerateRandomUploadIdOLD()
        //{
        //    var total = GenerateUploadId();
        //    var uploadId = total + rnd.Next(11111, 99999).ToString("D5");
        //    return uploadId;
        //}
        internal static string GenerateRandomUploadId()
        {
            return DateTime.UtcNow.ToUnixTimeMiliSeconds().ToString();
        }

        internal static string GenerateUploadId()
        {
            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            var uploadId = (long)timeSpan.TotalSeconds;
            return uploadId.ToString();
        }

        internal string GenerateChallengeSignature(InstaApiVersion apiVersion,
                                                   string signatureKey,
                                                   string csrfToken,
                                                   out string deviceid)
        {
            if (string.IsNullOrEmpty(signatureKey))
            {
                signatureKey = apiVersion.SignatureKey;
            }

            var api = new InstaApiRequestChallengeMessage
            {
                CsrtToken = csrfToken,
                DeviceId = DeviceId,
                Guid = Guid,
                LoginAttemptCount = "1",
                Password = Password,
                PhoneId = PhoneId,
                Username = Username,
                AdId = AdId
            };
            var res = InstaCryptoHelper.CalculateHash(
                signatureKey,
                JsonConvert.SerializeObject(api));
            deviceid = DeviceId;
            return res;
        }

        internal string GenerateSignature(InstaApiVersion apiVersion, string signatureKey, out string deviceid)
        {
            if (string.IsNullOrEmpty(signatureKey))
            {
                signatureKey = apiVersion.SignatureKey;
            }

            var res = InstaCryptoHelper.CalculateHash(
                signatureKey,
                JsonConvert.SerializeObject(this));
            deviceid = DeviceId;
            return res;
        }

        internal string GetChallengeMessageString(string csrfToken)
        {
            var api = new InstaApiRequestChallengeMessage
            {
                CsrtToken = csrfToken,
                DeviceId = DeviceId,
                Guid = Guid,
                LoginAttemptCount = "1",
                Password = Password,
                PhoneId = PhoneId,
                Username = Username,
                AdId = AdId
            };
            var json = JsonConvert.SerializeObject(api);
            return json;
        }

        internal string GetChallengeVerificationCodeSend(string verify)
        {
            return JsonConvert.SerializeObject(new
            {
                security_code = verify,
                _csrftoken = "ReplaceCSRF",
                Guid,
                DeviceId
            });
        }

        internal string GetMessageString()
        {
            var json = JsonConvert.SerializeObject(this);
            return json;
        }

        internal string GetMessageStringForChallengeVerificationCodeSend(int choice = 1)
        {
            return JsonConvert.SerializeObject(new
            {
                choice = choice.ToString(),
                _csrftoken = "ReplaceCSRF",
                Guid,
                DeviceId
            });
        }

        internal bool IsEmpty()
        {
            if (string.IsNullOrEmpty(PhoneId))
            {
                return true;
            }

            if (string.IsNullOrEmpty(DeviceId))
            {
                return true;
            }

            if (Guid.Empty == Guid)
            {
                return true;
            }

            return false;
        }
    }
}