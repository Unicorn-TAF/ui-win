using System;
using System.Net;
using System.Net.Http;

namespace Unicorn.Backend.Services.RestService
{
    internal static class HttpUtils
    {
        internal static CookieCollection GetCookieCollectionFrom(HttpRequestMessage request)
        {
            CookieCollection cookies = new CookieCollection();

            foreach (var header in request.Headers)
            {
                foreach (var headerValue in header.Value)
                {
                    if (headerValue.Contains(";"))
                    {
                        // if header contains multiple cookies
                        var headerParts = headerValue.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var part in headerParts)
                        {
                            var pair = part.Trim().Split('=');
                            cookies.Add(GetCookie(pair[0], pair[1], request.RequestUri.Host));
                        }
                    }
                    else
                    {
                        // if header contains just one cookie
                        cookies.Add(GetCookie(header.Key, headerValue, request.RequestUri.Host));
                    }
                }
            }

            return cookies;

            Cookie GetCookie(string key, string value, string host) =>
                new Cookie(key, value)
                {
                    Domain = host
                };
        }
    }
}
