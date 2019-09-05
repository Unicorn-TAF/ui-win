using System.Linq;
using Unicorn.Backend.Services.Rest;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    public class HasTokensCountMatcher : TypeSafeMatcher<RestResponse>
    {
        private readonly string jsonPath;
        private readonly int expectedCount;

        public HasTokensCountMatcher(string jsonPath, int expectedCount)
        {
            this.jsonPath = jsonPath;
            this.expectedCount = expectedCount;
        }

        public override string CheckDescription => 
            $"has {this.expectedCount} tokens with JSONPath '{this.jsonPath}'";

        public override bool Matches(RestResponse actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var hasCount = actual.AsJObject
                .SelectTokens(this.jsonPath).Count().Equals(this.expectedCount);

            DescribeMismatch(actual.Body);
            return hasCount;
        }
    }
}
