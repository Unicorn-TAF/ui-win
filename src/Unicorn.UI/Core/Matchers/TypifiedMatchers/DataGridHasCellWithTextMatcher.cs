using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="IDataGrid"/> UI control has cell located in specified 
    /// coordinates (indexses or search by text) having specified text.
    /// </summary>
    public class DataGridHasCellWithTextMatcher : TypeSafeMatcher<IDataGrid>
    {
        private readonly string _searchColumnName;
        private readonly string _searchCellValue;
        private readonly string _targetColumnName;
        private readonly string _expectedCellText;
        private readonly bool _useIndexes;
        private readonly int _rowIndex;
        private readonly int _columnIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridHasCellWithTextMatcher"/> class for specified cell 
        /// search data and cell expected text.
        /// </summary>
        /// <param name="searchColumnName">name of column to locate target row</param>
        /// <param name="searchCellValue">cell text for search column to locate target row</param>
        /// <param name="targetColumnName">name of target column to get corresponding cell in target row</param>
        /// <param name="expectedCellText">expected text of target cell</param>
        public DataGridHasCellWithTextMatcher(
            string searchColumnName, string searchCellValue, string targetColumnName, string expectedCellText)
        {
            _searchColumnName = searchColumnName;
            _searchCellValue = searchCellValue;
            _targetColumnName = targetColumnName;
            _expectedCellText = expectedCellText;
            _useIndexes = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridHasCellWithTextMatcher"/> class for 
        /// specified row and column indexes and cell expected text.
        /// </summary>
        /// <param name="rowIndex">data grid row index</param>
        /// <param name="columnIndex">data grid column index</param>
        /// <param name="expectedCellText">expected text of target cell</param>
        public DataGridHasCellWithTextMatcher(int rowIndex, int columnIndex, string expectedCellText)
        {
            _rowIndex = rowIndex;
            _columnIndex = columnIndex;
            _expectedCellText = expectedCellText;
            _useIndexes = true;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => _useIndexes ?
            $"has cell with index [{_columnIndex}, {_rowIndex}] with text '{_expectedCellText}'" :
            $"has cell '{_targetColumnName}' with text '{_expectedCellText}' in row where '{_searchColumnName}' = '{_searchCellValue}'";

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

            IDataGridCell cell = _useIndexes ?
                actual.GetCell(_rowIndex, _columnIndex) :
                actual.GetCell(_searchColumnName, _searchCellValue, _targetColumnName);

            string cellData = cell.Data;

            bool isMatch = cellData.Equals(_expectedCellText);

            DescribeMismatch(cellData);
            return isMatch;
        }
    }
}
