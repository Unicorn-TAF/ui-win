namespace Unicorn.Taf.Core.Verification.Matchers
{
    public abstract class TypeUnsafeMatcher : AbstractMatcher
    {
        protected TypeUnsafeMatcher() : base()
        {
        }

        public abstract bool Matches(object actual);
    }
}
