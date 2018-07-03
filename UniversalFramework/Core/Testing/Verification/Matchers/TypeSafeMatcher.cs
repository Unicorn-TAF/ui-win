using System.Text;

namespace Unicorn.Core.Testing.Verification.Matchers
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

        public abstract bool Matches(T obj);

        public override string ToString()
        {
            return this.CheckDescription;
        }
    }
}
