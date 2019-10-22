using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    public class IsSequenceEqualToCollectionMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly IEnumerable<T> expected;

        public IsSequenceEqualToCollectionMatcher(IEnumerable<T> expected)
        {
            this.expected = expected;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription
        {
            get
            {
                string itemsList = string.Join(", ", this.expected);

                if (itemsList.Length > 200)
                {
                    itemsList = itemsList.Substring(0, 200) + " etc . . .";
                }

                return "Is sequence equal to collection: [" + itemsList + "]";
            }
        }

        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return this.Reverse;
            }

            DescribeMismatch(string.Join(", ", actual));
            return actual.SequenceEqual(expected);
        }
    }
}
