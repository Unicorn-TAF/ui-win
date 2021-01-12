using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="IDataGrid"/> UI control has row with specified column. 
    /// </summary>
    public class DataGridHasColumnMatcher : TypeSafeMatcher<IDataGrid>
    {
        private readonly string _columnName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridHasColumnMatcher"/> class for specified column name.
        /// </summary>
        public DataGridHasColumnMatcher(string columnName)
        {
            _columnName = columnName;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"has column '{_columnName}'";

        /// <summary>
        /// Checks if data grid has specified column.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if data grid has specified column; otherwise - false</returns>
        public override bool Matches(IDataGrid actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            bool hasColumn = actual.HasColumn(_columnName);

            DescribeMismatch(hasColumn ? "having column" : "not having column");
            return hasColumn;
        }
    }
}
