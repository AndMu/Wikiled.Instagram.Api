using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Logic.Builder
{
    public class InstaApiBuilder : IInstaApiBuilder
    {
        private InstaApiVersionType? apiVersionType;

        private ILoggerFactory loggerFactory;

        private IRequestDelay delay = RequestDelay.Empty();

        private AndroidDevice device;

        private HttpClient httpClient;

        private HttpClientHandler httpHandler = new HttpClientHandler();

        private IHttpRequestProcessor httpRequestProcessor;

        private ApiRequestMessage requestMessage;

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
                    device = AndroidDeviceGenerator.GetRandomAndroidDevice();
                }

                requestMessage = new ApiRequestMessage
                {
                    PhoneId = device.PhoneGuid.ToString(),
                    Guid = device.DeviceGuid,
                    Password = user?.Password,
                    Username = user?.UserName,
                    DeviceId = ApiRequestMessage.GenerateDeviceId(),
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
                InstaApiConstants.TimezoneOffset = (int)DateTimeOffset.Now.Offset.TotalSeconds;
            }
            catch
            {
            }

            if (httpRequestProcessor == null)
            {
                httpRequestProcessor = new HttpRequestProcessor(delay, httpClient, httpHandler, requestMessage, loggerFactory?.CreateLogger<HttpRequestProcessor>());
            }

            if (apiVersionType == null)
            {
                apiVersionType = InstaApiVersionType.Version86;
            }

            var instaApi = new InstaApi(loggerFactory?.CreateLogger<InstaApi>(), httpRequestProcessor, user, apiVersionType.Value, device);
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
        public IInstaApiBuilder SetApiRequestMessage(ApiRequestMessage requestMessage)
        {
            this.requestMessage = requestMessage;
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

        public IInstaApiBuilder UseLogger(ILoggerFactory logger)
        {
            loggerFactory = logger;
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