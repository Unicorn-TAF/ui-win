using Unicorn.UI.Core.Controls.Interfaces;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    /// <summary>
    /// UI data grid sub-elements.
    /// </summary>
    public enum GridElement
    {
        /// <summary>
        /// Data grid header.
        /// </summary>
        Header = 1,

        /// <summary>
        /// Data grid row.
        /// </summary>
        Row = 2,

        /// <summary>
        /// Data grid cell.
        /// </summary>
        Cell = 3,

        /// <summary>
        /// Data grid content load indicator.
        /// </summary>
        Loader = 4
    }

    /// <summary>
    /// Interface for dynamically defined UI data grid.
    /// </summary>
    public interface IDynamicGrid : IDynamicControl, IDataGrid, ILoadable
    {
        IControl GetColumnHeader(string columnName);
    }
}
