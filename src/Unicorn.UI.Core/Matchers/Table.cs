using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    public static class Table
    {
        public static TableHasRowMatcher HasRow(string column, string cellValue)
        {
            return new TableHasRowMatcher(column, cellValue);
        }
    }
}
