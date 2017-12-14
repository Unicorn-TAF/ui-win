namespace Unicorn.Core.Testing.Assertions.Matchers.MiscMatchers
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
}
