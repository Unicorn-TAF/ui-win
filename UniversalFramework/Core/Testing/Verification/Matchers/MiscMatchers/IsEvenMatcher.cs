namespace Unicorn.Core.Testing.Verification.Matchers.MiscMatchers
{
    public class IsEvenMatcher : TypeSafeMatcher<int>
    {
        public override string CheckDescription => "An Even number";

        public override void DescribeMismatch(object number)
        {
            base.DescribeMismatch(number);
        }

        protected override bool Assertion(object number)
        {
            bool isEven = (int)number % 2 == 0;
            if (!isEven)
            {
                DescribeMismatch(number);
            }

            return isEven;
        }
    }
}
