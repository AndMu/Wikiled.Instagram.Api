using System;

namespace Wikiled.Instagram.Api.Helpers
{
    public static class InstaHttpExtensions
    {
        public static Uri AddQueryParameter(this Uri uri, string name, string value)
        {
            if (value == null || value == "" || value == "[]")
            {
                return uri;
            }

            var httpValueCollection = InstaHttpUtility.ParseQueryString(uri);

            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);

            var ub = new UriBuilder(uri);
            var q = "";
            foreach (var item in httpValueCollection)
            {
                if (q == "")
                {
                    q += $"{item.Key}={item.Value}";
                }
                else
                {
                    q += $"&{item.Key}={item.Value}";
                }
            }

            ub.Query = q;
            return ub.Uri;
        }

        public static Uri AddQueryParameterIfNotEmpty(this Uri uri, string name, string value)
        {
            if (value == null || value == "" || value == "[]")
            {
                return uri;
            }

            var httpValueCollection = InstaHttpUtility.ParseQueryString(uri);
            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);
            var ub = new UriBuilder(uri);
            var q = "";
            foreach (var item in httpValueCollection)
            {
                if (q == "")
                {
                    q += $"{item.Key}={item.Value}";
                }
                else
                {
                    q += $"&{item.Key}={item.Value}";
                }
            }

            ub.Query = q;
            return ub.Uri;
        }
    }
}