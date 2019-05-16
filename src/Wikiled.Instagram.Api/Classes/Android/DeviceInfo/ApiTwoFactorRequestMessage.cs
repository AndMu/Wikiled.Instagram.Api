using Newtonsoft.Json;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logic.Versions;

namespace Wikiled.Instagram.Api.Classes.Android.DeviceInfo
{
    internal class InstaApiTwoFactorRequestMessage
    {
        internal InstaApiTwoFactorRequestMessage(
            string verificationCode,
            string username,
            string deviceId,
            string twoFactorIdentifier)
        {
            VerificationCode = verificationCode;
            this.Username = username;
            DeviceId = deviceId;
            TwoFactorIdentifier = twoFactorIdentifier;
        }

        public string DeviceId { get; set; }

        public string TwoFactorIdentifier { get; set; }

        public string Username { get; set; }

        public string VerificationCode { get; set; }

        internal string GenerateSignature(InstaApiVersion apiVersion, string signatureKey)
        {
            if (string.IsNullOrEmpty(signatureKey))
            {
                signatureKey = apiVersion.SignatureKey;
            }

            return InstaCryptoHelper.CalculateHash(
                signatureKey,
                JsonConvert.SerializeObject(this));
        }

        internal string GetMessageString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}