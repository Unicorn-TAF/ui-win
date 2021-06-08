using System.Net;
using Unicorn.Backend.Services;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    /// <summary>
    /// Matcher to check if REST service response content contains substring.
    /// </summary>
    public class HasContentContainsMatcher : TypeSafeMatcher<ServiceResponse>
    {
        private readonly string _expectedSubstring;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasContentContainsMatcher"/> class with status code.
        /// </summary>
        /// <param name="expectedSubstring">substring to search for in response content</param>
        public HasContentContainsMatcher(string expectedSubstring)
        {
            _expectedSubstring = expectedSubstring;
        }

        /// <summary>
        /// Gets verification description.
        /// </summary>
        public override string CheckDescription => $"content contains substring '{_expectedSubstring}'";

        /// <summary>
        /// Checks if target <see cref="ServiceResponse"/> matches condition corresponding to specific matcher implementations.
        /// </summary>
        /// <param name="actual">service response under assertion</param>
        /// <returns>true - if object matches specific condition; otherwise - false</returns>
        public override bool Matches(ServiceResponse actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var containsSubstring = actual.Content.Contains(_expectedSubstring);

            DescribeMismatch(actual.Content.ToString());
            return containsSubstring;
        }
    }
}
