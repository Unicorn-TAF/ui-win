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
        private readonly TypeSafeMatcher<T> _matcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyMatcher{T}"/> class with specified main matcher instance.
        /// </summary>
        /// <param name="matcher">instance of collection matcher with specified check</param>
        public AnyMatcher(TypeSafeMatcher<T> matcher)
        {
            _matcher = matcher;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription => $"Any of elements {_matcher.CheckDescription}";

        /// <summary>
        /// Checks if any collection item satisfies main matcher check.
        /// </summary>
        /// <param name="actual">objects collection under check</param>
        /// <returns>true - if any collection item satisfies main matcher check; otherwise - false</returns>
        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            if (!actual.Any(a => _matcher.Matches(a)))
            {
                DescribeMismatch("not having such element");
                return false;
            }

            return true;
        }
    }
}
