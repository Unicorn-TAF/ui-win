using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    /// <summary>
    /// Entry point for DataGrid matchers.
    /// </summary>
    public static class DataGrid
    {
        /// <summary>
        /// Gets matcher to check if data grid contains row with specified value in specified column.
        /// </summary>
        /// <param name="column">column to search by</param>
        /// <param name="cellValue">expected cell value</param>
        /// <returns>matcher instance</returns>
        public static DataGridHasRowMatcher HasRow(string column, string cellValue) 
            => new DataGridHasRowMatcher(column, cellValue);

        /// <summary>
        /// Gets matcher to check if data grid contains specified column.
        /// </summary>
        /// <param name="columnName">expected data grid column</param>
        /// <returns>matcher instance</returns>
        public static DataGridHasColumnMatcher HasColumn(string columnName)
            => new DataGridHasColumnMatcher(columnName);
    }
}
