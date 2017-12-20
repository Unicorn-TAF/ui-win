using System.Text;

namespace Unicorn.Core.Testing.Assertions.Matchers
{
    public abstract class Matcher
    {
        private bool nullCheckable = true;
        private bool reverse = false;
        private StringBuilder matcherOutput;

        protected Matcher()
        {
            this.matcherOutput = new StringBuilder();
        }

        public abstract string CheckDescription { get; }

        public StringBuilder MatcherOutput => this.matcherOutput;

        protected bool NullCheckable
        {
            get
            {
                return this.nullCheckable;
            }

            set
            {
                this.nullCheckable = value;
            }
        }

        protected bool Reverse
        {
            get
            {
                return this.reverse;
            }

            set
            {
                this.reverse = value;
            }
        }

        public void DescribeTo()
        {
            this.matcherOutput.Append(this.CheckDescription);
        }

        public virtual void DescribeMismatch(object obj)
        {
            this.matcherOutput.Append("was ").Append(obj);
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
                DescribeMismatch("null");
                return false;
            }

            return true;
        }
    }
}
