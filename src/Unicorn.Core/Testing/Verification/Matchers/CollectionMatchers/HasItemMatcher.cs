using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Core.Testing.Verification.Matchers.CollectionMatchers
{
    public class HasItemMatcher : Matcher
    {
        private readonly object expectedObject;

        public HasItemMatcher(object expectedObject)
        {
            this.expectedObject = expectedObject;
        }

        public override string CheckDescription => $"Collection has item {this.expectedObject}";

        public override bool Matches(object actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            if (((IEnumerable<object>)actual).Contains(this.expectedObject))
            {
                return true;
            }
            else
            {
                DescribeMismatch(this.Reverse ? "was contains the value" : "was not contain the value");
                return false;
            }
        }
    }
}
