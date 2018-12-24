using System.Linq;
using Unicorn.Backend.Services.Rest;
using Unicorn.Core.Testing.Verification.Matchers;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    public class HasTokenMatchingJsonPathMatcher : TypeSafeMatcher<RestResponse>
    {
        private readonly string jsonPath;

        public HasTokenMatchingJsonPathMatcher(string jsonPath)
        {
            this.jsonPath = jsonPath;
        }

        public override string CheckDescription => $"has token matching JSONPath '{this.jsonPath}'";

        public override bool Matches(RestResponse actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var hasMatch = actual.AsJObject.SelectTokens(this.jsonPath).Any();

            DescribeMismatch(actual.Body);
            return hasMatch;
        }
    }
}
