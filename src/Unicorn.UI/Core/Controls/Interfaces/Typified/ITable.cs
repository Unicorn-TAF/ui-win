namespace Unicorn.UI.Core.Controls.Interfaces.Typified
{
    public interface ITable
    {
        bool HasColumn(string columnName);

        bool HasRow(string columnName, string cellValue);
    }
}
