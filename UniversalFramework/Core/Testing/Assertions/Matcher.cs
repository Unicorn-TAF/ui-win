using System.Text;

namespace Unicorn.Core.Testing.Assertions
{
    public abstract class Matcher
    {
        public StringBuilder MatcherOutput;

        protected bool NullCheckable = true;
        protected bool PartOfNotMatcher = false;

        public abstract string CheckDescription { get; }


        protected Matcher()
        {
            MatcherOutput = new StringBuilder();
        }



        public void DescribeTo()
        {
            MatcherOutput.Append(CheckDescription);
        }


        public virtual void DescribeMismatch(object _object)
        {
            MatcherOutput.Append("was ").Append(_object);
        }


        public abstract bool Matches(object _object);


        protected bool IsNotNull(object _object)
        {
            if (NullCheckable && _object == null)
            {
                MatcherOutput.Append("was null");
                return false;
            }
            return true;
        }


        public override string ToString()
        {
            return CheckDescription;
        }
    }
}
