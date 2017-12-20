using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Core.Testing.Assertions.Matchers.CollectionMatchers
{
    public class HasItemMatcher : Matcher
    {
        private object expectedObject;

        public HasItemMatcher(object expectedObject)
        {
            this.expectedObject = expectedObject;
        }

        public override string CheckDescription => "Collection has item " + this.expectedObject;

        public override bool Matches(object collection)
        {
            if (!IsNotNull(collection))
            {
                return this.Reverse;
            }

            string mismatch = string.Empty;
            bool result = ((IEnumerable<object>)collection).Contains(this.expectedObject);

            if (this.Reverse)
            {
                mismatch = "was contains the value";
            }
            else
            {
                mismatch = "was not contain the value";
            }

            if (!result)
            {
                DescribeMismatch(mismatch);
            }

            return result;
        }
    }
}
