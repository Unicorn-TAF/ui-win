using System.Text;

namespace Unicorn.Core.Testing.Assertions.Matchers
{
    public abstract class Matcher
    {
        protected bool nullCheckable = true;
        protected bool partOfNotMatcher = false;
        protected StringBuilder matcherOutput;

        protected Matcher()
        {
            this.matcherOutput = new StringBuilder();
        }

        public StringBuilder MatcherOutput => this.matcherOutput;

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
