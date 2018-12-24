using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Unicorn.Backend.Services.Rest
{
    public class RestResponse : HttpResponse
    {
        public RestResponse() : base()
        {
        }

        public RestResponse(HttpStatusCode status, WebHeaderCollection headers, string body)
            : base(status, headers, body)
        {
        }

        public JObject AsJObject => JObject.Parse(this.Body);
    }
}
