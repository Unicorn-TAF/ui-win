using System.Text;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    public abstract class TypeSafeMatcher<T>
    {
        protected TypeSafeMatcher()
        {
            this.MatcherOutput = new StringBuilder();
        }

        public abstract string CheckDescription { get; }

        public StringBuilder MatcherOutput { get; protected set; }

        protected bool Reverse { get; set; } = false;

        public void DescribeTo()
        {
            this.MatcherOutput.Append(this.CheckDescription);
        }

        public virtual void DescribeMismatch(string mismatch)
        {
            this.MatcherOutput.Append("was ").Append(mismatch);
        }

        public abstract bool Matches(T actual);

        public override string ToString()
        {
            return this.CheckDescription;
        }
    }
}
