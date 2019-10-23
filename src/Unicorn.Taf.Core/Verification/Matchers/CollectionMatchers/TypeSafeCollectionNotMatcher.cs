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
        private readonly TypeSafeCollectionMatcher<T> matcher;

        public TypeSafeCollectionNotMatcher(TypeSafeCollectionMatcher<T> matcher)
        {
            matcher.Reverse = true;
            this.matcher = matcher;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription => $"Not {this.matcher.CheckDescription}";

        public override bool Matches(IEnumerable<T> actual)
        {
            if (this.matcher.Matches(actual))
            {
                this.Output.Append(this.matcher.Output);
                return false;
            }

            return true;
        }
    }
}
