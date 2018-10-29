using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace Unicorn.Backend.Services.Rest
{
    public enum RestRequestType
    {
        POST,
        GET,
        PUT,
        PATCH,
        DELETE
    }

    public class RestService : IWebService
    {
        public RestService(string baseUrl, string serviceRelativeUrl, RestRequestType requestType)
        {
            this.BaseUrl = baseUrl.TrimEnd('/');
            this.RelativeUrl = "/" + serviceRelativeUrl.TrimStart('/');
            this.RequestType = requestType;
        }

        public RestService()
        {
        }

        public static ISessionInfo CurrentSession { get; set; } = null;

        public string BaseUrl { get; set; }

        public string RelativeUrl { get; set; }

        public RestRequestType RequestType { get; set; }

        public virtual HttpResponse SendRequest(string requestBody)
        {
            var request = CreateRequestWithHeaders(CurrentSession);

            var responseText = new StringBuilder();

            if (!this.RequestType.Equals(RestRequestType.GET))
            {
                byte[] bodyData = Encoding.UTF8.GetBytes(requestBody);
                request.ContentLength = bodyData.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bodyData, 0, bodyData.Length);
                requestStream.Close();
            }

            var timer = Stopwatch.StartNew();

            HttpWebResponse webResponse = request.GetResponse() as HttpWebResponse;
            Stream responseStream = webResponse.GetResponseStream();
            Encoding encode = Encoding.GetEncoding("utf-8");

            // Pipe the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(responseStream, encode);
            var read = new char[256];

            // Read 256 charcters at a time.    
            int count = readStream.Read(read, 0, 256);

            while (count > 0)
            {
                // Dump the 256 characters on a string and display the string onto the console.
                responseText.Append(read);
                count = readStream.Read(read, 0, 256);
            }

            // Release the resources of stream object.
            readStream.Close();
            timer.Stop();

            var response = new HttpResponse(webResponse.StatusCode, webResponse.Headers, responseText.ToString())
            {
                ExecutionTime = timer.Elapsed,
                StatusDescription = webResponse.StatusDescription
            };

            // Release the resources of response object.
            webResponse.Close();

            return response;
        }

        public virtual HttpResponse SendRequestAndDecompress(string requestBody)
        {
            var request = CreateRequestWithHeaders(CurrentSession);
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");

            if (!this.RequestType.Equals(RestRequestType.GET))
            {
                byte[] bodyData = Encoding.UTF8.GetBytes(requestBody);
                request.ContentLength = bodyData.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bodyData, 0, bodyData.Length);
                requestStream.Close();
            }

            var timer = Stopwatch.StartNew();

            HttpWebResponse webResponse = request.GetResponse() as HttpWebResponse;
            Stream responseStream = webResponse.GetResponseStream();

            byte[] output;
            byte[] buffer = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
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
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                decompressedOutput = resultStream.ToArray();
            }

            var response = new HttpResponse(webResponse.StatusCode, webResponse.Headers, Encoding.UTF8.GetString(decompressedOutput))
            {
                ExecutionTime = timer.Elapsed,
                StatusDescription = webResponse.StatusDescription
            };

            // Release the resources of response object.
            webResponse.Close();

            return response;
        }

        private HttpWebRequest CreateRequestWithHeaders(ISessionInfo session)
        {
            var request = (HttpWebRequest)WebRequest.Create(this.BaseUrl + this.RelativeUrl);

            request.Method = this.RequestType.ToString();
            request.ContentType = "application/json";
            request.AllowAutoRedirect = false;
            session?.UpdateRequestWithSessionInfo(ref request);
            return request;
        }
    }
}
