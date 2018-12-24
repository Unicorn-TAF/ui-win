using System.Net;
using Unicorn.Backend.Matchers.RestMatchers;

namespace Unicorn.Backend.Matchers
{
    public static class Response
    {
        public static HasStatusCodeMatcher HasStatusCode(HttpStatusCode statusCode) =>
            new HasStatusCodeMatcher(statusCode);

        public static HasTokenMatchingJsonPathMatcher HasTokenMatchingJsonPath(string jsonPath) =>
            new HasTokenMatchingJsonPathMatcher(jsonPath);

        public static HasTokenWithValueMatcher<T> HasTokenWithValue<T>(string jsonPath, T tokenValue) =>
            new HasTokenWithValueMatcher<T>(jsonPath, tokenValue);

        public static HasTokensCountMatcher HasTokensCount(string jsonPath, int expectedCount) =>
            new HasTokensCountMatcher(jsonPath, expectedCount);

        public static EachTokenHasChildMatcher EachTokenHasChild(string tokensJsonPath, string expectedChild) =>
            new EachTokenHasChildMatcher(tokensJsonPath, expectedChild);
    }
}
