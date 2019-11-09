namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    /// <summary>
    /// Matcher to negotiate action of another matcher.
    /// </summary>
    public class NotMatcher : TypeUnsafeMatcher
    {
        private readonly TypeUnsafeMatcher _matcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotMatcher"/> class for specified matcher.
        /// </summary>
        /// <param name="matcher">instance of matcher with specified check</param>
        public NotMatcher(TypeUnsafeMatcher matcher)
        {
            matcher.Reverse = true;
            _matcher = matcher;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"Not {_matcher.CheckDescription}";

        /// <summary>
        /// Negates main matcher check.
        /// </summary>
        /// <param name="actual">object under check</param>
        /// <returns>true - if main matching was failed; otherwise - false</returns>
        public override bool Matches(object actual)
        {
            if (_matcher.Matches(actual))
            {
                Output.Append(_matcher.Output);
                return false;
            }
            
            return true;
        }
    }
}
