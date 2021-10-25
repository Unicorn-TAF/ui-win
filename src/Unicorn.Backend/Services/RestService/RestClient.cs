using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Backend.Services.RestService
{
    /// <summary>
    /// Describes base rest service client containing basic actions.
    /// </summary>
    public class RestClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class with service base url.<br/>
        /// <see cref="SecurityProtocolType.Tls12"/> protocol is used by default.
        /// </summary>
        /// <param name="baseUri">service base url</param>
        public RestClient(string baseUri) : this(new Uri(baseUri), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class with service base url.<br/>
        /// <see cref="SecurityProtocolType.Tls12"/> protocol is used by default.
        /// </summary>
        /// <param name="baseUri">service base url</param>
        public RestClient(Uri baseUri) : this(baseUri, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class with service base url 
        /// based on existing session.<br/>
        /// <see cref="SecurityProtocolType.Tls12"/> protocol is used by default.
        /// </summary>
        /// <param name="baseUri">service base uri</param>
        /// <param name="session">existing service session</param>
        public RestClient(Uri baseUri, IServiceSession session)
        {
            BaseUri = baseUri;
            Session = session;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class.
        /// </summary>
        public RestClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Gets or sets service session.
        /// </summary>
        public IServiceSession Session { get; set; }

        /// <summary>
        /// Gets or sets service base url.
        /// </summary>
        public Uri BaseUri { get; set; }

        /// <summary>
        /// Gets content type for service calls.
        /// </summary>
        protected virtual string ContentType { get; } = "application/json";

        /// <summary>
        /// Gets encoding for service calls.
        /// </summary>
        protected virtual Encoding Encoding { get; } = Encoding.UTF8;

        /// <summary>
        /// Sends specified <see cref="HttpRequestMessage"/> request.
        /// </summary>
        /// <param name="request">request to send</param>
        /// <returns>service response</returns>
        public virtual RestResponse SendRequest(HttpRequestMessage request)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Sending {request.Method} request to {request.RequestUri}.");

            using (var handler = new HttpClientHandler())
            {
                handler.AllowAutoRedirect = true;

                // By default HttpClient uses CookieContainer.
                // If we want to set Cookie via Headers we need to disable cookies.
                if (request.Headers.Contains("Cookie"))
                {
                    handler.UseCookies = false;
                }

                using (var client = new HttpClient(handler))
                {
                    var timer = Stopwatch.StartNew();

                    var response = client.SendAsync(request).Result;
                    var responseContent = response.Content.ReadAsStringAsync().Result;

                    var elapsed = timer.Elapsed;

                    var restResponse = new RestResponse(response.StatusCode, response.ReasonPhrase, response.Headers)
                    {
                        Content = responseContent,
                        ExecutionTime = elapsed,
                    };

                    Logger.Instance.Log(LogLevel.Debug, $"Getting {restResponse.Status} response.");

                    if (Logger.Level.Equals(LogLevel.Trace) && restResponse.Content != null)
                    {
                        Logger.Instance.Log(LogLevel.Trace, "Response body: " + restResponse.Content);
                    }

                    request.Dispose();

                    return restResponse;
                }
            }
        }

        /// <summary>
        /// Sends service request type to endpoint with content.
        /// </summary>
        /// <param name="method">Http method</param>
        /// <param name="endpoint">service endpoint relative url</param>
        /// <param name="content">request content</param>
        /// <returns>service response</returns>
        public virtual RestResponse SendRequest(HttpMethod method, string endpoint, string content)
        {
            var request = CreateRequest(method, endpoint, content);

            return SendRequest(request);
        }

        /// <summary>
        /// Sends service request type to endpoint without content.
        /// </summary>
        /// <param name="method">Http method</param>
        /// <param name="endpoint">service endpoint relative url</param>
        /// <returns></returns>
        public virtual RestResponse SendRequest(HttpMethod method, string endpoint) =>
            SendRequest(method, endpoint, string.Empty);

        /// <summary>
        /// Returns <seealso cref="HttpResponseMessage"/> witn file<para/>
        /// Responce should be disposed later
        /// </summary>
        /// <param name="endpoint">Service endpoint relative url</param>
        /// <returns><seealso cref="HttpResponseMessage"/> that contains file</returns>
        public HttpResponseMessage DownloadFile(string endpoint)
        {
            return GenerateDownloadFileRequest(endpoint);
        }

        /// <summary>
        /// Downloads file to specific location in <paramref name="path"/>
        /// </summary>
        /// <param name="path">Where to save</param>
        /// <param name="endpoint">Endpoint</param>
        public void DownloadFile(string path, string endpoint)
        {
            var request = GenerateDownloadFileRequest(endpoint);
            var streamToReadFrom = request.Content.ReadAsStreamAsync().Result;

            using (var fileStream = File.Create(path))
            {
                streamToReadFrom.Seek(0, SeekOrigin.Begin);
                streamToReadFrom.CopyTo(fileStream);
            }
        }

        private HttpResponseMessage GenerateDownloadFileRequest(string endpoint)
        {
            // Getting request just to get cookies for existing sesion if any
            var request = CreateRequest(HttpMethod.Get, endpoint, string.Empty);

            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true
            };
            handler.CookieContainer.SetCookies(this.BaseUri, request.Headers.ToString());
            HttpClient client = new HttpClient(handler);
            return client.GetAsync(this.BaseUri + endpoint).Result;
        }

        /// <summary>
        /// Creates <see cref="HttpRequestMessage"/> and fills it's headers from session.
        /// </summary>
        /// <param name="method">Http method</param>
        /// <param name="endpoint">service endpoint relative url</param>
        /// <param name="content">request content</param>
        /// <returns><see cref="HttpRequestMessage"/> instance</returns>
        protected virtual HttpRequestMessage CreateRequest(HttpMethod method, string endpoint, string content)
        {
            var uri = new Uri(BaseUri, endpoint);
            var request = new HttpRequestMessage(method, uri);

            if (!method.Equals(HttpMethod.Get))
            {
                request.Content = new StringContent(content, Encoding, ContentType);
            }

            Session?.UpdateRequestWithSessionData(request);

            return request;
        }
    }
}
