using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    /// <summary>
    /// Entry point for DataGrid matchers.
    /// </summary>
    public class DataGridMatchers
    {
        /// <summary>
        /// Gets matcher to check if data grid has specified rows count.
        /// </summary>
        /// <param name="expectedRowsCount">expected rows count</param>
        /// <returns>matcher instance</returns>
        public DataGridHasRowsCountMatcher HasRowsCount(int expectedRowsCount) =>
            new DataGridHasRowsCountMatcher(expectedRowsCount);

        /// <summary>
        /// Gets matcher to check if data grid contains row with specified value in specified column.
        /// </summary>
        /// <param name="column">column to search by</param>
        /// <param name="cellValue">expected cell value</param>
        /// <returns>matcher instance</returns>
        public DataGridHasRowMatcher HasRow(string column, string cellValue) =>
            new DataGridHasRowMatcher(column, cellValue);

        /// <summary>
        /// Gets matcher to check if data grid contains specified column.
        /// </summary>
        /// <param name="columnName">expected data grid column</param>
        /// <returns>matcher instance</returns>
        public static DataGridHasColumnMatcher HasColumn(string columnName) =>
            new DataGridHasColumnMatcher(columnName);

        /// <summary>
        /// Gets matcher to check if data grid has cell located by text search having specified text.
        /// </summary>
        /// <param name="searchColumnName">name of column to locate target row</param>
        /// <param name="searchCellValue">cell text for search column to locate target row</param>
        /// <param name="targetColumnName">name of target column to get corresponding cell in target row</param>
        /// <param name="expectedCellText">expected text of target cell</param>
        /// <returns></returns>
        public static DataGridHasCellWithTextMatcher HasCellWithText(
            string searchColumnName, string searchCellValue, string targetColumnName, string expectedCellText) =>
            new DataGridHasCellWithTextMatcher(searchColumnName, searchCellValue, targetColumnName, expectedCellText);

        /// <summary>
        /// Gets matcher to check if data grid has cell located in specified coordinates having specified text.
        /// </summary>
        /// <param name="rowIndex">data grid row index</param>
        /// <param name="columnIndex">data grid column index</param>
        /// <param name="expectedCellText">expected text of target cell</param>
        /// <returns></returns>
        public static DataGridHasCellWithTextMatcher HasCellWithText(
            int rowIndex, int columnIndex, string expectedCellText) =>
            new DataGridHasCellWithTextMatcher(rowIndex, columnIndex, expectedCellText);
    }
}
