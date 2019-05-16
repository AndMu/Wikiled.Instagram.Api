using System;
using System.Net;

namespace Wikiled.Instagram.Api.Helpers
{
    public class InstaProxy : IWebProxy
    {
        private readonly string ipaddress;

        private readonly string port;

        public InstaProxy(string ipaddress, string port)
        {
            this.ipaddress = ipaddress;
            this.port = port;
        }

        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination)
        {
            return new Uri($"http://{ipaddress}:{port}");
        }

        public bool IsBypassed(Uri host)
        {
            return false;
        }
    }
}