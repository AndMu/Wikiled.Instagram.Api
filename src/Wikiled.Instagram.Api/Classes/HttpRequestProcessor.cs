using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Wikiled.Instagram.Api.Classes
{
    internal class HttpRequestProcessor : IHttpRequestProcessor
    {
        private readonly IInstaLogger _logger;

        public HttpRequestProcessor(
            IRequestDelay delay,
            HttpClient httpClient,
            HttpClientHandler httpHandler,
            ApiRequestMessage requestMessage,
            IInstaLogger logger)
        {
            Delay = delay;
            Client = httpClient;
            HttpHandler = httpHandler;
            RequestMessage = requestMessage;
            _logger = logger;
        }

        public HttpClient Client { get; set; }

        public IRequestDelay Delay { get; set; }

        public HttpClientHandler HttpHandler { get; set; }

        public ApiRequestMessage RequestMessage { get; }

        public async Task<string> GeJsonAsync(Uri requestUri)
        {
            _logger?.LogRequest(requestUri);
            if (Delay.Exist)
            {
                await Task.Delay(Delay.Value);
            }

            var response = await Client.GetAsync(requestUri);
            LogHttpResponse(response);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            _logger?.LogRequest(requestUri);
            if (Delay.Exist)
            {
                await Task.Delay(Delay.Value);
            }

            var response = await Client.GetAsync(requestUri);
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
                await Task.Delay(Delay.Value);
            }

            var response = await Client.SendAsync(requestMessage, completionOption);
            LogHttpResponse(response);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            LogHttpRequest(requestMessage);
            if (Delay.Exist)
            {
                await Task.Delay(Delay.Value);
            }

            var response = await Client.SendAsync(requestMessage);
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
                await Task.Delay(Delay.Value);
            }

            var response = await Client.SendAsync(requestMessage, completionOption);
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
            _logger?.LogRequest(request);
        }

        private void LogHttpResponse(HttpResponseMessage request)
        {
            _logger?.LogResponse(request);
        }
    }
}
