using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    public class AnyMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly TypeSafeMatcher<T> matcher;

        public AnyMatcher(TypeSafeMatcher<T> matcher)
        {
            this.matcher = matcher;
        }

        public override string CheckDescription => $"Any of elements {this.matcher.CheckDescription}";

        public override bool Matches(IEnumerable<T> actual)
        {
            if (!actual.Any(a => this.matcher.Matches(a)))
            {
                DescribeMismatch("not having such element");
                return false;
            }

            return true;
        }
    }
}
