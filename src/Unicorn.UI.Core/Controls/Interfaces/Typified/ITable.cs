namespace Unicorn.UI.Core.Controls.Interfaces.Typified
{
    public interface ITable
    {
        ITableRow GetRow(string column, string cellValue);
    }

    public interface ITableRow
    {
    }
}
