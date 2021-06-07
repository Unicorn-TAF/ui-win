namespace Unicorn.Taf.Core.Verification.Matchers.MiscMatchers
{
    /// <summary>
    /// Matcher to check if integer is positive. 
    /// </summary>
    public class IsPositiveMatcher : TypeSafeMatcher<int>
    {
        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => "Is positive number";

        /// <summary>
        /// Checks if target number is positive.
        /// </summary>
        /// <param name="actual">object under assertion</param>
        /// <returns>true - if number is positive; otherwise - false</returns>
        public override bool Matches(int actual)
        {
            DescribeMismatch(actual.ToString());
            return actual > 0;
        }
    }
}
