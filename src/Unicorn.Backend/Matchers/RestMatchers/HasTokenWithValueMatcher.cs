using System.Linq;
using Newtonsoft.Json.Linq;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    /// <summary>
    /// Matcher to check if REST service JSON response has any child matching specified 
    /// JSONPath and having specified value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HasTokenWithValueMatcher<T> : TypeSafeMatcher<RestResponse>
    {
        private readonly string _jsonPath;
        private readonly T _tokenValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasTokenWithValueMatcher{T}"/> class with JSONPAth and token value.
        /// </summary>
        /// <param name="jsonPath">JSONPath to search for tokens</param>
        /// <param name="tokenValue">expected token value</param>
        public HasTokenWithValueMatcher(string jsonPath, T tokenValue)
        {
            _jsonPath = jsonPath;
            _tokenValue = tokenValue;
        }

        /// <summary>
        /// Gets verification description.
        /// </summary>
        public override string CheckDescription => 
            $"has token '{_jsonPath}' with value '{_tokenValue}'";

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

            var hasMatch = actual.AsJObject.SelectTokens(_jsonPath)
                .Any(t => t.Value<T>().Equals(_tokenValue));

            DescribeMismatch(actual.Content);
            return hasMatch;
        }
    }
}
