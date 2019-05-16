using System;
using System.Net.Http;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Builder
{
    public interface IInstaApiBuilder
    {
        /// <summary>
        ///     Create new API instance
        /// </summary>
        /// <returns>API instance</returns>
        IInstaApi Build();

        /// <summary>
        ///     Set custom request message. Used to be able to customize device info.
        /// </summary>
        /// <param name="requestMessage">Custom request message object</param>
        /// <remarks>Please, do not use if you don't know what you are doing</remarks>
        /// <returns>API Builder</returns>
        IInstaApiBuilder SetApiRequestMessage(InstaApiRequestMessage requestMessage);

        [Obsolete("Deprecated. Please use IInstaApi.SetApiVersion instead.")]
        /// <summary>
        ///     Set instagram api version (for user agent version)
        /// </summary>
        /// <param name="apiVersion">Api version</param>
        IInstaApiBuilder SetApiVersion(InstaApiVersionType apiVersion);

        [Obsolete("Deprecated. Please use IInstaApi.SetDevice instead.")]
        /// <summary>
        ///     Set custom android device.
        ///     <para>Note: this is optional, if you didn't set this, <see cref="InstagramApiSharp"/> will choose random device.</para>
        /// </summary>
        /// <param name="androidDevice">Android device</param>
        /// <returns>API Builder</returns>
        IInstaApiBuilder SetDevice(InstaAndroidDevice androidDevice);

        /// <summary>
        ///     Set Http request processor
        /// </summary>
        /// <param name="httpRequestProcessor">HttpRequestProcessor</param>
        /// <returns></returns>
        IInstaApiBuilder SetHttpRequestProcessor(IHttpRequestProcessor httpRequestProcessor);

        /// <summary>
        ///     Set delay between requests. Useful when API supposed to be used for mass-bombing.
        /// </summary>
        /// <param name="delay">Timespan delay</param>
        /// <returns>API Builder</returns>
        IInstaApiBuilder SetRequestDelay(IRequestDelay delay);

        /// <summary>
        ///     Set session handler
        /// </summary>
        /// <param name="sessionHandler">Session handler</param>
        /// <returns></returns>
        IInstaApiBuilder SetSessionHandler(ISessionHandler sessionHandler);

        /// <summary>
        ///     Specify user login, password from here
        /// </summary>
        /// <param name="user">User auth data</param>
        /// <returns>API Builder</returns>
        IInstaApiBuilder SetUser(UserSessionData user);

        /// <summary>
        ///     Set specific HttpClient
        /// </summary>
        /// <param name="httpClient">HttpClient</param>
        /// <returns>API Builder</returns>
        IInstaApiBuilder UseHttpClient(HttpClient httpClient);

        /// <summary>
        ///     Set custom HttpClientHandler to be able to use certain features, e.g Proxy and so on
        /// </summary>
        /// <param name="handler">HttpClientHandler</param>
        /// <returns>API Builder</returns>
        IInstaApiBuilder UseHttpClientHandler(HttpClientHandler handler);

        /// <summary>
        ///     Use custom logger
        /// </summary>
        /// <param name="logger">IInstaLogger implementation</param>
        /// <returns>API Builder</returns>
        IInstaApiBuilder UseLogger(IInstaLogger logger);
    }
}