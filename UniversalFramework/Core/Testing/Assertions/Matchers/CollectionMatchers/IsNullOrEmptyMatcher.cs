using System.Collections;

namespace Unicorn.Core.Testing.Assertions.Matchers.CollectionMatchers
{
    public class IsNullOrEmptyMatcher : Matcher
    {
        private string mismatch = string.Empty;

        public IsNullOrEmptyMatcher()
        {
        }

        public override string CheckDescription => "Is empty";

        public override bool Matches(object collectionObj)
        {
            bool result = false;
            ICollection collection = (ICollection)collectionObj;

            if (collection == null)
            {
                result = true;
            }
            else if (collection.Count == 0)
            {
                result = true;
            }

            if (collection == null)
            {
                this.mismatch = "was null";
            }
            else
            {
                this.mismatch = $"had length = {collection.Count}";
            }

            if (!result)
            {
                this.DescribeMismatch(collectionObj);
            }

            return result;
        }

        public override void DescribeMismatch(object collection)
        {
            this.MatcherOutput.Append(this.mismatch);
        }
    }
}
