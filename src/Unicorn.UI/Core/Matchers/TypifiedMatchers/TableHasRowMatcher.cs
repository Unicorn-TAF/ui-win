using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    public class DataGridHasRowMatcher : TypeSafeMatcher<IDataGrid>
    {
        private readonly string column;
        private readonly string cellValue;

        public DataGridHasRowMatcher(string column, string cellValue)
        {
            this.column = column;
            this.cellValue = cellValue;
        }

        public override string CheckDescription => $"has row where column '{this.column}' has value '{this.cellValue}'";

        public override bool Matches(IDataGrid actual)
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
