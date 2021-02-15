using System;
using System.Collections.Generic;
using System.Linq;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.Synchronization;

namespace Unicorn.UI.Web.Controls.Dynamic
{
    /// <summary>
    /// Describes dynamically defined DataGrid (each sub-control could be defined using attribute).
    /// </summary>
    public class DynamicDataGrid : WebControl, IDynamicGrid
    {
        /// <summary>
        /// Gets current data grid rows count.
        /// </summary>
        public virtual int RowsCount => HasRows ? GetRows().Count : 0;

        /// <summary>
        /// Gets a value indicating whether grid has any row or not.
        /// </summary>
        public virtual bool HasRows => Locators.ContainsKey(GridElement.Row) ?
            TryGetChild<WebControl>(Locators[GridElement.Row]) :
            throw new NotSpecifiedLocatorException($"{nameof(GetRows)} data grid sub-control locator was not specified.");

        /// <summary>
        /// Gets dictionary of dd elements locators.
        /// </summary>
        protected Dictionary<GridElement, ByLocator> Locators = new Dictionary<GridElement, ByLocator>();

        /// <summary>
        /// Populates sub-elements locators from input dictionary.
        /// </summary>
        /// <param name="elementsLocators">sub-elements locators dictionary</param>
        public void Populate(Dictionary<int, ByLocator> elementsLocators)
        {
            foreach (var locator in elementsLocators)
            {
                var key = (GridElement)locator.Key;

                if (Locators.ContainsKey(key))
                {
                    Locators[key] = locator.Value;
                }
                else
                {
                    Locators.Add((GridElement)locator.Key, locator.Value);
                }
            }
        }

        /// <summary>
        /// Gets list of data grid headers.
        /// </summary>
        protected virtual IList<WebControl> GetHeaders() => Locators.ContainsKey(GridElement.Header) ?
            FindList<WebControl>(Locators[GridElement.Header]) :
            throw new NotSpecifiedLocatorException($"{nameof(GetHeaders)} data grid sub-control locator was not specified.");

        /// <summary>
        /// Gets list of data grid rows.
        /// </summary>
        protected virtual IList<Row> GetRows() => InitializeRows();

        /// <summary>
        /// Gets a value indicating whether data grid has column with specified name.
        /// </summary>
        /// <param name="columnName">column name to search for</param>
        /// <returns>true - if such column exists, otherwise - false</returns>
        public virtual bool HasColumn(string columnName) =>
            GetHeaders().Any(h => h.Text.Trim().Equals(columnName));

        /// <summary>
        /// Gets a value indicating whether data grid has row where specified column value is equal to expected.
        /// </summary>
        /// <param name="columnName">search column name</param>
        /// <param name="cellValue">expected value</param>
        /// <returns>true - if such row exists, otherwise - false</returns>
        public virtual bool HasRow(string columnName, string cellValue) =>
            HasRows && GetRows().Any(r => r.GetCell(GetColumnIndex(columnName)).Data.Equals(cellValue));

