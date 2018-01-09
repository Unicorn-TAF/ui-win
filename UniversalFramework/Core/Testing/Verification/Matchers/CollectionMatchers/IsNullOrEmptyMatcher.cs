using System.Collections;

namespace Unicorn.Core.Testing.Verification.Matchers.CollectionMatchers
{
    public class IsNullOrEmptyMatcher : Matcher
    {
        public IsNullOrEmptyMatcher()
        {
        }

        public override string CheckDescription => "Is empty";

        public override bool Matches(object collectionObj)
        {
            bool result = false;
            string mismatch = string.Empty;
            ICollection collection = (ICollection)collectionObj;

            result = collection == null || collection.Count == 0;

            if (collection == null)
            {
                mismatch = "was null";
            }
            else
            {
                mismatch = $"had length = {collection.Count}";
            }

            if (!result)
            {
                this.DescribeMismatch(mismatch);
            }

            return result;
        }
    }
}
