namespace Unicorn.Taf.Core.Verification.Matchers
{
    public abstract class TypeSafeMatcher<T> : AbstractMatcher
    {
        protected TypeSafeMatcher() : base()
        {
        }

        public abstract bool Matches(T actual);
    }
}
