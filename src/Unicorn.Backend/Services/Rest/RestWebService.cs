using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Backend.Services.Rest
{
    public enum RestAction
    {
        Post,
        Get,
        Put,
        Patch,
        Delete
    }

    public class RestWebService
    {
        public RestWebService(string baseUrl) : 
            this(baseUrl, null)
        {
        }

        public RestWebService(string baseUrl, ISession sessionInfo)
        {
            this.BaseUrl = new Uri(baseUrl);
            this.Session = sessionInfo;
        }

        public RestWebService()
        {
        }

        public ISession Session { get; set; }

        public Uri BaseUrl { get; set; }

        protected virtual string ContentType { get; } = "application/json";

        public virtual RestResponse Send(RestAction action, string endpoint, string requestBody)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var request = CreateRequestWithHeaders(Session, endpoint, action);

            var responseText = new StringBuilder();

            if (!action.Equals(RestAction.Get))
            {
                var bodyData = Encoding.UTF8.GetBytes(requestBody);
                request.ContentLength = bodyData.Length;

                var requestStream = request.GetRequestStream();
                requestStream.Write(bodyData, 0, bodyData.Length);
                requestStream.Close();
            }

            Logger.Instance.Log(LogLevel.Trace, $"Sending {action} request to {request.Address}\n\tHeaders: {request.Headers}");

            var timer = Stopwatch.StartNew();
            var webResponse = request.GetResponse() as HttpWebResponse;
            var responseStream = webResponse.GetResponseStream();
            var encode = Encoding.GetEncoding("utf-8");

            // Pipe the stream to a higher level stream reader with the required encoding format. 
            var readStream = new StreamReader(responseStream, encode);
            var read = new char[256];

            // Read 256 charcters at a time.    
            var count = readStream.Read(read, 0, 256);

            while (count > 0)
            {
                // Dump the 256 characters on a string and display the string onto the console.
                var str = new string(read, 0, count);
                responseText.Append(str);
                count = readStream.Read(read, 0, 256);
            }

            // Release the resources of stream object.
            readStream.Close();
            timer.Stop();

            var response = new RestResponse(webResponse.StatusCode, webResponse.Headers, responseText.ToString())
            {
                ExecutionTime = timer.Elapsed,
                StatusDescription = webResponse.StatusDescription
            };

            // Release the resources of response object.
            webResponse.Close();

            return response;
        }

        public virtual RestResponse SendAndDecompressResponse(RestAction action, string endpoint, string requestBody)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            
            var request = CreateRequestWithHeaders(Session, endpoint, action);
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");

            if (!action.Equals(RestAction.Get))
            {
                var bodyData = Encoding.UTF8.GetBytes(requestBody);
                request.ContentLength = bodyData.Length;

                var requestStream = request.GetRequestStream();
                requestStream.Write(bodyData, 0, bodyData.Length);
                requestStream.Close();
            }

            Logger.Instance.Log(LogLevel.Trace, $"Sending {action} request to {request.Address}\n\tHeaders: {request.Headers}");

            var timer = Stopwatch.StartNew();
            var webResponse = request.GetResponse() as HttpWebResponse;
            var responseStream = webResponse.GetResponseStream();

            byte[] output;
            var buffer = new byte[16 * 1024];

            using (var ms = new MemoryStream())
            {
                int read;

                while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                output = ms.ToArray();
            }

            timer.Stop();

            byte[] decompressedOutput;

            using (var compressedStream = new MemoryStream(output))
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        decompressedOutput = resultStream.ToArray();
                    }
                }
            }

            var response = new RestResponse(webResponse.StatusCode, webResponse.Headers, Encoding.UTF8.GetString(decompressedOutput))
            {
                ExecutionTime = timer.Elapsed,
                StatusDescription = webResponse.StatusDescription
            };

            // Release the resources of response object.
            webResponse.Close();

            return response;
        }

        private HttpWebRequest CreateRequestWithHeaders(ISession session, string endpoint, RestAction action)
        {
            var uri = new Uri(this.BaseUrl, endpoint);
            var request = (HttpWebRequest)WebRequest.Create(uri);

            request.Method = action.ToString().ToUpper();
            request.ContentType = ContentType;
            request.AllowAutoRedirect = false;
            session?.UpdateRequestWithSessionData(ref request);
            return request;
        }
    }
}
