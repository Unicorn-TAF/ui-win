using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    public static class DataGrid
    {
        public static DataGridHasRowMatcher HasRow(string column, string cellValue) 
            => new DataGridHasRowMatcher(column, cellValue);

        public static DataGridHasColumnMatcher HasColumn(string columnName)
            => new DataGridHasColumnMatcher(columnName);
    }
}
