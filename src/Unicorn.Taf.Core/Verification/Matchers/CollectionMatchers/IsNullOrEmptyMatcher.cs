using System.Collections;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Matcher to check if collection is null or empty. 
    /// </summary>
    public class IsNullOrEmptyMatcher : TypeUnsafeMatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsNullOrEmptyMatcher"/> class.
        /// </summary>
        public IsNullOrEmptyMatcher()
        {
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription => "Is empty";

        /// <summary>
        /// Checks if collection is null or empty.
        /// </summary>
        /// <param name="actual">objects collection under check</param>
        /// <returns>true - if collection is null or empty; otherwise - false</returns>
        public override bool Matches(object actual)
        {
            ICollection collection = (ICollection)actual;

            if (collection == null)
            {
                DescribeMismatch("null");
                return true;
            }

            this.DescribeMismatch($"of length = {collection.Count}");
            return collection.Count == 0;
        }
    }
}
