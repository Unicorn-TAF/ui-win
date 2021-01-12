namespace Unicorn.Taf.Core.Verification.Matchers.MiscMatchers
{
    /// <summary>
    /// Matcher to check if integer is even. 
    /// </summary>
    public class IsEvenMatcher : TypeSafeMatcher<int>
    {
        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => "Is even number";

        /// <summary>
        /// Checks if target number is even.
        /// </summary>
        /// <param name="actual">object under assertion</param>
        /// <returns>true - if number is even; otherwise - false</returns>
        public override bool Matches(int actual)
        {
            DescribeMismatch(actual.ToString());
            return actual % 2 == 0;
        }
    }
}
