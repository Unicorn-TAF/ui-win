namespace Unicorn.Core.Testing.Assertions.Matchers
{
    public class StringContainsMatcher : TypeSafeMatcher<string>
    {
        private string objectToCompare;

        public StringContainsMatcher(string objectToCompare)
        {
            this.objectToCompare = objectToCompare;
        }

        public override string CheckDescription => "Contains " + this.objectToCompare;

        protected override bool Assertion(object obj)
        {
            string objString = (string)obj;

            bool contains = objString.Contains(this.objectToCompare);
            if (!contains)
            {
                this.DescribeMismatch(objString);
            }

            return contains;
        }
    }

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
