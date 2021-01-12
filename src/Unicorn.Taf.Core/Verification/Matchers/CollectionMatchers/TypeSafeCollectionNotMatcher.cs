using System.Collections.Generic;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;

namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    /// <summary>
    /// Matcher to negotiate action of specified collection matcher.
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class TypeSafeCollectionNotMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly TypeSafeCollectionMatcher<T> _matcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSafeCollectionNotMatcher{T}"/> class for specified matcher.
        /// </summary>
        /// <param name="matcher">instance of collection matcher with specified check</param>
        public TypeSafeCollectionNotMatcher(TypeSafeCollectionMatcher<T> matcher)
        {
            matcher.Reverse = true;
            _matcher = matcher;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription => $"Not {_matcher.CheckDescription}";

        /// <summary>
        /// Negates main matcher check.
        /// </summary>
        /// <param name="actual">object under check</param>
        /// <returns>true - if main matching was failed; otherwise - false</returns>
        public override bool Matches(IEnumerable<T> actual)
        {
            if (_matcher.Matches(actual))
            {
                Output.Append(_matcher.Output);
                return false;
            }

            return true;
        }
    }
}
