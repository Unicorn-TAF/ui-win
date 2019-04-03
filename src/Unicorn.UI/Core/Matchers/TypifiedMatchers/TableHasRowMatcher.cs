using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    public class TableHasRowMatcher : TypeSafeMatcher<ITable>
    {
        private readonly string column;
        private readonly string cellValue;

        public TableHasRowMatcher(string column, string cellValue)
        {
            this.column = column;
            this.cellValue = cellValue;
        }

        public override string CheckDescription => $"has row where column '{this.column}' has value '{this.cellValue}'";

        public override bool Matches(ITable actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            bool hasRow = actual.HasRow(column, cellValue);

            DescribeMismatch(hasRow ? "having row" : "not having row");
            return hasRow;
        }
    }
}
