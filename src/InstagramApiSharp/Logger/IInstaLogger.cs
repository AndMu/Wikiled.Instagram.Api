using System;
using System.Net.Http;

namespace Wikiled.Instagram.Api.Logger
{
    public interface IInstaLogger
    {
        void LogException(Exception exception);

        void LogInfo(string info);

        void LogRequest(HttpRequestMessage request);

        void LogRequest(Uri uri);

        void LogResponse(HttpResponseMessage response);
    }
}
