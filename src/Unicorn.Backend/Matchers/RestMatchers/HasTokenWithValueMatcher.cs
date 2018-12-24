using System.Linq;
using Newtonsoft.Json.Linq;
using Unicorn.Backend.Services.Rest;
using Unicorn.Core.Testing.Verification.Matchers;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    public class HasTokenWithValueMatcher<T> : TypeSafeMatcher<RestResponse>
    {
        private readonly string jsonPath;
        private readonly T tokenValue;

        public HasTokenWithValueMatcher(string jsonPath, T tokenValue)
        {
            this.jsonPath = jsonPath;
            this.tokenValue = tokenValue;
        }

        public override string CheckDescription => 
            $"has token '{this.jsonPath}' with value '{this.tokenValue}'";

        public override bool Matches(RestResponse actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var hasMatch = actual.AsJObject.SelectTokens(this.jsonPath)
                .Any(t => t.Value<T>().Equals(tokenValue));

            DescribeMismatch(actual.Body);
            return hasMatch;
        }
    }
}
