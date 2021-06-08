using System.Net;
using Unicorn.Backend.Services;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    /// <summary>
    /// Matcher to check if REST service response has specified status code.
    /// </summary>
    public class HasStatusCodeMatcher : TypeSafeMatcher<ServiceResponse>
    {
        private readonly HttpStatusCode _statusCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasStatusCodeMatcher"/> class with status code.
        /// </summary>
        /// <param name="statusCode">expected status code</param>
        public HasStatusCodeMatcher(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }

        /// <summary>
        /// Gets verification description.
        /// </summary>
        public override string CheckDescription => $"has status code '{_statusCode}'";

        /// <summary>
        /// Checks if target <see cref="ServiceResponse"/> matches condition corresponding to specific matcher implementations.
        /// </summary>
        /// <param name="actual">service response under assertion</param>
        /// <returns>true - if object matches specific condition; otherwise - false</returns>
        public override bool Matches(ServiceResponse actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var hasStatusCode = actual.Status.Equals(_statusCode);

            DescribeMismatch(actual.Status.ToString());
            return hasStatusCode;
        }
    }
}
