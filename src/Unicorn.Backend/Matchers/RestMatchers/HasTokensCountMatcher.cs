using System.Linq;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    /// <summary>
    /// Matcher to check if REST service JSON response has expected number of tokens 
    /// matching specified JSONPath.
    /// </summary>
    public class HasTokensCountMatcher : TypeSafeMatcher<RestResponse>
    {
        private readonly string _jsonPath;
        private readonly int _expectedCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasTokensCountMatcher"/> class with JSONPAth and tokens count.
        /// </summary>
        /// <param name="jsonPath">JSONPath to search for tokens</param>
        /// <param name="expectedCount">expected tokens count</param>
        public HasTokensCountMatcher(string jsonPath, int expectedCount)
        {
            _jsonPath = jsonPath;
            _expectedCount = expectedCount;
        }

        /// <summary>
        /// Gets verification description.
        /// </summary>
        public override string CheckDescription => 
            $"has {_expectedCount} tokens with JSONPath '{_jsonPath}'";

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

            var hasCount = actual.AsJObject
                .SelectTokens(_jsonPath).Count().Equals(_expectedCount);

            DescribeMismatch(actual.Content);
            return hasCount;
        }
    }
}
