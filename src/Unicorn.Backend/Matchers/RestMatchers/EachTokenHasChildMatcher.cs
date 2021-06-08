using System.Linq;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    /// <summary>
    /// Matcher to check if REST service JSON response has tokens matching specified JSONPath 
    /// and having specified child tokens.
    /// </summary>
    public class EachTokenHasChildMatcher : TypeSafeMatcher<RestResponse>
    {
        private readonly string _tokensJsonPath;
        private readonly string _expectedChild;

        /// <summary>
        /// Initializes a new instance of the <see cref="EachTokenHasChildMatcher"/> class with JSONPAth and child token name.
        /// </summary>
        /// <param name="tokensJsonPath">JSONPath to search for parent tokens</param>
        /// <param name="expectedChild">expected child token name</param>
        public EachTokenHasChildMatcher(string tokensJsonPath, string expectedChild)
        {
            _tokensJsonPath = tokensJsonPath;
            _expectedChild = expectedChild;
        }

        /// <summary>
        /// Gets verification description.
        /// </summary>
        public override string CheckDescription => 
            $"each token in {_tokensJsonPath} has child '{_expectedChild}'";

        /// <summary>
        /// Checks if target <see cref="RestResponse"/> matches condition corresponding to specific matcher implementations.
        /// </summary>
        /// <param name="actual">REST response under assertion</param>
        /// <returns>true - if object matches specific condition; otherwise - false</returns>
        public override bool Matches(RestResponse actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var hasChild = actual.AsJObject.SelectTokens(_tokensJsonPath)
                .All(t => t.SelectTokens($"$.{_expectedChild}").Any());

            DescribeMismatch(actual.Content);
            return hasChild;
        }
    }
}
