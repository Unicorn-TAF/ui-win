using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Core.Testing.Assertions.Matchers.CollectionMatchers
{
    public class HasItemMatcher : Matcher
    {
        private object expectedObject;
        private string mismatch = string.Empty;

        public HasItemMatcher(object expectedObject)
        {
            this.expectedObject = expectedObject;
        }

        public override string CheckDescription => "Collection has item " + this.expectedObject;

        public override bool Matches(object collection)
        {
            if (!IsNotNull(collection))
            {
                return this.partOfNotMatcher;
            }

            bool result = ((IEnumerable<object>)collection).Contains(this.expectedObject);

            if (this.partOfNotMatcher)
            {
                this.mismatch = "Collection contains the value";
            }
            else
            {
                this.mismatch = "Collection does not contain the value";
            }

            if (!result)
            {
                DescribeMismatch(collection);
            }

            return result;
        }

        public override void DescribeMismatch(object collection)
        {
            MatcherOutput.Append(this.mismatch);
        }
    }
}
