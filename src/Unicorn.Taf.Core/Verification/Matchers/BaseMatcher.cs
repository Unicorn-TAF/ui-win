using System.Text;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    /// <summary>
    /// Provides implementation of abstract self-describing check.
    /// </summary>
    public abstract class BaseMatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMatcher"/> class.
        /// </summary>
        protected BaseMatcher()
        {
            Output = new StringBuilder();
        }

        /// <summary>
        /// Gets output which contains results check.
        /// </summary>
        public StringBuilder Output { get; }

        /// <summary>
        /// Gets verification description.
        /// </summary>
        public abstract string CheckDescription { get; }

        /// <summary>
        /// Gets or sets a value indicating whether need to reverse check results. <para/> 
        /// (used in complex checks combined with negating matchers)
        /// </summary>
        public bool Reverse { get; set; } = false;

        /// <summary>
        /// Describes mismatch with expected result (basically actual result).
        /// </summary>
        /// <param name="mismatch">mismatch description</param>
        public virtual void DescribeMismatch(string mismatch) =>
            Output.Append("was ").Append(mismatch);

        /// <summary>
        /// Gets check description.
        /// </summary>
        /// <returns>description string</returns>
        public override string ToString() =>
            CheckDescription;
    }
}
