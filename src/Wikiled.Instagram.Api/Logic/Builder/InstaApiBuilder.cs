using System;
using System.Net.Http;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Builder
{
    public class InstaApiBuilder : IInstaApiBuilder
    {
        private InstaApiVersionType? apiVersionType;

        private IRequestDelay delay = RequestDelay.Empty();

        private InstaAndroidDevice device;

        private HttpClient httpClient;

        private HttpClientHandler httpHandler = new HttpClientHandler();

        private IHttpRequestProcessor httpRequestProcessor;

        private IInstaLogger logger;

        private InstaApiRequestMessage requestMessage;

        private ISessionHandler sessionHandler;

        private UserSessionData user;

        private InstaApiBuilder()
        {
        }

        /// <summary>
        ///     Create new API instance
        /// </summary>
        /// <returns>
        ///     API instance
        /// </returns>
        /// <exception cref="ArgumentNullException">User auth data must be specified</exception>
        public IInstaApi Build()
        {
            if (user == null)
            {
                user = UserSessionData.Empty;
            }

            if (httpHandler == null)
            {
                httpHandler = new HttpClientHandler();
            }

            if (httpClient == null)
            {
                httpClient = new HttpClient(httpHandler) { BaseAddress = new Uri(InstaApiConstants.InstagramUrl) };
            }

            if (requestMessage == null)
            {
                if (device == null)
                {
                    device = InstaAndroidDeviceGenerator.GetRandomAndroidDevice();
                }

                requestMessage = new InstaApiRequestMessage
                {
                    PhoneId = device.PhoneGuid.ToString(),
                    Guid = device.DeviceGuid,
                    Password = user?.Password,
                    Username = user?.UserName,
                    DeviceId = InstaApiRequestMessage.GenerateDeviceId(),
                    AdId = device.AdId.ToString()
                };
            }

            if (string.IsNullOrEmpty(requestMessage.Password))
            {
                requestMessage.Password = user?.Password;
            }

            if (string.IsNullOrEmpty(requestMessage.Username))
            {
                requestMessage.Username = user?.UserName;
            }

            try
            {
                InstaApiConstants.TimezoneOffset = int.Parse(DateTimeOffset.Now.Offset.TotalSeconds.ToString());
            }
            catch
            {
            }

            if (httpRequestProcessor == null)
            {
                httpRequestProcessor =
                    new InstaHttpRequestProcessor(delay, httpClient, httpHandler, requestMessage, logger);
            }

            if (apiVersionType == null)
            {
                apiVersionType = InstaApiVersionType.Version86;
            }

            var instaApi = new InstaApi(user, logger, device, httpRequestProcessor, apiVersionType.Value);
            if (sessionHandler != null)
            {
                sessionHandler.Api = instaApi;
                instaApi.SessionHandler = sessionHandler;
            }

            return instaApi;
        }

        /// <summary>
        ///     Set custom request message. Used to be able to customize device info.
        /// </summary>
        /// <param name="requestMessage">Custom request message object</param>
        /// <returns>
        ///     API Builder
        /// </returns>
        /// <remarks>
        ///     Please, do not use if you don't know what you are doing
        /// </remarks>
        public IInstaApiBuilder SetApiRequestMessage(InstaApiRequestMessage requestMessage)
        {
            this.requestMessage = requestMessage;
            return this;
        }

        /// <summary>
        ///     Set instagram api version (for user agent version)
        /// </summary>
        /// <param name="apiVersion">Api version</param>
        /// <returns>
        ///     API Builder
        /// </returns>
        public IInstaApiBuilder SetApiVersion(InstaApiVersionType apiVersion)
        {
            apiVersionType = apiVersion;
            return this;
        }

        /// <summary>
        ///     Set custom android device.
        ///     <para>Note: this is optional, if you didn't set this, InstagramApiSharp will choose random device.</para>
        /// </summary>
        /// <param name="androidDevice">Android device</param>
        /// <returns>
        ///     API Builder
        /// </returns>
        public IInstaApiBuilder SetDevice(InstaAndroidDevice androidDevice)
        {
            device = androidDevice;
            return this;
        }

        /// <summary>
        ///     Set Http request processor
        /// </summary>
        /// <param name="httpRequestProcessor">HttpRequestProcessor</param>
        /// <returns>
        ///     API Builder
        /// </returns>
        public IInstaApiBuilder SetHttpRequestProcessor(IHttpRequestProcessor httpRequestProcessor)
        {
            this.httpRequestProcessor = httpRequestProcessor;
            return this;
        }

        /// <summary>
        ///     Set delay between requests. Useful when API supposed to be used for mass-bombing.
        /// </summary>
        /// <param name="delay">Timespan delay</param>
        /// <returns>
        ///     API Builder
        /// </returns>
        public IInstaApiBuilder SetRequestDelay(IRequestDelay delay)
        {
            if (delay == null)
            {
                delay = RequestDelay.Empty();
            }

            this.delay = delay;
            return this;
        }

        /// <summary>
        ///     Set session handler
        /// </summary>
        /// <param name="sessionHandler">Session handler</param>
        /// <returns>
        ///     API Builder
        /// </returns>
        public IInstaApiBuilder SetSessionHandler(ISessionHandler sessionHandler)
        {
            this.sessionHandler = sessionHandler;
            return this;
        }

        /// <summary>
        ///     Specify user login, password from here
        /// </summary>
        /// <param name="user">User auth data</param>
        /// <returns>
        ///     API Builder
        /// </returns>
        public IInstaApiBuilder SetUser(UserSessionData user)
        {
            this.user = user;
            return this;
        }

        /// <summary>
        ///     Set specific HttpClient
        /// </summary>
        /// <param name="httpClient">HttpClient</param>
        /// <returns>
        ///     API Builder
        /// </returns>
        public IInstaApiBuilder UseHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            return this;
        }

        /// <summary>
        ///     Set custom HttpClientHandler to be able to use certain features, e.g Proxy and so on
        /// </summary>
        /// <param name="handler">HttpClientHandler</param>
        /// <returns>
        ///     API Builder
        /// </returns>
        public IInstaApiBuilder UseHttpClientHandler(HttpClientHandler handler)
        {
            httpHandler = handler;
            return this;
        }

        /// <summary>
        ///     Use custom logger
        /// </summary>
        /// <param name="logger">IInstaLogger implementation</param>
        /// <returns>
        ///     API Builder
        /// </returns>
        public IInstaApiBuilder UseLogger(IInstaLogger logger)
        {
            this.logger = logger;
            return this;
        }

        /// <summary>
        ///     Creates the builder.
        /// </summary>
        /// <returns>
        ///     API Builder
        /// </returns>
        public static IInstaApiBuilder CreateBuilder()
        {
            return new InstaApiBuilder();
        }
    }
}