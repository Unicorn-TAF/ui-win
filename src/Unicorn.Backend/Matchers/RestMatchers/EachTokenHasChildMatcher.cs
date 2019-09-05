using System.Linq;
using Unicorn.Backend.Services.Rest;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    public class EachTokenHasChildMatcher : TypeSafeMatcher<RestResponse>
    {
        private readonly string tokensJsonPath;
        private readonly string expectedChild;

        public EachTokenHasChildMatcher(string tokensJsonPath, string expectedChild)
        {
            this.tokensJsonPath = tokensJsonPath;
            this.expectedChild = expectedChild;
        }

        public override string CheckDescription => 
            $"each token in {this.tokensJsonPath} has child '{this.expectedChild}'";

        public override bool Matches(RestResponse actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var hasChild = actual.AsJObject.SelectTokens(this.tokensJsonPath)
                .All(t => t.SelectTokens($"$.{this.expectedChild}").Any());

            DescribeMismatch(actual.Body);
            return hasChild;
        }
    }
}
