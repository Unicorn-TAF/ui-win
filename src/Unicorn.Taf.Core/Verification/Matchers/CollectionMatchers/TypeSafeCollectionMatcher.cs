using System.Collections.Generic;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Base matcher for type specific collection matchers realizations.
    /// </summary>
    /// <typeparam name="T">type of object under assertion</typeparam>
    public abstract class TypeSafeCollectionMatcher<T> : AbstractMatcher
    {
        protected TypeSafeCollectionMatcher() : base()
        {
        }

        public abstract bool Matches(IEnumerable<T> actual);

        protected string DescribeCollection(IEnumerable<T> collection)
        {
            string itemsList = string.Join(", ", collection);

            if (itemsList.Length > 200)
            {
                itemsList = itemsList.Substring(0, 200) + " etc . . .";
            }

            return itemsList;
        }
    }
}
