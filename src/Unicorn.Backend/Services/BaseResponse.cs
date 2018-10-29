using System;
using System.Net;

namespace Unicorn.Backend.Services
{
    public class HttpResponse
    {
        public HttpResponse()
        {
        }

        public HttpResponse(HttpStatusCode status, WebHeaderCollection headers, string body)
        {
        }

        public TimeSpan ExecutionTime { get; set; }

        public HttpStatusCode Status { get; set; }

        public string StatusDescription { get; set; }

        public string Body { get; set; }

        public WebHeaderCollection Headers { get; set; }
    }
}
