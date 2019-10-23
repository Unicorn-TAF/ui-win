using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Matcher to check if collection has at least one element matches specified matcher. 
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class AnyMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly TypeSafeMatcher<T> matcher;

        public AnyMatcher(TypeSafeMatcher<T> matcher)
        {
            this.matcher = matcher;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription => $"Any of elements {this.matcher.CheckDescription}";

        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            if (!actual.Any(a => this.matcher.Matches(a)))
            {
                DescribeMismatch("not having such element");
                return false;
            }

            return true;
        }
    }
}
