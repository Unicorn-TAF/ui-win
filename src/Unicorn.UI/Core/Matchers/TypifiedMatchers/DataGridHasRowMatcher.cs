using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="IDataGrid"/> UI control has row with specified cell value for specified column. 
    /// </summary>
    public class DataGridHasRowMatcher : TypeSafeMatcher<IDataGrid>
    {
        private readonly string _column;
        private readonly string _cellValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridHasRowMatcher"/> class for specified column name and cell value.
        /// </summary>
        public DataGridHasRowMatcher(string column, string cellValue)
        {
            _column = column;
            _cellValue = cellValue;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"has row where column '{_column}' has value '{_cellValue}'";

        /// <summary>
        /// Checks if data grid has specified row.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if data grid has specified row; otherwise - false</returns>
        public override bool Matches(IDataGrid actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            bool hasRow = actual.HasRow(_column, _cellValue);

            DescribeMismatch(hasRow ? "having row" : "not having row");
            return hasRow;
        }
    }
}
