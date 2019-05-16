using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Logger
{
    public class ResponseLogger
    {
        private readonly HttpResponseMessage response;

        private StringBuilder builder;

        public ResponseLogger(HttpResponseMessage response)
        {
            this.response = response;
        }

        public async Task<string> Build()
        {
            builder = new StringBuilder();
            builder.AppendLine($"Response: {response.RequestMessage.Method} {response.RequestMessage.RequestUri} [{response.StatusCode}]");
            await WriteContent(response.Content, Formatting.None).ConfigureAwait(false);
            return builder.ToString();
        }

        private async Task WriteContent(HttpContent content, Formatting formatting, int maxLength = 0)
        {
            builder.AppendLine("Content:");
            var raw = await content.ReadAsStringAsync().ConfigureAwait(false);
            if (formatting == Formatting.Indented)
            {
                raw = FormatJson(raw);
            }

            raw = raw.Contains("<!DOCTYPE html>") ? "got html content!" : raw;
            if ((raw.Length > maxLength) & (maxLength != 0))
            {
                raw = raw.Substring(0, maxLength);
            }

            builder.AppendLine(raw);
        }

        private string FormatJson(string json)
        {
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }
    }
}
