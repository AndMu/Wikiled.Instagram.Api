using Microsoft.Extensions.Logging;
using System.Net.Http;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.SessionHandlers;

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
        IInstaApiBuilder SetApiRequestMessage(ApiRequestMessage requestMessage);

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
        /// <param name="logger">ILogger implementation</param>
        /// <returns>API Builder</returns>
        IInstaApiBuilder UseLogger(ILoggerFactory logger);
    }
}