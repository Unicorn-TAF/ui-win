namespace Unicorn.UI.Core.Controls.Interfaces
{
    /// <summary>
    /// Describes direction of sorting
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Ascending sort direction.
        /// </summary>
        Ascending,

        /// <summary>
        /// Descending sort direction.
        /// </summary>
        Descending,

        /// <summary>
        /// Not sorted.
        /// </summary>
        NotSorted
    }

    /// <summary>
    /// Interface for controls which have functionality of data records sorting.
    /// </summary>
    public interface ISortable
    {
        /// <summary>
        /// Gets current sorting direction.
        /// </summary>
        SortDirection CurrentSorting
        {
            get;
        }

        /// <summary>
        /// Sort data with specified direction.
        /// </summary>
        /// <param name="direction"><see cref="SortDirection"/> value</param>
        /// <returns>true - if sorting was applied; false if already sorted in specified direction</returns>
        bool Sort(SortDirection direction);

        /// <summary>
        /// Get value indicating if data is sorted with specified direction.
        /// </summary>
        /// <param name="direction">sort direction</param>
        /// <returns>true - if sorted in correct direction; otherwise - false</returns>
        bool IsSorted(SortDirection direction);
    }
}
