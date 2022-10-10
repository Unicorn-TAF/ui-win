using System;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unicorn.Taf.Core.Logging;

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
        /// <param name="statusDescription">response status description</param>
        /// <param name="headers">response headers</param>
        public RestResponse(HttpStatusCode status, string statusDescription, HttpResponseHeaders headers)
            : base(status, statusDescription, headers)
        {
        }

        /// <summary>
        /// Gets response content in form of <see cref="JObject"/>.
        /// </summary>
        public JObject AsJObject
        {
            get
            {
                try
                {
                    return JObject.Parse(Content);
                }
                catch (JsonReaderException)
                {
                    ULog.Error("Unable to parse response as JSON. Content:" + Environment.NewLine + Content);
                    throw;
                }
            }
        }
    }
}