        /// <summary>
        /// Gets a value indicating whether data grid has specified cell in column in row where specified column value is equal to expected.
        /// </summary>
        /// <param name="searchColumnName">row search column name</param>
        /// <param name="searchCellValue">row search expected value</param>
        /// <param name="targetColumnName">target column name</param>
        /// <returns>true - if such cell exists, otherwise - false</returns>
        public virtual bool HasCell(string searchColumnName, string searchCellValue, string targetColumnName)
        {
            if (HasRows)
            {
                try
                {
                    GetCell(searchColumnName, searchCellValue, targetColumnName);
                    return true;
                }
                catch (ControlNotFoundException)
                {
                    // Do nothing, code will return a false in this case.
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether data grid has cell with specified value in column in row where specified column value is equal to expected.
        /// </summary>
        /// <param name="searchColumnName">row search column name</param>
        /// <param name="searchCellValue">row search expected value</param>
        /// <param name="targetColumnName">target column name</param>
        /// <param name="targetCellValue">target cell text</param>
        /// <returns>true - if such cell exists, otherwise - false</returns>
        public virtual bool HasCell(string searchColumnName, string searchCellValue, string targetColumnName, string targetCellValue)
        {
            if (HasRows)
            {
                try
                {
                    return GetCell(searchColumnName, searchCellValue, targetColumnName).Data.Equals(targetCellValue);
                }
                catch (ControlNotFoundException)
                {
                    // Do nothing, code will return a false in this case.
                }
            }

            return false;
        }

        /// <summary>
        /// Gets header control for column with specified name.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns><see cref="IControl"/> instance</returns>
        /// <exception cref="ControlNotFoundException">is thrown when specified column was not found</exception>
        public virtual IControl GetColumnHeader(string columnName)
        {
            var header = GetHeaders().FirstOrDefault(h => h.Text.Trim().Equals(columnName));

            return header == null ?
                throw new ControlNotFoundException($"Column '{columnName}' does not exist.") :
                header;
        }

        /// <summary>
        /// Gets data grid row by index.
        /// </summary>
        /// <param name="rowIndex">row index</param>
        /// <returns><see cref="IDataGridRow"/> instance</returns>
        /// <exception cref="ControlNotFoundException">is thrown when specified row was not found</exception>
        public virtual IDataGridRow GetRow(int rowIndex) =>
            HasRows && GetRows().Count >= rowIndex + 1 ?
            GetRows()[rowIndex] :
            throw new ControlNotFoundException($"Row with index '{rowIndex}' does not exist.");

        /// <summary>
        /// Gets data grid row where specified column value is equal to expected.
        /// </summary>
        /// <param name="columnName">search column name</param>
        /// <param name="cellValue">expected value</param>
        /// <returns><see cref="IDataGridRow"/> instance</returns>
        /// <exception cref="ControlNotFoundException">is thrown when specified row was not found</exception>
        public virtual IDataGridRow GetRow(string columnName, string cellValue)
        {
            if (HasRows)
            {
                var suitableRows = GetRows().Where(r => r.GetCell(GetColumnIndex(columnName)).Data.Equals(cellValue));
                
                if (suitableRows.Any())
                {
                    return suitableRows.First();
                }
            }

            throw new ControlNotFoundException($"Row where cell '{columnName}' = '{cellValue}' does not exist.");
        }

        /// <summary>
        /// Gets data grid row where value of clolumn by specified index is equal to expected.
        /// </summary>
        /// <param name="columnIndex">search column index</param>
        /// <param name="cellValue">expected value</param>
        /// <returns><see cref="IDataGridRow"/> instance</returns>
        /// <exception cref="ControlNotFoundException">is thrown when specified row was not found</exception>
        public virtual IDataGridRow GetRow(int columnIndex, string cellValue)
        {
            if (HasRows)
            {
                var suitableRows = GetRows().Where(r => r.GetCell(columnIndex).Data.Equals(cellValue));

                if (suitableRows.Any())
                {
                    return suitableRows.First();
                }
            }

            throw new ControlNotFoundException($"Row where cell with index {columnIndex} = '{cellValue}' does not exist.");
        }

        /// <summary>
        /// Gets a cell with specified index from row with specified index.
        /// </summary>
        /// <param name="rowIndex">row index</param>
        /// <param name="columnIndex">column index</param>
        /// <returns><see cref="IDataGridCell"/> instance</returns>
        /// <exception cref="ControlNotFoundException">is thrown when specified cell or row was not found</exception>
        public virtual IDataGridCell GetCell(int rowIndex, int columnIndex) =>
            GetRow(rowIndex).GetCell(columnIndex);

        /// <summary>
        /// Gets a cell for specified column from row where specified column value is equal to expected.
        /// </summary>
        /// <param name="searchColumnName">row search column name</param>
        /// <param name="searchCellValue">row search expected value</param>
        /// <param name="targetColumnName">target column name</param>
        /// <returns><see cref="IDataGridCell"/> instance</returns>
        /// <exception cref="ControlNotFoundException">is thrown when specified cell or row was not found</exception>
        public virtual IDataGridCell GetCell(string searchColumnName, string searchCellValue, string targetColumnName) =>
            GetRow(GetColumnIndex(searchColumnName), searchCellValue).GetCell(GetColumnIndex(targetColumnName));

        /// <summary>
        /// Waits for dropdown loader appearance for 1.5 seconds and then for its disappearance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="LoaderTimeoutException">thrown if loader has not disappeared during timeout period</exception>
        public virtual bool WaitForLoading(TimeSpan timeout)
        {
            if (Locators.ContainsKey(GridElement.Loader))
            {
                new LoaderHandler(
                    () => TryGetChild<WebControl>(Locators[GridElement.Loader]),
                    () => !TryGetChild<WebControl>(Locators[GridElement.Loader]))
                .WaitFor(timeout);
            }

            return true;
        }

        /// <summary>
        /// Describes dynamic data grid row element.
        /// </summary>
        public class Row : WebControl, IDataGridRow
        {
            private ByLocator cellsLocator;

            internal void Populate(ByLocator cellLocator) =>
                cellsLocator = cellLocator;

            /// <summary>
            /// Gets list of row cells controls.
            /// </summary>
            protected IList<Cell> Cells =>
                cellsLocator == null ?
                throw new NotSpecifiedLocatorException($"{nameof(Cells)} data grid sub-control locator is not specified.") :
                FindList<Cell>(cellsLocator);

            /// <summary>
            /// Gets row cell with specified index.
            /// </summary>
            /// <param name="index">cell index</param>
            /// <returns></returns>
            /// <exception cref="ControlNotFoundException">is thrown when specified cell was not found</exception>
            public virtual IDataGridCell GetCell(int index) =>
                Cells.Count >= index + 1 ? 
                Cells[index] : 
                throw new ControlNotFoundException($"Cell with index '{index}' does not exist.");
        }

        /// <summary>
        /// Describes dynamic grid cell element.
        /// </summary>
        public class Cell : WebControl, IDataGridCell
        {
            /// <summary>
            /// Gets cell string data.
            /// </summary>
            public virtual string Data => GetAttribute("innerText").Trim();
        }

        private int GetColumnIndex(string columnName)
        {
            int index = 0;

            foreach (var header in GetHeaders())
            {
                if (header.Text.Trim().Equals(columnName))
                {
                    return index;
                }

                index++;
            }

            throw new ControlNotFoundException($"Column '{columnName}' does not exist.");
        }
    
        private IList<Row> InitializeRows()
        {
            var rows = Locators.ContainsKey(GridElement.Row) ?
                FindList<Row>(Locators[GridElement.Row]) :  
                throw new ControlNotFoundException($"{nameof(GetRows)} data grid sub-control locator is not specified.");

            if (Locators.ContainsKey(GridElement.Cell))
            {
                foreach (var row in rows)
                {
                    row.Populate(Locators[GridElement.Cell]);
                }
            }

            return rows;
        }
    }
}
