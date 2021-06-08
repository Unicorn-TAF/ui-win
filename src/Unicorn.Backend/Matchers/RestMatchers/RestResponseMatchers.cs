using System.Net;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    /// <summary>
    /// Entry point for matchers for REST service response.
    /// </summary>
    public class RestResponseMatchers
    {
        /// <summary>
        /// Matcher to check if REST service response has specified status code.
        /// </summary>
        /// <param name="statusCode">expected status code</param>
        /// <returns><see cref="HasStatusCodeMatcher"/> instance</returns>
        public HasStatusCodeMatcher HasStatusCode(HttpStatusCode statusCode) =>
            new HasStatusCodeMatcher(statusCode);

        /// <summary>
        /// Matcher to check if REST service response content contains substring.
        /// </summary>
        /// <param name="expectedSubstring">substring to search for in response content</param>
        /// <returns><see cref="HasContentContainsMatcher"/> instance</returns>
        public HasContentContainsMatcher ContentContains(string expectedSubstring) =>
            new HasContentContainsMatcher(expectedSubstring);

        /// <summary>
        /// Matcher to check if REST service JSON response has any child matching specified JSONPath.
        /// </summary>
        /// <param name="jsonPath">JSONPath to search for tokens</param>
        /// <returns><see cref="HasTokenMatchingJsonPathMatcher"/> instance</returns>
        public HasTokenMatchingJsonPathMatcher HasTokenMatchingJsonPath(string jsonPath) =>
            new HasTokenMatchingJsonPathMatcher(jsonPath);

        /// <summary>
        /// Matcher to check if REST service JSON response has any child matching specified 
        /// JSONPath and having specified value.
        /// </summary>
        /// <param name="jsonPath">JSONPath to search for tokens</param>
        /// <param name="tokenValue">expected token value</param>
        /// <returns><see cref="HasTokenWithValueMatcher{T}"/> instance</returns>
        public HasTokenWithValueMatcher<T> HasTokenWithValue<T>(string jsonPath, T tokenValue) =>
            new HasTokenWithValueMatcher<T>(jsonPath, tokenValue);

        /// <summary>
        /// Matcher to check if REST service JSON response has expected number of tokens 
        /// matching specified JSONPath.
        /// </summary>
        /// <param name="jsonPath">JSONPath to search for tokens</param>
        /// <param name="expectedCount">expected tokens count</param>
        /// <returns><see cref="HasTokensCountMatcher"/> instance</returns>
        public HasTokensCountMatcher HasTokensCount(string jsonPath, int expectedCount) =>
            new HasTokensCountMatcher(jsonPath, expectedCount);

        /// <summary>
        /// Matcher to check if REST service JSON response has tokens matching specified JSONPath 
        /// and having specified child tokens.
        /// </summary>
        /// <param name="tokensJsonPath">JSONPath to search for parent tokens</param>
        /// <param name="expectedChild">expected child token name</param>
        /// <returns><see cref="EachTokenHasChildMatcher"/> instance</returns>
        public EachTokenHasChildMatcher EachTokenHasChild(string tokensJsonPath, string expectedChild) =>
            new EachTokenHasChildMatcher(tokensJsonPath, expectedChild);
    }
}
