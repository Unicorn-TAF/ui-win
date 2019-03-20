using Unicorn.Taf.Core.Testing.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    public class TableHasColumnMatcher : TypeSafeMatcher<ITable>
    {
        private readonly string columnName;

        public TableHasColumnMatcher(string columnName)
        {
            this.columnName = columnName;
        }

        public override string CheckDescription => $"has column '{this.columnName}'";

        public override bool Matches(ITable actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            bool hasColumn = actual.HasColumn(this.columnName);

            DescribeMismatch(hasColumn ? "having column" : "not having column");
            return hasColumn;
        }
    }
}
