using System.Text;

namespace Unicorn.Core.Testing.Assertions
{
    public abstract class Matcher
    {
        public StringBuilder MatcherOutput;

        protected bool nullCheckable = true;
        protected bool partOfNotMatcher = false;

        protected Matcher()
        {
            this.MatcherOutput = new StringBuilder();
        }

        public abstract string CheckDescription { get; }

        public void DescribeTo()
        {
            this.MatcherOutput.Append(this.CheckDescription);
        }

        public virtual void DescribeMismatch(object obj)
        {
            this.MatcherOutput.Append("was ").Append(obj);
        }

        public abstract bool Matches(object obj);

        public override string ToString()
        {
            return this.CheckDescription;
        }

        protected bool IsNotNull(object obj)
        {
            if (this.nullCheckable && obj == null)
            {
                this.MatcherOutput.Append("was null");
                return false;
            }

            return true;
        }
    }
}
