using System.Net;
using Unicorn.Backend.Services;
using Unicorn.Core.Testing.Verification.Matchers;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    public class HasStatusCodeMatcher : TypeSafeMatcher<HttpResponse>
    {
        private readonly HttpStatusCode statusCode;

        public HasStatusCodeMatcher(HttpStatusCode statusCode)
        {
            this.statusCode = statusCode;
        }

        public override string CheckDescription => $"has status code '{this.statusCode}'";

        public override bool Matches(HttpResponse actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var hasStatusCode = actual.Status.Equals(statusCode);

            DescribeMismatch(actual.Status.ToString());
            return hasStatusCode;
        }
    }
}
