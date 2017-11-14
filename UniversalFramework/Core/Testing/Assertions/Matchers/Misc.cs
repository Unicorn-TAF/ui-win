
namespace Unicorn.Core.Testing.Assertions.Matchers
{
    public class StringContainsMatcher : TypeSafeMatcher<string>
    {
        private string ObjectToCompare;


        public StringContainsMatcher(string objectToCompare)
        {
            ObjectToCompare = objectToCompare;
        }

        public override string CheckDescription
        {
            get
            {
                return "Contains " + ObjectToCompare;
            }
        }


        protected override bool Assertion(object _object)
        {
            string _objString = (string)_object;

            bool contains = _objString.Contains(ObjectToCompare);
            if (!contains)
                DescribeMismatch(_objString);

            return contains;
        }
    }


    public class IsEvenMatcher : TypeSafeMatcher<int>
    {
        public override string CheckDescription
        {
            get
            {
                return "An Even number";
            }
        }


        protected override bool Assertion(object number)
        {
            bool isEven = (int)number % 2 == 0;
            if (!isEven)
                DescribeMismatch(number);

            return isEven;
        }

        public override void DescribeMismatch(object number)
        {
            base.DescribeMismatch(number);
        }
    }

}
