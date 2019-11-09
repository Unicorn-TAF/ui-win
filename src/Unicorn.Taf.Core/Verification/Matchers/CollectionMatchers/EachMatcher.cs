using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Matcher to check if each collection element matches specified matcher. 
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class EachMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly TypeSafeMatcher<T> _matcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="EachMatcher{T}"/> class with specified main matcher instance.
        /// </summary>
        /// <param name="matcher">instance of collection matcher with specified check</param>
        public EachMatcher(TypeSafeMatcher<T> matcher)
        {
            _matcher = matcher;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription => $"Each element {_matcher.CheckDescription}";

        /// <summary>
        /// Checks if each collection item satisfies main matcher check.
        /// </summary>
        /// <param name="actual">objects collection under check</param>
        /// <returns>true - if each collection item satisfies main matcher check; otherwise - false</returns>
        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var matches = true;

            for (var i = 0; i < actual.Count(); i++)
            {
                if (!_matcher.Matches(actual.ElementAt(i)))
                {
                    matches = false;
                    Output.AppendFormat("element at index {0}:", i).AppendLine(_matcher.Output.ToString());
                    _matcher.Output.Clear();
                }
            }

            return matches;
        }
    }
}
