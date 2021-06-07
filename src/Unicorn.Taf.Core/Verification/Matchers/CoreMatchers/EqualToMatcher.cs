namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    /// <summary>
    /// Matcher to check if object is equal to expected one. 
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class EqualToMatcher<T> : TypeSafeMatcher<T>
    {
        private readonly T _objectToCompare;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualToMatcher{T}"/> class for specified expected object.
        /// </summary>
        /// <param name="objectToCompare">expected object</param>
        public EqualToMatcher(T objectToCompare)
        {
            _objectToCompare = objectToCompare;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => "Is equal to " + _objectToCompare;

        /// <summary>
        /// Checks if object is equal to expected one.
        /// </summary>
        /// <param name="actual">object under check</param>
        /// <returns>true - if actual object is equal to expected one; otherwise - false</returns>
        public override bool Matches(T actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            DescribeMismatch(actual.ToString());
            return actual.Equals(_objectToCompare);
        }
    }
}
