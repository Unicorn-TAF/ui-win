using System.Linq;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    /// <summary>
    /// Matcher to check if REST service JSON response has any child matching specified JSONPath.
    /// </summary>
    public class HasTokenMatchingJsonPathMatcher : TypeSafeMatcher<RestResponse>
    {
        private readonly string _jsonPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasTokenMatchingJsonPathMatcher"/> class with JSONPAth.
        /// </summary>
        /// <param name="jsonPath">JSONPath to search for tokens</param>
        public HasTokenMatchingJsonPathMatcher(string jsonPath)
        {
            _jsonPath = jsonPath;
        }

        /// <summary>
        /// Gets verification description.
        /// </summary>
        public override string CheckDescription => $"has token matching JSONPath '{_jsonPath}'";

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

            var hasMatch = actual.AsJObject.SelectTokens(_jsonPath).Any();

            DescribeMismatch(actual.Content);
            return hasMatch;
        }
    }
}
