using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    public class DataGridHasRowMatcher : TypeSafeMatcher<IDataGrid>
    {
        private readonly string _column;
        private readonly string _cellValue;

        public DataGridHasRowMatcher(string column, string cellValue)
        {
            _column = column;
            _cellValue = cellValue;
        }

        public override string CheckDescription => $"has row where column '{_column}' has value '{_cellValue}'";

        public override bool Matches(IDataGrid actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            bool hasRow = actual.HasRow(_column, _cellValue);

            DescribeMismatch(hasRow ? "having row" : "not having row");
            return hasRow;
        }
    }
}
