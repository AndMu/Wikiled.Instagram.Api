using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Wikiled.Instagram.Api.Classes
{
    public interface IHttpRequestProcessor
    {
        HttpClient Client { get; }

        IRequestDelay Delay { get; set; }

        HttpClientHandler HttpHandler { get; set; }

        ApiRequestMessage RequestMessage { get; }

        Task<string> GeJsonAsync(Uri requestUri);

        Task<HttpResponseMessage> GetAsync(Uri requestUri);

        Task<string> SendAndGetJsonAsync(HttpRequestMessage requestMessage, HttpCompletionOption completionOption);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, HttpCompletionOption completionOption);

        void SetHttpClientHandler(HttpClientHandler handler);
    }
}
