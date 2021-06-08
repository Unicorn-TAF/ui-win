using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Unicorn.Backend.Services.RestService
{
    /// <summary>
    /// Describes base REST service response object.
    /// </summary>
    public class RestResponse : ServiceResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestResponse"/> class.
        /// </summary>
        public RestResponse() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestResponse"/> class with status code, message and headers.
        /// </summary>
        /// <param name="status">response status code</param>
        /// <param name="headers">response headers</param>
        /// <param name="statusDescription">response status description</param>
        public RestResponse(HttpStatusCode status, HttpResponseHeaders headers, string statusDescription)
            : base(status, headers, statusDescription)
        {
        }

        /// <summary>
        /// Gets response content in form of <see cref="JObject"/>.
        /// </summary>
        public JObject AsJObject => JObject.Parse(Content);
    }
}
