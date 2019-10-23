namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    /// <summary>
    /// Matcher to check if object is null. 
    /// </summary>
    public class NullMatcher : TypeUnsafeMatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullMatcher"/> class.
        /// </summary>
        public NullMatcher() : base()
        {
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => "Is null";

        public override bool Matches(object actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return true;
            }
            else
            {
                DescribeMismatch(actual.ToString());
                return false;
            }
        }
    }
}
