using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Classes
{
    internal class HttpRequestProcessor : IHttpRequestProcessor
    {
        private readonly ILogger logger;

        public HttpRequestProcessor(
            IRequestDelay delay,
            HttpClient httpClient,
            HttpClientHandler httpHandler,
            ApiRequestMessage requestMessage,
            ILogger logger)
        {
            Delay = delay;
            Client = httpClient;
            HttpHandler = httpHandler;
            RequestMessage = requestMessage;
            this.logger = logger;
        }

        public HttpClient Client { get; set; }

        public IRequestDelay Delay { get; set; }

        public HttpClientHandler HttpHandler { get; set; }

        public ApiRequestMessage RequestMessage { get; }

        public async Task<string> GeJsonAsync(Uri requestUri)
        {
            logger?.LogRequest(requestUri);
            if (Delay.Exist)
            {
                await Task.Delay(Delay.Value).ConfigureAwait(false);
            }

            var response = await Client.GetAsync(requestUri).ConfigureAwait(false);
            LogHttpResponse(response);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            logger?.LogRequest(requestUri);
            if (Delay.Exist)
            {
                await Task.Delay(Delay.Value).ConfigureAwait(false);
            }

            var response = await Client.GetAsync(requestUri).ConfigureAwait(false);
            LogHttpResponse(response);
            return response;
        }

        public async Task<string> SendAndGetJsonAsync(
            HttpRequestMessage requestMessage,
            HttpCompletionOption completionOption)
        {
            LogHttpRequest(requestMessage);
            if (Delay.Exist)
            {
                await Task.Delay(Delay.Value).ConfigureAwait(false);
            }

            var response = await Client.SendAsync(requestMessage, completionOption).ConfigureAwait(false);
            LogHttpResponse(response);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            LogHttpRequest(requestMessage);
            if (Delay.Exist)
            {
                await Task.Delay(Delay.Value).ConfigureAwait(false);
            }

            var response = await Client.SendAsync(requestMessage).ConfigureAwait(false);
            LogHttpResponse(response);
            return response;
        }

        public async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage requestMessage,
            HttpCompletionOption completionOption)
        {
            LogHttpRequest(requestMessage);
            if (Delay.Exist)
            {
                await Task.Delay(Delay.Value).ConfigureAwait(false);
            }

            var response = await Client.SendAsync(requestMessage, completionOption).ConfigureAwait(false);
            LogHttpResponse(response);
            return response;
        }

        public void SetHttpClientHandler(HttpClientHandler handler)
        {
            HttpHandler = handler;
            Client = new HttpClient(handler);
        }

        private void LogHttpRequest(HttpRequestMessage request)
        {
            logger?.LogRequest(request);
        }

        private void LogHttpResponse(HttpResponseMessage request)
        {
            logger?.LogResponse(request);
        }
    }
}