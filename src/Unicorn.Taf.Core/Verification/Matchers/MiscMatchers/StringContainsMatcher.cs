namespace Unicorn.Taf.Core.Verification.Matchers.MiscMatchers
{
    /// <summary>
    /// Matcher to check if string contains specified sub-string. 
    /// </summary>
    public class StringContainsMatcher : TypeSafeMatcher<string>
    {
        private readonly string _objectToCompare;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringContainsMatcher"/> class for specified sub-string.
        /// </summary>
        /// <param name="objectToCompare">expected sub-string</param>
        public StringContainsMatcher(string objectToCompare)
        {
            _objectToCompare = objectToCompare;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"Contains substring '{_objectToCompare}'";

        /// <summary>
        /// Checks if target string contains specified sub-string.
        /// </summary>
        /// <param name="actual">object under assertion</param>
        /// <returns>true - if string contains specified sub-string; otherwise - false</returns>
        public override bool Matches(string actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            DescribeMismatch(actual);
            return actual.Contains(_objectToCompare);
        }
    }
}
