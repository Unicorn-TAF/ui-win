using System;
using System.Net;
using System.Net.Http;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    /// <summary>
    /// Matcher to check if REST service has specified endpoint.
    /// </summary>
    public class HasEndpointMatcher : TypeSafeMatcher<RestClient>
    {
        private readonly string _endpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasEndpointMatcher"/> class with endpoint relative url.
        /// </summary>
        /// <param name="endpoint"></param>
        public HasEndpointMatcher(string endpoint)
        {
            _endpoint = endpoint;
        }

        /// <summary>
        /// Gets verification description.
        /// </summary>
        public override string CheckDescription => $"has '{_endpoint}' endpoint";

        /// <summary>
        /// Checks if target <see cref="RestResponse"/> matches condition corresponding to specific matcher implementations.
        /// </summary>
        /// <param name="actual">REST response under assertion</param>
        /// <returns>true - if object matches specific condition; otherwise - false</returns>
        public override bool Matches(RestClient actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            if (actual.BaseUri == null)
            {
                DescribeMismatch("missing base URI");
                return Reverse;
            }

            try
            {
                var response = actual.SendRequest(new HttpMethod("HEAD"), _endpoint);
                DescribeMismatch(response.StatusDescription);

                return response.Status == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                DescribeMismatch(ex.Message);
                //Any exception will returns false.
                return false;
            }
        }
    }
}
