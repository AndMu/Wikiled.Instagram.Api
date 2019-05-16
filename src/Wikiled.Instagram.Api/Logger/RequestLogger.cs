using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Wikiled.Instagram.Api.Logger
{
    public class RequestLogger
    {
        private readonly HttpRequestMessage request;

        private StringBuilder builder;

        public RequestLogger(HttpRequestMessage request)
        {
            this.request = request;
        }

        public async Task<string> Build()
        {
            builder = new StringBuilder();
            WriteSeprator();
            builder.AppendLine($"Request: {request.Method} {request.RequestUri}");
            WriteHeaders(request.Headers);
            WriteProperties(request.Properties);
            if (request.Method == HttpMethod.Post)
            {
                await WriteRequestContent(request.Content).ConfigureAwait(false);
            }

            return builder.ToString();
        }

        private async Task WriteRequestContent(HttpContent content, int maxLength = 0)
        {
            builder.AppendLine("Content:");
            var raw = await content.ReadAsStringAsync().ConfigureAwait(false);
            if ((raw.Length > maxLength) & (maxLength != 0))
            {
                raw = raw.Substring(0, maxLength);
            }

            builder.AppendLine(WebUtility.UrlDecode(raw));
        }

        private void WriteProperties(IDictionary<string, object> properties)
        {
            if (properties == null)
            {
                return;
            }

            if (properties.Count == 0)
            {
                return;
            }

            builder.AppendLine($"Properties:\n{JsonConvert.SerializeObject(properties, Formatting.Indented)}");
        }

        private void WriteSeprator()
        {
            var sep = new StringBuilder();
            for (var i = 0; i < 100; i++)
            {
                sep.Append("-");
            }

            sep.AppendLine();
        }

        private void WriteHeaders(HttpHeaders headers)
        {
            if (headers == null)
            {
                return;
            }

            if (!headers.Any())
            {
                return;
            }

            builder.AppendLine("Headers:");
            foreach (var item in headers)
            {
                builder.AppendLine($"{item.Key}:{JsonConvert.SerializeObject(item.Value)}");
            }
        }
    }
}