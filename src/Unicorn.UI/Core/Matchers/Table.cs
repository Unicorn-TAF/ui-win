using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    public static class Table
    {
        public static TableHasRowMatcher HasRow(string column, string cellValue) 
            => new TableHasRowMatcher(column, cellValue);

        public static TableHasColumnMatcher HasColumn(string columnName)
            => new TableHasColumnMatcher(columnName);
    }
}
