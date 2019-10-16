using System.Collections.Generic;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;

namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    public class TypeSafeCollectionNotMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly TypeSafeCollectionMatcher<T> matcher;

        public TypeSafeCollectionNotMatcher(TypeSafeCollectionMatcher<T> matcher)
        {
            matcher.Reverse = true;
            this.matcher = matcher;
        }

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
