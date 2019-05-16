using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Wikiled.Instagram.Api.Logger
{
    public static class LoggerExtension
    {
        public static async Task LogRequest(this ILogger logger, HttpRequestMessage request)
        {
            if (!logger.IsEnabled(LogLevel.Trace))
            {
                return;
            }

            var parser = new RequestLogger(request);
            var text = await parser.Build().ConfigureAwait(false);
            logger.LogTrace(text);
        }

        public static void LogRequest(this ILogger logger, Uri uri)
        {
            logger.LogTrace("Request: {0}", uri);
        }

        public static async Task LogResponse(this ILogger logger, HttpResponseMessage response)
        {
            if (!logger.IsEnabled(LogLevel.Trace))
            {
                return;
            }

            var parser = new ResponseLogger(response);
            logger.LogTrace(await parser.Build().ConfigureAwait(false));
        }
    }
}
