using System.Text;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    public abstract class AbstractMatcher
    {
        protected AbstractMatcher()
        {
            this.Output = new StringBuilder();
        }

        public StringBuilder Output { get; protected set; }

        public abstract string CheckDescription { get; }

        public bool Reverse { get; set; } = false;

        public virtual void DescribeMismatch(string mismatch) =>
            this.Output.Append("was ").Append(mismatch);

        public override string ToString() =>
            this.CheckDescription;
    }
}
