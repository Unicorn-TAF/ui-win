namespace Unicorn.UI.Core.Controls.Interfaces
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public interface ISortable
    {
        SortDirection CurrentSorting
        {
            get;
        }

        bool Sort(SortDirection direction);
    }
}
