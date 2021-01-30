using Unicorn.UI.Core.Controls.Interfaces;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    public enum GridElement
    {
        Header = 1,
        Row = 2,
        Cell = 3,
        Loader = 4
    }

    public interface IDynamicGrid : IDynamicControl, IDataGrid, ILoadable
    {
        IControl GetColumnHeader(string columnName);
    }
}
