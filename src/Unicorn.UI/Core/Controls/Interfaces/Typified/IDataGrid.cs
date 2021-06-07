namespace Unicorn.UI.Core.Controls.Interfaces.Typified
{
    /// <summary>
    /// Interface for base data grid implementation.
    /// </summary>
    public interface IDataGrid
    {
        /// <summary>
        /// Gets count of rows in data grid
        /// </summary>
        int RowsCount
        {
            get;
        }

        /// <summary>
        /// Gets value indicating whether data grid has specified column or not.
        /// </summary>
        /// <param name="columnName">column name to search for</param>
        /// <returns>true - if specified column exists, otherwise - false</returns>
        bool HasColumn(string columnName);

        /// <summary>
        /// Gets value indicating whether data grid has specified row or not.
        /// </summary>
        /// <param name="columnName">column name to search by</param>
        /// <param name="cellValue">value of corresponding cell in target column</param>
        /// <returns>true - if specified row exists, otherwise - false</returns>
        bool HasRow(string columnName, string cellValue);

        /// <summary>
        /// Gets value indicating whether data grid has specified cell or not.
        /// </summary>
        /// <param name="searchColumnName">column name to search row by</param>
        /// <param name="searchCellValue">value of corresponding cell in search column</param>
        /// <param name="targetColumnName">target column name to locate cell in found row</param>
        /// <returns>data grid cell instance</returns>
        bool HasCell(string searchColumnName, string searchCellValue, string targetColumnName);

        /// <summary>
        /// Get <see cref="IDataGridRow"/> with specified index from the data grid.
        /// </summary>
        /// <param name="rowIndex">row index</param>
        /// <returns>data grid row instance</returns>
        IDataGridRow GetRow(int rowIndex);

        /// <summary>
        /// Get <see cref="IDataGridRow"/> searched by cell value in specified column.
        /// </summary>
        /// <param name="columnName">column name to search by</param>
        /// <param name="cellValue">value of corresponding cell in target column</param>
        /// <returns>data grid row instance</returns>
        IDataGridRow GetRow(string columnName, string cellValue);

        /// <summary>
        /// Get <see cref="IDataGridRow"/> searched by cell value in column with specified index.
        /// </summary>
        /// <param name="columnIndex">index of column to search by</param>
        /// <param name="cellValue">value of corresponding cell in target column</param>
        /// <returns>data grid row instance</returns>
        IDataGridRow GetRow(int columnIndex, string cellValue);

        /// <summary>
        /// Get <see cref="IDataGridCell"/> specified coordinates (row and column indexes)
        /// </summary>
        /// <param name="rowIndex">index of row</param>
        /// <param name="columnIndex">index of column</param>
        /// <returns>data grid cell instance</returns>
        IDataGridCell GetCell(int rowIndex, int columnIndex);

        /// <summary>
        /// Get <see cref="IDataGridCell"/> specified coordinates (row and column indexes)
        /// </summary>
        /// <param name="searchColumnName">column name to search row by</param>
        /// <param name="searchCellValue">value of corresponding cell in search column</param>
        /// <param name="targetColumnName">target column name to locate cell in found row</param>
        /// <returns>data grid cell instance</returns>
        IDataGridCell GetCell(string searchColumnName, string searchCellValue, string targetColumnName);
    }

    /// <summary>
    /// Interface for base data grid row implementation.
    /// </summary>
    public interface IDataGridRow
    {
        /// <summary>
        /// Get cell with specified index.
        /// </summary>
        /// <param name="index">cell index</param>
        /// <returns>cell instance</returns>
        IDataGridCell GetCell(int index);
    }

    /// <summary>
    /// Interface for base data grid cell implementation.
    /// </summary>
    public interface IDataGridCell
    {
        /// <summary>
        /// Gets cell data
        /// </summary>
        string Data { get; }
    }
}
