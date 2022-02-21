using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="IDataGrid"/> UI control has specified rows count. 
    /// </summary>
    public class DataGridHasRowsCountMatcher : TypeSafeMatcher<IDataGrid>
    {
        private readonly int _expectedRowsCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridHasRowsCountMatcher"/> class for specified rows count.
        /// </summary>
        /// <param name="expectedRowsCount">expected rows count</param>
        public DataGridHasRowsCountMatcher(int expectedRowsCount)
        {
            _expectedRowsCount = expectedRowsCount;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"has {_expectedRowsCount} rows";

        /// <summary>
        /// Checks if data grid has specified rows count.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if data grid has specified rows count; otherwise - false</returns>
        public override bool Matches(IDataGrid actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            int actualRowsCount = actual.RowsCount;
            bool isMatch = actualRowsCount == _expectedRowsCount;

            DescribeMismatch(actualRowsCount.ToString());
            return isMatch;
        }
    }
}
