using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Logic.Versions;

namespace Wikiled.Instagram.Api.Helpers
{
    internal class InstaHttpHelper
    {
        public /*readonly*/ InstaApiVersion ApiVersion;

        internal InstaHttpHelper(InstaApiVersion apiVersionType)
        {
            ApiVersion = apiVersionType;
        }

        public HttpRequestMessage GetDefaultRequest(HttpMethod method, Uri uri, AndroidDevice deviceInfo)
        {
            var userAgent = deviceInfo.GenerateUserAgent(ApiVersion);
            var request = new HttpRequestMessage(method, uri);
            request.Headers.Add(InstaApiConstants.HeaderAcceptLanguage, InstaApiConstants.AcceptLanguage);
            request.Headers.Add(InstaApiConstants.HeaderIgCapabilities, ApiVersion.Capabilities);
            request.Headers.Add(InstaApiConstants.HeaderIgConnectionType, InstaApiConstants.IgConnectionType);
            request.Headers.Add(InstaApiConstants.HeaderUserAgent, userAgent);
            request.Headers.Add(InstaApiConstants.HeaderIgAppId, InstaApiConstants.IgAppId);
            request.Properties.Add(
                new KeyValuePair<string, object>(
                    InstaApiConstants.HeaderXgoogleAdIde,
                    deviceInfo.GoogleAdId.ToString()));
            return request;
        }

        public HttpRequestMessage GetDefaultRequest(HttpMethod method,
                                                    Uri uri,
                                                    AndroidDevice deviceInfo,
                                                    Dictionary<string, string> data)
        {
            var request = GetDefaultRequest(HttpMethod.Post, uri, deviceInfo);
            request.Content = new FormUrlEncodedContent(data);
            return request;
        }

        public string GetSignature(JObject data)
        {
            var hash = InstaCryptoHelper.CalculateHash(ApiVersion.SignatureKey, data.ToString(Formatting.None));
            var payload = data.ToString(Formatting.None);
            var signature = $"{hash}.{payload}";
            return signature;
        }

        public HttpRequestMessage GetSignedRequest(
            HttpMethod method,
            Uri uri,
            AndroidDevice deviceInfo,
            Dictionary<string, string> data)
        {
            var hash = InstaCryptoHelper.CalculateHash(
                ApiVersion.SignatureKey,
                JsonConvert.SerializeObject(data));
            var payload = JsonConvert.SerializeObject(data);
            var signature = $"{hash}.{payload}";

            var fields = new Dictionary<string, string>
            {
                { InstaApiConstants.HeaderIgSignature, signature },
                { InstaApiConstants.HeaderIgSignatureKeyVersion, InstaApiConstants.IgSignatureKeyVersion }
            };
            var request = GetDefaultRequest(HttpMethod.Post, uri, deviceInfo);
            request.Content = new FormUrlEncodedContent(fields);
            request.Properties.Add(InstaApiConstants.HeaderIgSignature, signature);
            request.Properties.Add(
                InstaApiConstants.HeaderIgSignatureKeyVersion,
                InstaApiConstants.IgSignatureKeyVersion);
            return request;
        }

        public HttpRequestMessage GetSignedRequest(
            HttpMethod method,
            Uri uri,
            AndroidDevice deviceInfo,
            Dictionary<string, int> data)
        {
            var hash = InstaCryptoHelper.CalculateHash(
                ApiVersion.SignatureKey,
                JsonConvert.SerializeObject(data));
            var payload = JsonConvert.SerializeObject(data);
            var signature = $"{hash}.{payload}";

            var fields = new Dictionary<string, string>
            {
                { InstaApiConstants.HeaderIgSignature, signature },
                { InstaApiConstants.HeaderIgSignatureKeyVersion, InstaApiConstants.IgSignatureKeyVersion }
            };
            var request = GetDefaultRequest(HttpMethod.Post, uri, deviceInfo);
            request.Content = new FormUrlEncodedContent(fields);
            request.Properties.Add(InstaApiConstants.HeaderIgSignature, signature);
            request.Properties.Add(
                InstaApiConstants.HeaderIgSignatureKeyVersion,
                InstaApiConstants.IgSignatureKeyVersion);
            return request;
        }

        public HttpRequestMessage GetSignedRequest(
            HttpMethod method,
            Uri uri,
            AndroidDevice deviceInfo,
            Dictionary<string, object> data)
        {
            var hash = InstaCryptoHelper.CalculateHash(
                ApiVersion.SignatureKey,
                JsonConvert.SerializeObject(data));
            var payload = JsonConvert.SerializeObject(data);
            var signature = $"{hash}.{payload}";

            var fields = new Dictionary<string, string>
            {
                { InstaApiConstants.HeaderIgSignature, signature },
                { InstaApiConstants.HeaderIgSignatureKeyVersion, InstaApiConstants.IgSignatureKeyVersion }
            };
            var request = GetDefaultRequest(HttpMethod.Post, uri, deviceInfo);
            request.Content = new FormUrlEncodedContent(fields);
            request.Properties.Add(InstaApiConstants.HeaderIgSignature, signature);
            request.Properties.Add(
                InstaApiConstants.HeaderIgSignatureKeyVersion,
                InstaApiConstants.IgSignatureKeyVersion);
            return request;
        }

        public HttpRequestMessage GetSignedRequest(
            HttpMethod method,
            Uri uri,
            AndroidDevice deviceInfo,
            JObject data)
        {
            var hash = InstaCryptoHelper.CalculateHash(
                ApiVersion.SignatureKey,
                data.ToString(Formatting.None));
            var payload = data.ToString(Formatting.None);
            var signature = $"{hash}.{payload}";
            var fields = new Dictionary<string, string>
            {
                { InstaApiConstants.HeaderIgSignature, signature },
                { InstaApiConstants.HeaderIgSignatureKeyVersion, InstaApiConstants.IgSignatureKeyVersion }
            };
            var request = GetDefaultRequest(HttpMethod.Post, uri, deviceInfo);
            request.Content = new FormUrlEncodedContent(fields);
            request.Properties.Add(InstaApiConstants.HeaderIgSignature, signature);
            request.Properties.Add(
                InstaApiConstants.HeaderIgSignatureKeyVersion,
                InstaApiConstants.IgSignatureKeyVersion);
            return request;
        }

        /// <summary>
        ///     This is only for https://instagram.com site
        /// </summary>
        public HttpRequestMessage GetWebRequest(HttpMethod method, Uri uri, AndroidDevice deviceInfo)
        {
            var request = GetDefaultRequest(HttpMethod.Get, uri, deviceInfo);
            request.Headers.Remove(InstaApiConstants.HeaderUserAgent);
            request.Headers.Add(InstaApiConstants.HeaderUserAgent, InstaApiConstants.WebUserAgent);
            return request;
        }
    }
}